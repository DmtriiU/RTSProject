using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTSProject
{
    public class Unit : GameObject
    {
        readonly string type;
        readonly string name;
        public UnitParameters Parameters { get; private set; }
        public PointF position;
        public PointF speed;
        Point target;
        Point? tile_target;
        public List<Point> Path { get; private set; }
        int current_node = 1;

        public List<Vector2> Vectors { get; private set; }
        public Bitmap Image { get; private set; }

        public Unit(string type, string name, UnitParameters parameters, Point position, string image)
        {
            this.type = type;
            this.name = name;
            this.Parameters = parameters;
            this.position = position;
            this.Image = new Bitmap(image);
            this.tile_target = null;
        }

        public override void Draw(Graphics graphics, Point offset)
        {
            graphics.DrawImage(Image, new PointF(position.X - offset.X/* - unit.image.Width / 2*/, position.Y - offset.Y/* - unit.image.Height / 2*/));
        }
      
        //public void Update(Unit unit, Point targetPoint)
        //{
        //    unit.position = new PointF(unit.position.X - Map.origin.X, unit.position.Y - Map.origin.Y);
        //    targetPoint = new Point(targetPoint.X - Map.origin.X, targetPoint.Y - Map.origin.Y);
        //    Path = Map.GetPath(unit.position, targetPoint);
        //    Vectors = new List<Vector2>();

        //    for (int i = 0; i < Path.Count - 1; i++)
        //    {
        //        Vectors.Add(new Vector2(Path[i + 1].X - Path[i].X, Path[i + 1].Y - Path[i].Y));
        //    }
        //}
        
        public void SetTarget(Point targetPoint)
        {
            target = targetPoint;

            Path = Map.GetPath(position, target);
            if (Path != null && Path.Count > 1)
                tile_target = Path[1];
        }

        public override void Update()
        {          
            UpdateTarget();
            /*if (NearTarget())
            {
                
            }*/
            if (tile_target != null)
            {
                var penetratiobCoef = Map.GetPenetrationCoef(position);
                Point t = (Point)tile_target;
                double dx = t.X - position.X;
                double dy = t.Y - position.Y;
                double len = Math.Sqrt(dx * dx + dy * dy);
                if (len < 1)
                    len = 1;
                speed.X = (float)(/*penetratiobCoef * 2 * */5 * dx / len/* * 5*/);
                speed.Y = (float)(/*penetratiobCoef * 2 * */5 * dy / len/* * 5*/);
            }
            position.X += speed.X;
            position.Y += speed.Y;
        }

        private void UpdateTarget()
        {
            if (Path != null)
            {
                if (NearTarget())
                {
                    current_node++;
                    if (current_node == Path.Count)
                    {
                        Point t = (Point)tile_target;
                        speed.X = 0;
                        speed.Y = 0;
                        position.X = t.X;
                        position.Y = t.Y;
                        tile_target = null;
                        current_node = 1;
                    }
                    else
                        tile_target = Path[current_node];
                }
            }
        }

        private bool NearTarget()
        {
            if (tile_target != null)
            {
                Point t = (Point)tile_target;
                double dx = t.X - position.X;
                double dy = t.Y - position.Y;
                double len = Math.Sqrt(dx * dx + dy * dy);
                return len < 10;
            } else
                return false;
        }

        public void Attack(GameObject target)
        {

        }

        //public void Move(Unit unit, PointF location)
        //{
        //    var penetratiobCoef = Map.GetPenetrationCoef(unit.position);

        //    var vector = new Vector2(location.X - unit.position.X, location.Y - unit.position.Y);


        //    unit.position.X += (float)penetratiobCoef * unit.parameters.speed * vector.X / vector.Length();
        //    unit.position.Y += (float)penetratiobCoef * unit.parameters.speed * vector.Y / vector.Length();

        //}

    }
}

