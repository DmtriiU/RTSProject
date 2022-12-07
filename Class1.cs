using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSProject
{
    public class Class1
    {
        /// <summary>
        /// Определяет пересекаются ли два указанных прямоугольника.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns>true если прямоугольники пересекаются; в противном случае — false.</returns>
        public static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            if (r1.Top > r2.Bottom || r2.Top > r1.Bottom || r1.Left > r2.Right || r2.Left > r1.Right)
            {
                return false;
            }
            return true;
        }
    }
}
