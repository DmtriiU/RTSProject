using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSProject
{
    public struct UnitParameters
    {
        public int hp;
        public int damage;
        public int attackRadius;
        public float speed;
        public int view;
        public Price price;
        public int kills;
        public bool enemy;

        public UnitParameters(int hp, int damage, int attackRadius, float speed, int view, Price price, int kills, bool enemy)
        {
            this.hp = hp;
            this.damage = damage;
            this.attackRadius = attackRadius;
            this.speed = speed;
            this.view = view;
            this.price = price;
            this.kills = kills;
            this.enemy = enemy;
        }
    }
}
