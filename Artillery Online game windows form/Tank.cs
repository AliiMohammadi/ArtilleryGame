using OneeChanRemake.Operation_System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Artillery_Online_game_windows_form
{
    internal class Tank 
    {
        public PictureBox TankImage;
        public byte ArmyNumber;
        public uint TankCollitionRadias;
        /// <summary>
        /// موقعیت دقیقا وسط عکس نه گوشه بالا سمت چپ!
        /// </summary>
        public Point Possition
        {
            get
            {
                return Vector.CenterOf(TankImage);
            }
            set
            {
                TankImage.Invoke(new MethodInvoker(() =>
                {
                    TankImage.Location = Vector.ConvertPositionToWorld(TankImage,value);
                }));
            }
        }

        public Tank(byte ArmyNumber, PictureBox pbox)
        {
            TankImage = pbox;
            this.ArmyNumber = ArmyNumber;
            TankCollitionRadias = (uint)Math.Sqrt(Math.Pow(TankImage.Width / 2,2) + Math.Pow(TankImage.Height / 2, 2));
            TankCollitionRadias = TankCollitionRadias - (uint)(TankCollitionRadias * 0.5f);
        }

        public void Destroy()
        {
            TankImage.Image = Properties.Resources.dead_panzzer;
            AudioPlayer.PlayExplotion();
        }
    }
}
