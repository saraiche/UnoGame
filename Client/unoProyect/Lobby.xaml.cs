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
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {
<<<<<<< HEAD
        private Logic.CallDataService logic = new Logic.CallDataService();
=======
        public Logic.CallChatService CallChatService { get; set; }
>>>>>>> 686a88bdba32d126740f403df59d1810a05fdb9f
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                if (logic.SearchUser(username))
                {
                    MessageBox.Show("El username está okei");
                    ///TODO: enviar invitación por correo
                }
                else
                {
                    MessageBox.Show("El username no existe");
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
                    ///TODO: enviar invitación por correo
                    MessageBox.Show("Email okei");
                }
                else
                {
                    MessageBox.Show(Properties.Resources.wrongEmail, Properties.Resources.error);
                }
            }
        }
    }
}
