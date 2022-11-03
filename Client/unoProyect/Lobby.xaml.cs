using Logic;
using Logic.Proxy;
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

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page, IChatServiceCallback
    {
        public Logic.CallChatService CallChatService { get; set; }
        public string Username { get; set; }
        public string InvitationCode { get; set; }
        

        public ObservableCollection<string> Messages { get; set; }
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
            
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            CallChatService.SendMessage(this.Username,TbMessage.Text,this.InvitationCode);
            //Console.WriteLine(CallChatService.Messages.ToString());
            string mensaje = CallChatService.Message;
            Console.WriteLine(mensaje+"DEsde click button");
            LvChat.Items.Add(mensaje);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {

        }

        public void RecieveMessage(string user, string message)
        {
            Console.WriteLine("Desde presntacion" + user,message);
        }

        public void GetUsers(string user)
        {
            LvFriendList.Items.Add(user);
        }
    }
}
