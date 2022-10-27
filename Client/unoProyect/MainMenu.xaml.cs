﻿using System;
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
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public string Username { get; set; }
        public MainMenu()
        {
            InitializeComponent();
        }
        public MainMenu(string username):this()
        {
            this.Username = username;
        }


        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            Lobby lobby = new Lobby(this.Username);
            this.NavigationService.Navigate(lobby);
        }

        private void BtnFriends_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
