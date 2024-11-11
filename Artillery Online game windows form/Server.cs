using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form
{
    internal class Server
    {
        public delegate void ServerRecive(string data);
        public static event ServerRecive OnDataRecive;
        public static Action OnServerConnect;

        public static int PORT_NUMBER = 912;
        public static string SERVER_IP = "192.168.1.8";
        static IPEndPoint serverIp = new IPEndPoint(IPAddress.Parse(SERVER_IP),PORT_NUMBER); 
        static Socket Clientsocket;

        public static void TryConnect()
        {
            try
            {
                Clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                OnServerConnect += delegate () {Clientsocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);};

                Task.Run(() =>
                {
                    //Task.Delay(5000);

                    do
                    {
                        try
                        {
                            Clientsocket.Connect(serverIp);
                        }
                        catch 
                        {
                            Task.Delay(300);
                        }
                        
                    } while (!Clientsocket.Connected);

                    OnServerConnect();
                });
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"Error connecting to game server!. {e.Message}");
            }
        }
        public static void SendData(string message)
        {
            try
            {
                if (Clientsocket.Send(Encoding.Default.GetBytes(message)) < 0)
                    System.Windows.Forms.MessageBox.Show("Sending data failed.");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"Sending data failed. {e.Message}");
            }
        }

        static void callback(IAsyncResult ar)
        {
            try
            {
                Clientsocket.EndReceive(ar);

                if(OnDataRecive!= null)
                {
                    byte[] buf = new byte[8192];

                    int rec = Clientsocket.Receive(buf, buf.Length, 0);

                    if (rec < buf.Length)
                        Array.Resize(ref buf, rec);

                    OnDataRecive(Encoding.UTF8.GetString(buf));
                }

                Clientsocket.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"Receiving falied. {e.Message}");
            }
        }
    }
}
