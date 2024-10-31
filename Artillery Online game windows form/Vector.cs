using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artillery_Online_game_windows_form
{
    internal class Vector
    {
        public static Point CenterOf(PictureBox picutre)
        {
            return new Point(picutre.Location.X + picutre.Height / 2, picutre.Location.Y + picutre.Width / 2);
        }
        public static Point ConvertPositionToWorld(PictureBox picutre,Point Position)
        {
            return new Point(Position.X - picutre.Height / 2, Position.Y - picutre.Width / 2);
        }
    }
}
