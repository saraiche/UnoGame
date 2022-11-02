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
        private Logic.CallDataService logic = new Logic.CallDataService();
        public string Username { get; set; }
        public Lobby()
        {
            InitializeComponent();
        }
        public Lobby(string username):this()
        {
            this.Username = username;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
