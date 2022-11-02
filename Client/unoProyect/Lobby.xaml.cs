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
    }
}
