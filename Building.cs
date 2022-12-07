using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSProject
{
    public class Building : GameObject
    {
        readonly string type;
        readonly string name;
        BuildingParameters parameters;
        Point position;
        Size size;
        string image;

        public Building(string type, string name, BuildingParameters parameters, Point position, Size size, string image)
        {
            this.type = type;
            this.name = name;
            this.parameters = parameters;
            this.position = position;
            this.size = size;
            this.image = image;
        }

        public override void Draw(Graphics graphics, Point offset)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
