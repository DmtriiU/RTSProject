using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSProject
{
    public abstract class GameObject
    {
        public string type;
        public string name;
        public Point position;
        public Bitmap image;

        public abstract void Draw(Graphics graphics, Point offset);

        public abstract void Update();
    }
}
