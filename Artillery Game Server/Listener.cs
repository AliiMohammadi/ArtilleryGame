using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Game_Server
{
    internal class Listener
    {
        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler SocketAccepted;

        public int port { get; set; }
        public bool Listening { get; set; }

        Socket Serversocket;

        public Listener(int port)
        {
            this.port = port;
            Serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            if (Listening)
                return;

            try
            {
                Serversocket.Bind(new IPEndPoint(0, port));
                Serversocket.Listen(0);

                Serversocket.BeginAccept(callback, null);
            }
            catch (Exception e )
            {
                System.Windows.Forms.MessageBox.Show($"Error running server.{e.Message}");
            }

        }

        void callback(IAsyncResult ar)
        {
            try
            {
                Socket newclient = this.Serversocket.EndAccept(ar);

                if (SocketAccepted != null)
                    SocketAccepted(newclient);

                Serversocket.BeginAccept(callback, null);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show($"Error accepting new client.{e.Message}");

            }
        }
    }
}
