using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artillery_Online_game_windows_form
{
    internal class PlayGround
    {
        public static PictureBox Image;
        public static PictureBox MissedFierMarkImage;
        public static readonly Rectangle Ground = new Rectangle(0,0,512,695);
        public static readonly Rectangle Ground1 = new Rectangle(0,347,512, 347);
        public static readonly Rectangle Ground2 = new Rectangle(0,0,512, 347);

        public static bool IsInTheGround1(Point hit)
        {
            return Isintheground(1,hit);
        }
        public static bool IsInTheGround2(Point hit)
        {
            return Isintheground(2, hit);
        }
        /// <summary>
        /// یک پوزیشن از یه جای زمنیمیگیره. متقارن اون پوزیشن رو توی زمین مقابل برمیگردونه
        /// </summary>
        /// <param name="possition"></param>
        /// <returns></returns>
        public static Point MirrorPosition(Point possition)
        {
            return new Point(Ground.Width - possition.X, Ground.Height - possition.Y);
        }

        static bool Isintheground(byte groundindex,Point hit)
        {
            if(groundindex == 2)
                if (0 < hit.X && hit.X <= Ground2.Width)
                    if (0 < hit.Y && hit.Y <= Ground2.Height)
                        return true;
            if (groundindex == 1)
                if (0 < hit.X && hit.X <= Ground1.Width)
                    if (Ground1.Height < hit.Y && hit.Y <= Ground1.Height * 2)
                        return true;

            return false;
        }
    }
}
