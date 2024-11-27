using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form.AI
{
    internal partial class SimpleAI
    {
        struct Spot
        {
            public Point Location { get; set; }
            public bool Filled { get; set; }

            public Spot(Point location)
            {
                Location = location;
                Filled = false;
            }

            public void Fill()
            {
                Filled = true;
            }
        }
    }
}
