using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public Logic.CallChatService CallChatService { get; set; }

        public string Username { get; set; }
        public string InvitationCode { get; set; }

        public Game()
        {

            InitializeComponent();
        }

        public Game(string username, string invitationCode):this()
        {
            lblPlayer1.Content = username;
            this.InvitationCode = invitationCode;
            this.Username = username;
            CallChatService = new Logic.CallChatService();
            BtnPlay.IsEnabled = true;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Game.xaml.cs Invitation code "+ InvitationCode + Username);
            CallChatService.NextTurn(InvitationCode, Username);
        }
    }
}
