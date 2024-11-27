using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artillery_Online_game_windows_form
{
    public partial class GameForm : Form
    {
        public Options options;
        public static GameForm MainForm;
        public GameForm()
        {
            InitializeComponent();
            MainForm = this;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            PanelManager.ShowMenuPanel();

            PlayGround.Image = Palygroundimage;
            PlayGround.MissedFierMarkImage = DotIMG;

            FlipEnemyTanks();
            AddTanksToTankManager();

            GameManager.OnGameOver += () => { SetResultText("GAME OVER!"); };
            GameManager.OnWin += () => { SetResultText("YOU WIN."); };
        }
        private void Palygroundimage_MouseClick(object sender, MouseEventArgs e)
        {
            //if(TankManager.GetTankAt(e.Location) !=null)
            //MessageBox.Show(TankManager.GetTankAt(e.Location).TankImage.Name);
            GameManager.ShotAt(e.Location);
        }
        private void GameForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void OnlineBTN_Click(object sender, EventArgs e)
        {
            PanelManager.ShowGamePanel();
            
            StartOnlineGame();
        }
        private void OfflineBTN_Click(object sender, EventArgs e)
        {
            PanelManager.ShowGamePanel();
            GameManager.Mode = GameManager.GameMode.Offline;
            GameManager.RepositionTanks();
            SetVisiblityForEnemyTanks(false);
            SetResultText(string.Empty);
            GameManager.Status = GameManager.GameStatus.YourTurn;
            GameManager.CreatAI();
        }
        private void OptionsBTN_Click(object sender, EventArgs e)
        {
            PanelManager.ShowOptionsPanel();
        }
        private void ExitBTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void StartOnlineGame()
        {
            try
            {
                ReconfigGame();
                ConnectToServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initilizing: " + ex.Message);
            }
        }
        void FlipEnemyTanks()
        {
            Tank6.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            Tank7.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            Tank8.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            Tank9.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            Tank10.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }
        void ReconfigGame()
        {
            try
            {
                Server.SERVER_IP = Iptextbox.Text;
                //Server.PORT_NUMBER = int.Parse(options.PrtTextBox.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        void ConnectToServer()
        {

            Server.OnServerConnect += () =>
            {
                GameManager.RepositionTanks();
                SetVisiblityForEnemyTanks(false);
                SetResultText(string.Empty);
                GameManager.SendTankStatusToServer();
            };

            Server.OnDataRecive += GameManager.GetUpdate;

            SetResultText("Connecting..");

            Server.TryConnect();
        }
        void AddTanksToTankManager()
        {
            TankManager.ArmyTanks1.Add(new Tank(1,Tank1));
            TankManager.ArmyTanks1.Add(new Tank(1,Tank2));
            TankManager.ArmyTanks1.Add(new Tank(1,Tank3));
            TankManager.ArmyTanks1.Add(new Tank(1,Tank4));
            TankManager.ArmyTanks1.Add(new Tank(1,Tank5));

            TankManager.ArmyTanks2.Add(new Tank(2,Tank6));
            TankManager.ArmyTanks2.Add(new Tank(2,Tank7));
            TankManager.ArmyTanks2.Add(new Tank(2,Tank8));
            TankManager.ArmyTanks2.Add(new Tank(2,Tank9));
            TankManager.ArmyTanks2.Add(new Tank(2,Tank10));
        }
        void UpdateTurnTip()
        {
            string newname = string.Empty;
            switch (GameManager.Status)
            {
                case GameManager.GameStatus.NotStarted:
                    newname = "Not Started";
                    break;
                case GameManager.GameStatus.YourTurn:
                    newname = "Your Turn";

                    break;
                case GameManager.GameStatus.EnemyTurn:
                    newname = "Enemy Turn";

                    break;
                case GameManager.GameStatus.Over:
                    newname = "Game Over";
                    break;
                default:
                    break;
            }

            SetFormName(Text + ": "+newname);
        }
        void SetFormName(string name)
        {
            Invoke(new MethodInvoker(() =>
            {
                Name = name;
            }));

        }

        void SetVisiblityForEnemyTanks(bool vis)
        {
            Invoke(new MethodInvoker(() =>
            {
                Tank6.Visible = vis;
                Tank7.Visible = vis;
                Tank8.Visible = vis;
                Tank9.Visible = vis;
                Tank10.Visible = vis;
            }));
        }
        void SetResultText(string message)
        {
            Resulttext.Invoke(new MethodInvoker(() =>
            {
                Resulttext.Enabled = true;
                Resulttext.Visible = true;
                Resulttext.Text = message;
            }));
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            PanelManager.ShowMenuPanel();
        }
    }
}
