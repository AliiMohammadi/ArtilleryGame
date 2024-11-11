using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form
{
    internal struct Shot
    {
        public Point StrikePoint { get; set; }
        public string Victom { get; set; }

        public Shot(Point Strikepoint, string victom)
        {
            StrikePoint = Strikepoint;
            Victom = victom;
        }
    }
}
