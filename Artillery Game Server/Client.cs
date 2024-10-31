using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Game_Server
{
    internal class Client
    {
        public delegate void ClientReceivedHandeler(Client sender, byte[] data);
        public delegate void ClientDisconnetctedHandeler(Client sender);

        public event ClientReceivedHandeler Received;
        public event ClientDisconnetctedHandeler Disconnetcted;

        public IPEndPoint endpoint { get; set; }

        public int PlayerNumber { get; set; }

        Socket sck;

        public Client(Socket accepted)
        {
            sck = accepted;
            endpoint = (IPEndPoint)sck.RemoteEndPoint;
            sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
        }

        public void SendData(string data)
        {
            sck.Send(Encoding.UTF8.GetBytes(data));
        }
        public void Close()
        {
            sck.Close();
            sck.Dispose();
        }

        void callback(IAsyncResult ar)
        {
            try
            {
                sck.EndReceive(ar);
                byte[] buf = new byte[8192];

                int rec = sck.Receive(buf, buf.Length, 0);

                if (rec < buf.Length)
                {
                    Array.Resize(ref buf, rec);
                }
                if (Received != null)
                    Received(this, buf);
                sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);

            }
            catch (Exception)
            {
                Close();
                if (Disconnetcted != null)
                    Disconnetcted(this);


            }
        }
    }
}
