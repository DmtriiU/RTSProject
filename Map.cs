using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTSProject
{
    public static class Map
    {
        public static Point origin = new Point(0, 0);

        static readonly string name;

        static readonly int width;

        static readonly int height;
        public static Tile[,] Tiles { get; private set; }
        public static List<Unit> Units { get; private set; }
        public static List<Building> Buildings { get; private set; }

        public static List<Tile> GetNextTiles(int i, int j)
        {
            List<Tile> nextTiles = new List<Tile>();
            var dirs = new int[][] { new [] { -1, 0 }, new [] { 0, -1 }, new [] { 1, 0 }, new [] { 0, 1 }, new [] { -1, -1 }, new [] { 1, -1 }, new[] { 1, 1 }, new[] { -1, 1 } };

            bool CheckNextTile(int x, int y)
            {
                foreach (var unit in Units)
                {
                    if (unit.position.X == x  * 50 && unit.position.Y == y  * 50)
                    {
                        return false;
                    }
                }
                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    return true;
                }
                return false;
            }

            for (int k = 0; k < dirs.Length; k++)
            {
                if (CheckNextTile(i + dirs[k][0], j + dirs[k][1]))
                {
                    nextTiles.Add(Tiles[i + dirs[k][0], j + dirs[k][1]]);
                }
            }
            return nextTiles;
        }

        public static List<Point> GetPath(PointF p1, Point p2)
        {
            var visited = new Dictionary<Tile, Tile>();
            var queue = new Queue<Tile>();
            var tile = Tiles[((int)p1.X) / 50, ((int)p1.Y) / 50];
            visited.Add(tile, new Tile());
            queue.Enqueue(tile);
            while (queue.Count > 0)
            {
                var currentTile = queue.Dequeue();
                if ((currentTile.Position.X + origin.X) / 50 == (p2.X) / 50 && (currentTile.Position.Y + origin.Y) / 50 == (p2.Y) / 50)
                {
                    break;
                }
                var nextTiles = GetNextTiles((currentTile.Position.X + origin.X) / 50, (currentTile.Position.Y + origin.Y) / 50);
                foreach (var nextTile in nextTiles)
                {
                    if (!visited.ContainsKey(nextTile))
                    {
                        queue.Enqueue(nextTile);
                        visited[nextTile] = currentTile;
                    }
                }
            }
            var pathHead = Tiles[p2.X / 50, p2.Y / 50];
            var pathSegment = Tiles[p2.X / 50, p2.Y / 50];
            var path = new List<Point>();
            while (visited.ContainsKey(pathHead) && visited.ContainsKey(pathSegment))
            {

                var p = new Point(pathSegment.Position.X + origin.X, pathSegment.Position.Y + origin.Y);
                path.Add(p);               
                //path.Add(pathSegment.Position);
                pathSegment = visited[pathSegment];
            }
            path.Reverse();
            return path;
        }

        public static Tile[,] GetTiles(Dictionary<string, object> mapData)
        {
            var tiles = new Tile[width, height];
            var tileData = File.ReadAllText(@"data\tiles.txt").Split(' ');
            var mapTiles = mapData["map"].ToString().Split(' ');
            int k = 0;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile(Convert.ToDouble(tileData[Convert.ToInt32(mapTiles[k])]), $@"data\images\tile{Convert.ToInt32(mapTiles[k]) + 1}.png");
                    k++;
                }
            }
            return tiles;
        }

        public static List<Unit> GetUnits(Dictionary<string, object> mapData)
        {
            var units = new List<Unit>();
            var unitsData = File.ReadAllText(@"data\units.txt").Split('\n', ':');
            var unitData = new Dictionary<string, Dictionary<string, object>>();
            int k = 0;
            for (int i = 0; i < unitsData.Length - 1; i += 18)
            {
                k++;
                var dict = new Dictionary<string, object>();
                for (int j = i + 2; j < 18 * k; j += 2)
                {
                    dict.Add(unitsData[j].Trim(), unitsData[j + 1].Trim('\r', ' '));
                }
                unitData.Add(unitsData[i + 1].Trim(), dict);
            }
            var mapUnits = mapData["units"].ToString().Split(' ');
            for (int i = 0; i < mapUnits.Length; i += 3)
            {
                var hp = Convert.ToInt32(unitData[mapUnits[i]]["hp"]);
                var damage = Convert.ToInt32(unitData[mapUnits[i]]["damage"]);
                var attack = Convert.ToInt32(unitData[mapUnits[i]]["attack"]);
                var speed = (float)Convert.ToDouble(unitData[mapUnits[i]]["speed"]);
                var view = Convert.ToInt32(unitData[mapUnits[i]]["view"]);
                var price = new Price(Convert.ToInt32(unitData[mapUnits[i]]["price"].ToString().Split()[0]),
                    Convert.ToInt32(unitData[mapUnits[i]]["price"].ToString().Split()[1]));
                var kills = Convert.ToInt32(unitData[mapUnits[i]]["kills"]);

                var unitParams = new UnitParameters(hp, damage, attack, speed, view, price, kills, false);

                var unitPosition = new Point(Convert.ToInt32(mapUnits[i + 1]), Convert.ToInt32(mapUnits[i + 2]));
                units.Add(new Unit(mapUnits[i], "enemyunit", unitParams, unitPosition, $@"data\images\{mapUnits[i]}.png"));
            }
            mapUnits = mapData["enemyunits"].ToString().Split(' ');
            for (int i = 0; i < mapUnits.Length; i += 3)
            {
                var hp = Convert.ToInt32(unitData[mapUnits[i]]["hp"]);
                var damage = Convert.ToInt32(unitData[mapUnits[i]]["damage"]);
                var attack = Convert.ToInt32(unitData[mapUnits[i]]["attack"]);
                var speed = (float)Convert.ToDouble(unitData[mapUnits[i]]["speed"]);
                var view = Convert.ToInt32(unitData[mapUnits[i]]["view"]);
                var price = new Price(Convert.ToInt32(unitData[mapUnits[i]]["price"].ToString().Split()[0]),
                    Convert.ToInt32(unitData[mapUnits[i]]["price"].ToString().Split()[1]));
                var kills = Convert.ToInt32(unitData[mapUnits[i]]["kills"]);

                var unitParams = new UnitParameters(hp, damage, attack, speed, view, price, kills, true);

                var unitPosition = new Point(Convert.ToInt32(mapUnits[i + 1]), Convert.ToInt32(mapUnits[i + 2]));
                units.Add(new Unit(mapUnits[i], "unit", unitParams, unitPosition, $@"data\images\enemy {mapUnits[i]}.png"));
            }
            return units;
        }

        public static List<Building> GetBildings(Dictionary<string, object> mapData)
        {
            var buildings = new List<Building>();
            var buildingsData = File.ReadAllText(@"data\buildings.txt").Split('\n', ':');
            var buildingData = new Dictionary<string, Dictionary<string, object>>();
            int k = 0;
            for (int i = 0; i < buildingsData.Length - 1; i += 12)
            {
                k++;
                var dict = new Dictionary<string, object>();
                for (int j = i + 2; j < 12 * k; j += 2)
                {
                    dict.Add(buildingsData[j].Trim(), buildingsData[j + 1].Trim('\r', ' '));
                }
                buildingData.Add(buildingsData[i + 1].Trim(), dict);
            }
            var mapBuildings = mapData["buildings"].ToString().Split(' ');
            for (int i = 0; i < mapBuildings.Length; i += 3)
            {
                var hp = Convert.ToInt32(buildingData[mapBuildings[i]]["hp"]);
                var price = new Price(Convert.ToInt32(buildingData[mapBuildings[i]]["price"].ToString().Split()[0]), Convert.ToInt32(buildingData[mapBuildings[i]]["price"].ToString().Split()[1]));
                var size = new Size(Convert.ToInt32(buildingData[mapBuildings[i]]["size"].ToString().Split()[0]), Convert.ToInt32(buildingData[mapBuildings[i]]["size"].ToString().Split()[1]));
                var buildingParams = new BuildingParameters(hp, price);

                var buildingPosition = new Point(Convert.ToInt32(mapBuildings[i + 1]), Convert.ToInt32(mapBuildings[i + 2]));
                buildings.Add(new Building(mapBuildings[i], "building", buildingParams, buildingPosition, size, $@"data\images\{mapBuildings[i]}.png"));
            }
            return buildings;
        }

        static Map()
        {
            var data = File.ReadAllText(@"data\maps\map1.txt").Split('\n', ':');
            var mapData = new Dictionary<string, object>();

            for (int i = 0; i < data.Length - 1; i += 2)
            {
                mapData.Add(data[i].Trim(), data[i + 1].Trim('\r', ' '));
            }

            name = mapData["name"].ToString();
            width = Convert.ToInt32(mapData["width"]);
            height = Convert.ToInt32(mapData["height"]);
            //tiles = GetTiles(mapData);
            Units = GetUnits(mapData);
            Buildings = GetBildings(mapData);

            name = "Map1";
            width = 26;
            height = 20;
            Tiles = new Tile[width, height];
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    int t = (i + j) % 3 + 1;
                    Tiles[i, j] = new Tile(t, $"data\\images\\tile{1}.png");
                }
            }
        }

        public static void DrawRect(Graphics graphics, Rectangle rectangle)
        {
            rectangle.X -= origin.X;
            rectangle.Y -= origin.Y;
            graphics.DrawRectangle(new Pen(Color.Black, 2), rectangle);
        }

        public static void DrawTargetPoint(Graphics graphics, Point targetPoint)
        {
            graphics.DrawEllipse(new Pen(Color.White, 2), targetPoint.X - origin.X, targetPoint.Y - origin.Y, 2, 2);
        }

        public static void Draw(Graphics graphics)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Point point = new Point(i * 50, j * 50);
                    //if (IsVisible(new Rectangle(point, tiles[i, j].Size)))
                    {
                        Tiles[i, j].SetPosition(new Point(point.X - origin.X, point.Y - origin.Y));
                        graphics.DrawImage(Tiles[i, j].Image, Tiles[i, j].Position);
                    }
                }
            }
            foreach (var unit in Units)
            {
                unit.Draw(graphics, origin);
            }
        }

        public static bool IsVisible(Rectangle rectangle)
        {
            Rectangle screen = new Rectangle(origin, new Size(1000, 600));
            return screen.IntersectsWith(rectangle);
        }

        public static double GetPenetrationCoef(PointF unitLocation)
        {
            foreach (var tile in Tiles)
            {
                if (tile.Rectangle.Contains(new Point((int)unitLocation.X + Tiles[0, 0].Position.X, (int)unitLocation.Y + Tiles[0, 0].Position.Y)))
                {
                    return tile.PenetrationCoef;
                }
            }
            return 0;
        }

        public static void Refresh(Point cursorPosition)
        {
            if (cursorPosition.X < 10)
            {
                origin.X -= 10;
            }
            else if (cursorPosition.X > 1000 - 50)
            {
                origin.X += 10;
            }
            if (cursorPosition.Y < 10)
            {
                origin.Y -= 10;
            }
            else if (cursorPosition.Y > 600 - 50)
            {
                origin.Y += 10;
            }
            if (origin.X < 0)
                origin.X = 0;
            if (origin.Y < 0)
                origin.Y = 0;
            if (origin.X / 50 + 1000 / 50 >= width)
                origin.X = width * 50 - 20 * 50;
            if (origin.Y / 50 + 600 / 50 >= height)
                origin.Y = height * 50 - 12 * 50;
        }

        public static void Update()
        {
            foreach (var unit in Units)
                unit.Update();
        }

        public static GameObject Find(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
