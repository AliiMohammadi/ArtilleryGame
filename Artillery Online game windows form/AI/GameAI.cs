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

    internal abstract class GameAI
    {
        public delegate void ShotEvent(Point shotLocation);
        public event ShotEvent OnShotChoose;

        public Rectangle PlayEnvironment;
        public Range NextStepDelay = new Range(1,3);

        public List<Tank> EnemyTanlks = new List<Tank>();

        Random stepdelyarand = new Random(DateTime.Now.Second * 7);

        public GameAI(Rectangle Playenvironment , List<Tank> EnemyTanks)
        {
            EnemyTanlks = EnemyTanks;
            PlayEnvironment = Playenvironment;
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

                Action();
            });
        }
        protected abstract void Action();

        protected void Resutl (Point Loc)
        {
            if (OnShotChoose != null)
                OnShotChoose(Loc);
        }

        protected void logShot(Point location)
        {
            Trace.WriteLine($"AI: RandomSelect: ({location.X},{location.Y})");
        }
    }
}
