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
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {
        public Logic.CallChatService CallChatService { get; set; }
        public string Username { get; set; }
        public string InvitationCode { get; set; }
        public Lobby()
        {
            InitializeComponent();
            
        }
        public Lobby(string username, string invitationCode):this()
        {
            this.Username = username;
            this.InvitationCode = invitationCode;
            CallChatService = new Logic.CallChatService();
            TbCodeGame.Text = this.InvitationCode.ToString();
            LvFriendList.Items.Add(CallChatService.Users);
            
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hola desde send button");
            //CallChatService.SendMessage(TbMessage.Text);
            //Console.WriteLine(CallChatService.Messages.ToString());
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
