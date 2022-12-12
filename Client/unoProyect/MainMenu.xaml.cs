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
using unoProyect.Security;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public Logic.CallChatService CallChatService { get; set; }

        public string Username { get; set; }
        private const int SUCCESFUL = 1;
        private const int ERROR = 0;
        private const bool GUEST = false;
        private const bool HOST = true;
        public MainMenu()
        {
            InitializeComponent();
        }
        public MainMenu(string username):this()
        {
            this.Username = username;
            CallChatService = new Logic.CallChatService();
            LblUsername.Content = username;

        }


        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {

            string invitationCode = CallChatService.NewRoom(this.Username);
            Lobby lobby = new Lobby(this.Username, invitationCode, HOST);
            CallChatService.LobbyView = lobby;
            CallChatService.GetUsersChat(invitationCode,Username);
            this.NavigationService.Navigate(lobby);
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {
            UserProfile userProfile = new UserProfile(Username);
            this.NavigationService.Navigate(userProfile);

        }

        private void BtnEnterWithCode_Click(object sender, RoutedEventArgs e)
        {
            string invitationCode = TbInvitationCode.Text;
            if (Utilities.ValidateInvitationCode(invitationCode))
            {
                int result = CallChatService.Join(Username, invitationCode);
                if (result == SUCCESFUL)
                {
                    Lobby lobby = new Lobby(this.Username, invitationCode, GUEST);
                    CallChatService.GetUsersChat(invitationCode, Username);
                    CallChatService.LobbyView = lobby;
                    this.NavigationService.Navigate(lobby);

                }
                else if (result == ERROR)
                {
                    MessageBox.Show(Properties.Resources.notMachesInvitationCode);
                }
            }
            else
            {
                MessageBox.Show(Properties.Resources.wrongInvitationCode);
            }
        }
    }
}
