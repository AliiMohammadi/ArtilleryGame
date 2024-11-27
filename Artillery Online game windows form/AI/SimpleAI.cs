using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form.AI
{
    internal partial class SimpleAI : GameAI
    {

        Random globalrandom = new Random(DateTime.Now.Second);
        Random selectmethodrand = new Random(DateTime.Now.Second * 3);

        Grader grader;

        public SimpleAI(Rectangle evo , List<Tank> tank) : base(evo,tank)
        {
            grader = new Grader(PlayEnvironment, 50);
        }

        protected override void Action()
        {
            int selectmethod = selectmethodrand.Next(100);

            RandomSelect();

            //if (selectmethod >= 45)
            //    CenterSelect();
            //if (selectmethod < 45 && selectmethod >= 10)
            //    RandomSelect();
            //else
            //    AccurateSelect();
        }

        void RandomSelect()
        {
            int x;
            int y;

            while (true)
            {
                x = globalrandom.Next(grader.width);
                y = globalrandom.Next(grader.height);

                if (!grader[x, y].Filled)
                    break;
            }

            Resutl(grader[x, y].Location);

            grader[x, y].Fill();

            logShot(grader[x, y].Location);

        }
        void CenterSelect()
        {
            float scale = 0.7f;

            Point Center = new Point((PlayEnvironment.Width / 2) + PlayEnvironment.X, (PlayEnvironment.Height / 2) + PlayEnvironment.Y);
            int Rad = (int)(((Math.Sqrt(Math.Pow(PlayEnvironment.Width, 2) + Math.Pow(PlayEnvironment.Height, 2))) / 2) * scale);


        }
        void AccurateSelect()
        {

        }
    }
}
