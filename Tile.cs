using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSProject
{
    public struct Tile
    {
        public double PenetrationCoef { get; private set; }
        public Bitmap Image { get; private set; }
        public Point Position;

        public const int width = 50;
        public const int height = 50;
        public Rectangle Rectangle { get; private set; }

        public Tile(double penetrationCoef, string image)
        {
            PenetrationCoef = penetrationCoef;
            Image = new Bitmap(image);
            Position = new Point(0, 0);
            Rectangle = new Rectangle(Position, new Size(width, height));
        }

        public void SetPosition(Point position)
        {
            Position = position;
            Rectangle = new Rectangle(Position, new Size(width, height));
        }
    }
}
