using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form.AI
{
    internal struct Range
    {
        public int Start;
        public int End;
        public Range(int a,int b)
        {
            Start = a;
            End = b;
        }
    }
    //R = 113
    //

    internal partial class GameAI
    {
        public delegate void ShotEvent(Point shotLocation);
        public event ShotEvent OnShotChoose;

        public Rectangle PlayEnvironment;
        public Range NextStepDelay = new Range(1,3);

        Random globalrandom = new Random(DateTime.Now.Second);
        Random stepdelyarand = new Random(DateTime.Now.Second * 7);
        Random selectmethodrand = new Random(DateTime.Now.Second * 3);

        Grader grader;

        List<Tank> tanks = new List<Tank>();

        public GameAI(Rectangle PlayEnvironment , List<Tank> EnemyTanks)
        {
            grader = new Grader(PlayEnvironment, 50);
            tanks = EnemyTanks;
        }

        public void NextStep()
        {
            Trace.WriteLine($"AI: Choosing step ...");

            switch (GameManager.Status)
            {
                case GameManager.GameStatus.NotStarted:
                    return;

                case GameManager.GameStatus.YourTurn:
                    return;
                case GameManager.GameStatus.Over:
                    return;
                default:
                    break;
            }

            Task.Run(() =>
            {

                Thread.Sleep(stepdelyarand.Next(NextStepDelay.Start * 1000,NextStepDelay.End * 1000));

                int selectmethod = selectmethodrand.Next(100);

                RandomSelect();


                //if (selectmethod >= 45)
                //    CenterSelect();
                //if (selectmethod < 45 && selectmethod >= 10)
                //    RandomSelect();
                //else
                //    AccurateSelect();
            });
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

            if (OnShotChoose != null)
                OnShotChoose(grader[x, y].Location);

            grader[x, y].Fill();

            logShot(grader[x,y].Location);

        }
        void CenterSelect()
        {
            float scale = 0.7f;

            Point Center = new Point((PlayEnvironment.Width/2) + PlayEnvironment.X, (PlayEnvironment.Height/2) + PlayEnvironment.Y);
            int Rad = (int)(((Math.Sqrt(Math.Pow(PlayEnvironment.Width,2) + Math.Pow(PlayEnvironment.Height,2))) / 2) * scale);


        }
        void AccurateSelect()
        {

        }

        void logShot(Point location)
        {
            Trace.WriteLine($"AI: RandomSelect: ({location.X},{location.Y})");
        }
    }
}
