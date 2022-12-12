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
using unoProyect.Logic;
using unoProyect.Security;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para PlayAsGuest.xaml
    /// </summary>
    public partial class PlayAsGuest : Page
    {
        public Logic.CallChatService CallChatService { get; set; }
        private static string RandomUsername { get; set; }
        private const int SUCCESFUL = 1;
        private const int ERROR = 0;
        private const bool GUEST = false;
        public PlayAsGuest()
        {
            InitializeComponent();
            CallChatService = new Logic.CallChatService();
            TbUsername.IsReadOnly = true;
            RandomUsername = RandomString(8);
            MaterialDesignThemes.Wpf.HintAssist.SetHint(TbUsername, RandomUsername);
           

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            string invitationCode = TbInvitationCode.Text;
            if (Utilities.ValidateInvitationCode(invitationCode))
            {
                int result = CallChatService.Join(RandomUsername, invitationCode);
                if (result == SUCCESFUL)
                {
                    Lobby lobby = new Lobby(RandomUsername, invitationCode, GUEST);
                    CallChatService.GetUsersChat(invitationCode, RandomUsername);
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
