using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form.AI
{
    internal partial class GameAI
    {
        class Grader
        {
            public Spot[,] SpotMatrix;

            public int width
            {
                get { return SpotMatrix.GetLength(0);}
            }
            public int height
            {
                get { return SpotMatrix.GetLength(1);}
            }

            public Spot this[int x , int y]
            {
                get
                {
                    return SpotMatrix[x , y];
                }
                set
                {
                    SpotMatrix[x , y] = value;
                }
            }

            public Grader(Rectangle ground,int Radias)
            {
                Grade(ground, Radias, out SpotMatrix);
            }

            void Grade(Rectangle ground,int rad , out Spot[,] spots)
            {
                int x = ground.Width / rad;
                int y = ground.Height / rad;

                int ExtraX = ((ground.Width % rad) / 2) + ground.X;
                int ExtraY =((ground.Height % rad) / 2) + ground.Y;


                spots = new Spot[x,y];

                int F = (rad / 2);

                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                        spots[i, j] = ExactSpot(rad,ExtraX,ExtraY,i,j,F);
            }

            Spot ExactSpot(int rad, int ExtraX,int ExtraY, int i,int j, int F)
            {
                return new Spot(new Point(Fx(ExtraX, rad, i, F), Fx(ExtraY, rad, j, F)));
            }
            int Fx(int Extra,int rad,int x, int F)
            {
                return Extra + ((rad * (x+1)) - F);
            }
        }
    }
}
