using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery_Online_game_windows_form
{
    internal class PanelManager
    {
        public static void ShowGamePanel()
        {
            GameForm.MainForm.GamePanel.Show();
            GameForm.MainForm.OptionsPanel.Hide();
            GameForm.MainForm.MenuPanel.Hide();

        }
        public static void ShowMenuPanel()
        {
            GameForm.MainForm.GamePanel.Hide();
            GameForm.MainForm.OptionsPanel.Hide();
            GameForm.MainForm.MenuPanel.Show();

        }
        public static void ShowOptionsPanel()
        {
            GameForm.MainForm.GamePanel.Hide();
            GameForm.MainForm.OptionsPanel.Show();
            GameForm.MainForm.MenuPanel.Hide();

        }
    }
}
