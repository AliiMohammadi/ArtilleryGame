using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Artillery_Online_game_windows_form
{
    internal class Physics
    {
        public static bool IsColliding(Point Object1, Point Object2,uint radias1, uint radias2)
        {
            int distace = Distance(Object1,Object2) - (int)(radias1 + radias2);

            if (distace <= 0)
                return true;

            return false;
        }

        public static int Distance(Point p1,Point p2)
        {
            return (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
