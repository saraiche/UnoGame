using unoProyect.Logic;
using unoProyect.Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {

        private Logic.CallDataService logic = new Logic.CallDataService();
        public Logic.CallChatService CallChatService { get; set; }
        public bool IsHost { get; set; }

        public string Username { get; set; }
        public string InvitationCode { get; set; }
        public Lobby()
        {
            InitializeComponent();

        }
        public Lobby(string username, string invitationCode, bool isHost):this()
        {
            this.Username = username;
            this.InvitationCode = invitationCode;
            CallChatService = new Logic.CallChatService();
            TbCodeGame.Text = this.InvitationCode.ToString();
            LvFriendList.Items.Add(CallChatService.Users);
            IsHost = isHost;
            BtnKickFromTheGame.IsEnabled = false;
            IsPlayer();
        }

        private void IsPlayer()
        {
            if (!IsHost)
            {
                WpPlayWithFriends.Visibility = Visibility.Collapsed;
                BtnStart.Visibility = Visibility.Collapsed;
                BtnKickFromTheGame.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            CallChatService.SendMessage(this.Username,TbMessage.Text,this.InvitationCode);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string[] players = CallChatService.GetPlayersByInvitationCode(InvitationCode);
            if (players.Length < 2)
            {
                MessageBox.Show(Properties.Resources.errorStartGamePlayers, Properties.Resources.error);
            }
            else
            {
                CallChatService.RequestOpenGame(InvitationCode);
                Card center = GameLogic.GetRandomCenter();
                CallChatService.PutCardInCenter(InvitationCode, center);

            }
        }

        

        private void BtnSendByUsername_Click(object sender, RoutedEventArgs e)
        {
            var username = TbUsername.Text;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(username)){
                MessageBox.Show(Properties.Resources.notEmptyFields,
                            Properties.Resources.error);
            }
            else
            {
                DTOCredentials dTOCredentials = logic.SearchUserByUsername(username);
                if (dTOCredentials != null)
                {
                    Utilities.SendMail(dTOCredentials.Email, Properties.Resources.invitationCode, Username + Properties.Resources.instructionSomebodyIsInvitingYou + " " +
                        Properties.Resources.theGameCodeIs + InvitationCode);
                    MessageBox.Show(Properties.Resources.invitationSent);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.usernameNotFound, Properties.Resources.sorry);
                }

            }
        }

        private void BtnSendByEmail_Click(object sender, RoutedEventArgs e)
        {
            var email = TbEmail.Text;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(Properties.Resources.notEmptyFields,
                            Properties.Resources.error);
            }
            else
            {
                if (Utilities.ValidateEmail(email))
                {
                    Utilities.SendMail(email, Properties.Resources.invitationCode, Username + Properties.Resources.instructionSomebodyIsInvitingYou + " " + 
                        Properties.Resources.theGameCodeIs + InvitationCode);
                    MessageBox.Show(Properties.Resources.invitationSent);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.wrongEmail, Properties.Resources.error);
                }
            }
        }

        public void RecieveMessage(string user, string message)
        {
            Console.WriteLine("Desde presntacion" + user,message);
        }

        public void GetUsers(string user)
        {
            LvFriendList.Items.Add(user);
        }

        private void BtnKickFromTheGame_Click(object sender, RoutedEventArgs e)
        {
            CallChatService.DeletePlayer(InvitationCode, LvFriendList.SelectedItem.ToString());
        }

        private void LvFriendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsHost)
            {
                BtnKickFromTheGame.IsEnabled = true;
            }
        }
    }
}
