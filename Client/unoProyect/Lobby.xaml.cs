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
        Logic.CallChatService CallChatService { get; set; }
        public string Username { get; set; }
        public int InvitationCode { get; set; }
        public Lobby()
        {
            InitializeComponent();
            
        }
        public Lobby(string username):this()
        {
            this.Username = username;
            CallChatService = new Logic.CallChatService();
            this.InvitationCode = CallChatService.GetInvitationCode();
            TbCodeGame.Text = this.InvitationCode.ToString();
            CallChatService.Join(Username, InvitationCode);
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hola desde send button");
            CallChatService.SendMessage(TbMessage.Text);
            Console.WriteLine(CallChatService.Messages.ToString());
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
