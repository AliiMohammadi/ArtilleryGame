using Artillery_Online_game_windows_form.AI;
using OneeChanRemake.Operation_System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Artillery_Online_game_windows_form
{
    internal class GameManager
    {
        public static Action OnGameOver;
        public static Action OnWin;

        public enum GameStatus
        {
            NotStarted,YourTurn,EnemyTurn,Over
        }
        public static GameStatus Status = GameStatus.NotStarted;
        public enum GameMode
        {
            Offline,Online
        }
        public static GameMode Mode = GameMode.Offline;

        /// <summary>
        /// شماره بازی کن این کلاینت
        /// </summary>
        public static int PlayerNumber = -1;

        static GameAI AI;
        static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static void CreatAI()
        {
            AI = new GameAI(PlayGround.Ground1,TankManager.ArmyTanks1);
            AI.OnShotChoose += Botshot;
        }
        public static void ShotAt(Point hit)
        {
            switch (Mode)
            {
                case GameMode.Offline:
                    switch(Status)
                    {
                        case GameStatus.YourTurn:

                            if (!ShotPremition(hit))
                                return;

                            AnalisisShotOfflineMode(hit);

                            if (TankManager.ArmyTanks2.Count == 0)
                            {
                                Status = GameStatus.Over;
                                OnWin();
                            }
                            else
                            {
                                Status = GameStatus.EnemyTurn;
                                AI.NextStep();
                            }

                            break;
                    }
                    break;
                case GameMode.Online:
                    AnalisisShotOnlienMode(hit);

                    break;
                default:
                    break;
            }
        }
        public static void RepositionTanks()
        {
            TankManager.RepositionTanks();
        }
        /// <summary>
        /// موقعیت تانک هارو برای سرور میفرسته از اون طرفم سرور برای اونیکی بازیکن میفرسته تا تانک ها براساس بازی این کلاین ست بشه و دیترمین بشه.
        /// </summary>
        public static void SendTankStatusToServer()
        {
            List<Point> tankspossitions = new List<Point>();

            foreach (var item in TankManager.ArmyTanks1)
                tankspossitions.Add(item.Possition);
            foreach (var item in TankManager.ArmyTanks2)
                tankspossitions.Add(item.Possition);


            Server.SendData(Serializer.Serialize(tankspossitions));
        }

        /// <summary>
        /// تابع موقه ای اطلاعاتی از سرور دریافت میکنه
        /// </summary>
        /// <param name="data"></param>
        public static void GetUpdate(string data)
        {
            if (data.Contains("KILL:"))
            {
                string victomname = data.Replace("KILL:", "");

                if(string.Equals("NULL", victomname))
                {
                    Status = GameStatus.YourTurn;
                    AudioPlayer.PlayMiss();
                    return;
                }

                Tank victom = TankManager.ArmyTanks1.Where(x => string.Equals(x.TankImage.Name, victomname)).First();
                TankManager.Destroy(victom);

                if(TankManager.ArmyTanks1.Count > 0)
                    Status = GameStatus.YourTurn;

            }

            if (string.Equals("START1", data))
            {
                Status = GameStatus.YourTurn;

                PlayerNumber = 1;

                return;
            }

            if (string.Equals("START2", data))
            {
                Status = GameStatus.EnemyTurn;

                PlayerNumber = 2;

                return;
            }
            if (string.Equals("END", data))
            {
                OnGameOver();
                Status = GameStatus.Over;
                return;
            }
            try
            {
                DeterminTankPositions(Serializer.Deserialize<List<Point>>(data));
                return;// اطلاعات از نوع موقیت تانک ها بود که از سرور دریافت شد.

            }catch { }
        }


        static void AnalisisShotOfflineMode(Point shotlocation)
        {
            AudioPlayer.PlayShot();

            Tank TankVictom = TankManager.GetTankAt(shotlocation);

            Shot newshot = new Shot(shotlocation, string.Empty);

            if (TankVictom == null)
            {
                //Shot is null
                PlayGround.MissedFierMarkImage.Invoke(new MethodInvoker(() => { 
                    PlayGround.MissedFierMarkImage.Visible = true;
                    PlayGround.MissedFierMarkImage.Location = Vector.ConvertPositionToWorld(PlayGround.MissedFierMarkImage, shotlocation);
                }));
                return;
            }

            PlayGround.Image.Invoke(new MethodInvoker(() => {
                PlayGround.MissedFierMarkImage.Visible = true;
                newshot.Victom = TankVictom.TankImage.Name;
                TankVictom.TankImage.Visible = true;
                TankManager.Destroy(TankVictom);
            }));

        }
        static void AnalisisShotOnlienMode(Point shotlocation)
        {
            if (!ShotPremition(shotlocation))
                return;

            AudioPlayer.PlayShot();


            Status = GameStatus.EnemyTurn;

            Tank TankVictom = TankManager.GetTankAt(shotlocation);

            Shot newshot = new Shot(shotlocation, string.Empty);

            if (TankVictom == null)
            {
                //Shot is null
                Server.SendData(Serializer.Serialize(newshot));

                PlayGround.MissedFierMarkImage.Visible = true;
                PlayGround.MissedFierMarkImage.Location = Vector.ConvertPositionToWorld(PlayGround.MissedFierMarkImage, shotlocation);
                return;
            }

            PlayGround.MissedFierMarkImage.Visible = false;
            newshot.Victom = TankVictom.TankImage.Name;
            TankVictom.TankImage.Visible = true;
            TankManager.Destroy(TankVictom);

            Server.SendData(Serializer.Serialize(newshot));

            if (TankManager.ArmyTanks2.Count == 0)
            {
                Status = GameStatus.Over;
                OnWin();
                Server.SendData("END" + PlayerNumber);
            }
        }
        static void Botshot(Point shotlocation)
        {
            AnalisisShotOfflineMode(shotlocation);
            Status = GameStatus.YourTurn;
        }

        static bool ShotPremition(Point HitLocation)
        {
            switch (Status)
            {
                case GameStatus.NotStarted:
                    MessageBox.Show("Waitting for player 2 to join.");
                    return false;
                case GameStatus.EnemyTurn:
                    MessageBox.Show("You can not shot. It is not your turn.");
                    return false;
                case GameStatus.Over:
                    MessageBox.Show("The game is over.");
                    return false;

                default:
                    break;
            }

            if (PlayGround.IsInTheGround1(HitLocation))
                return false;

            return true;
        }
        static void DeterminTankPositions(List<Point> tankspos)
        {
            for (int i = 0; i < 5; i++)
            {
                TankManager.ArmyTanks1[i].Possition = PlayGround.MirrorPosition(tankspos[i + 5]);
                TankManager.ArmyTanks2[i].Possition = PlayGround.MirrorPosition(tankspos[i]);
            }
        }
    }
}
