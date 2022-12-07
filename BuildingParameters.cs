using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSProject
{
    public struct BuildingParameters
    {
        int hp;
        Price price;

        public BuildingParameters(int hp, Price price)
        {
            this.hp = hp;
            this.price = price;
        }
    }
}
