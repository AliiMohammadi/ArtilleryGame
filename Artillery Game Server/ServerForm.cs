using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.Script.Serialization;
using Artillery_Online_game_windows_form;
using System.Threading;
using System.Diagnostics;

namespace Artillery_Game_Server
{
    public partial class ServerForm : Form
    {
        const int Port = 912;
        Listener listener;
        List<Client> clients = new List<Client>();

        JavaScriptSerializer Serializer = new JavaScriptSerializer();

        /// <summary>
        /// اطلاعات موقیت های تانک بازی کن هارو نگه میداره. 
        /// </summary>
        string MatchTankPositionsSerilized;

        Client Player1;
        Client Player2;

        public ServerForm()
        {
            InitializeComponent();

            listener = new Listener(Port);
            listener.SocketAccepted += new Listener.SocketAcceptedHandler(ClientJoinServer);
        }

        private void StartBTN_Click(object sender, EventArgs e)
        {

        }
        private void ClientJoinServer(Socket e)
        {
            if (clients.Count > 2)
                return; //بیشتر از 2 نفر نمیشه پلی داد

            Client newclient = new Client(e);

            newclient.Received += new Client.ClientReceivedHandeler(ClientDataRecive);
            //newclient.Disconnetcted += Client_Disconnetcted;

            if (clients.Count == 0)
                Player1 = newclient;
            if (clients.Count == 1)
                Player2 = newclient;

            clients.Add(newclient);
            Print($"Client: {newclient.endpoint} Connected.");

            if (clients.Count == 2)
                StartMatch();
        }
        private void ServerForm_Load(object sender, EventArgs e)
        {
            RunnServer();
        }

        void RunnServer()
        {
            listener.Start();
            Print("Running...");
        }
        void StartMatch()
        {
            if (string.IsNullOrEmpty(MatchTankPositionsSerilized))
            {
                MessageBox.Show("Error: No initial tank position recived to start game.");
                return;
            }

            SendMessageTo(clients[0], "START1");
            SendMessageTo(clients[1], "START2");

            Thread.Sleep(500);

            SendMessageTo(Player2,MatchTankPositionsSerilized);

            Print($"Match started.");
        }
        void ClientDataRecive(Client sender, byte[] data)
        {
            string newdata = Encoding.ASCII.GetString(data);

            if (string.Equals(newdata, "END1"))
            {
                Print("Player 1 win.");
                SendMessageTo(OpositPlayer(sender),"END");
                return;
            }

            if (string.Equals(newdata, "END2"))
            {
                Print("Player 1 win.");
                SendMessageTo(OpositPlayer(sender), "END");
                return;
            }

            //تشخیص نوع داده:

            try
            {
                if (!newdata.Contains("StrikePoint"))
                {
                    if (!string.IsNullOrEmpty(MatchTankPositionsSerilized))
                        return;

                    List<Point> tankspos = Serializer.Deserialize<List<Point>>(newdata);

                    Print("Repositioning tanks requested.");
                    MatchTankPositionsSerilized = newdata;
                    Player1 = sender;
                    return;
                }
                else
                {
                    Shot newshot = Serializer.Deserialize<Shot>(newdata);

                    if (string.IsNullOrEmpty(newshot.Victom))
                    {
                        Print("Missed shot.");
                        SendMessageTo(OpositPlayer(sender), $"KILL:NULL");
                        return;
                    }

                    string OtherplayerTankName = "Tank" + (int.Parse(newshot.Victom.Replace("Tank", "")) - 5);
                    SendMessageTo(OpositPlayer(sender), $"KILL:{OtherplayerTankName}");
                    Print($"{OtherplayerTankName} destroied.");

                    return;//داده یک حرکت از بازی کن بود
                }
            }
            catch { }

            Print($"Warnning: unknown data detected:{newdata}");

        }

        void SendMessageToAll(string message)
        {
            SendMessageToList(clients, message);
        }
        void SendMessageToList(List<Client> clients, string message)
        {
            foreach (var clietn in clients)
                SendMessageTo(clietn, message);
        }
        void SendMessageTo(Client targetclient, string message)
        {
            targetclient.SendData(message);
        }
        void Print(object message)
        {
            Invoke((MethodInvoker)delegate {
                textbox.AppendText($"{message}\r\n");
            });
        }

        /// <summary>
        /// از دو بازی کن, بازی کن روبه رویی رو به نسبت بازی کن ورودی میگیرد.
        /// </summary>
        /// <param name="theone"></param>
        /// <returns></returns>
        Client OpositPlayer(Client theone)
        {
            if (clients.Count != 2)
                return null;

            return clients[(int)Math.Cos(clients.IndexOf(theone))];
        }
    }
}
