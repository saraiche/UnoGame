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

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para PlayAsGuest.xaml
    /// </summary>
    public partial class PlayAsGuest : Page
    {
        public Logic.CallChatService CallChatService { get; set; }
        private static string RandomUsername { get; set; }
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
            if (CallChatService.Join(RandomUsername, TbInvitationCode.Text))
            {
                Lobby lobby = new Lobby(RandomUsername, TbInvitationCode.Text, false);
                CallChatService.GetUsersChat(TbInvitationCode.Text, RandomUsername);
                CallChatService.LobbyView = lobby;
                this.NavigationService.Navigate(lobby);

            }
            else
            {
                MessageBox.Show(Properties.Resources.error);
            }
        }
    }
}
