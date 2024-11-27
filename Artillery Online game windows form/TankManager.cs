using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Web.Script.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Artillery_Online_game_windows_form
{
    internal class TankManager
    {
        public static List<Tank> ArmyTanks1 = new List<Tank>();
        public static List<Tank> ArmyTanks2 = new List<Tank>();

        static Random rand = new Random(DateTime.Now.Second);

        public static Tank GetTankAt(Point location)
        {
            return GetTankAt(location ,7);
        }
        public static Tank GetTankAt(Point location , uint Hitradias)
        {
            if (PlayGround.IsInTheGround1(location))
                return GetTankAt(ArmyTanks1, location, Hitradias);
            else
            if (PlayGround.IsInTheGround2(location))
                return GetTankAt(ArmyTanks2, location, Hitradias);

            return null;
        }

        public static Tank GetTankAt(List<Tank> ArmyTanks,Point Loc , uint Hitradias)
        {
            foreach (var tank in ArmyTanks)
                if (Physics.IsColliding(tank.Possition, Loc, tank.TankCollitionRadias, Hitradias))
                    return tank;

            return null;
        }

        public static void RepositionTanks()
        {
            foreach (Tank tank in ArmyTanks1)
                tank.Possition = GetRandomPositionForTanks1(tank);

            foreach (Tank tank in ArmyTanks2)
                tank.Possition = GetRandomPositionForTanks2(tank);
        }

        public static void Destroy(Tank victom)
        {
            if(victom.ArmyNumber == 1 )
                ArmyTanks1.Remove(victom);
            else
                ArmyTanks2.Remove(victom);

            victom.Destroy();
        }

        static Point GetRandomPositionForTanks1(Tank tank)
        {

            Point newpos;
            int tankrad = (int)tank.TankCollitionRadias;
            int mx = PlayGround.Ground.Width - tankrad;
            int my = PlayGround.Ground.Height - tankrad;

            while (true)
            {
                int x = rand.Next(tankrad, mx);
                int y = rand.Next(PlayGround.Ground1.Y + tankrad, my);

                if (x < mx && y < my)
                {
                    newpos = new Point(x, y);

                    if (TankManager.GetTankAt(newpos, (uint)(tankrad * 1.2f)) == null)
                        return newpos;
                }
            }
        }
        static Point GetRandomPositionForTanks2(Tank tank)
        {
            Point newpos;

            int tankrad = (int)tank.TankCollitionRadias;
            int mx = PlayGround.Ground2.Width - tankrad - 50;
            int my = PlayGround.Ground2.Height - tankrad - 50;

            while (true)
            {
                int x = rand.Next(tankrad, mx);
                int y = rand.Next(tankrad, my);

                if (x < mx && y < my)
                {
                    newpos = new Point(x, y);

                    if (TankManager.GetTankAt(newpos, (uint)(tankrad * 1.2f)) == null)
                        return newpos;
                }
            }

        }
    }
}
