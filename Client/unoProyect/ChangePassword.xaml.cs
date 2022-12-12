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
using unoProyect.Logic;
using unoProyect.Proxy;
using unoProyect.Security;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        CallDataService callDataService;
        DTOCredentials User { get; set; }
        public ChangePassword()
        {
            InitializeComponent();
            callDataService = new CallDataService();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LblEmptyFields.Visibility = Visibility.Hidden;
            string username = TbUsername.Text;
            string email = TbEmail.Text;
            if (!ValidateEmptyFields())
            {
                if (Utilities.ValidateEmail(email))
                {
                    User = callDataService.SearchUserByUsername(username);
                    if (User != null && User.Email == email)
                    {
                        SendCode(email);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.notMatches);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.wrongEmail);
                }
            }
        }

        private bool ValidateEmptyFields()
        {
            bool emptyUsername = string.IsNullOrEmpty(TbUsername.Text);
            bool emptyEmail = string.IsNullOrEmpty(TbEmail.Text);
            if (emptyUsername || emptyEmail)
            {
                LblEmptyFields.Visibility = Visibility.Visible;
            }
            return emptyUsername || emptyEmail;
        }
        public void SendCode(string email)
        {
            string code = new Random().Next(100000, 999999).ToString();
            bool result = Utilities.SendMail(email, Properties.Resources.changePassword, "¡Atención!\n Está recibiendo este correo electrónico " +
                "porque recibimos una solicitud de restablecimiento de contraseña para su cuenta de UNOGame. \n El código es: " + code + "\nSi no solicitó " +
                "un restablecimiento de contraseña, no es necesario realizar ninguna otra acción.\n Saludos, UNOGame");
            if (result)
            {
                NewPassword newPassword = new NewPassword(code, User);
                this.NavigationService.Navigate(newPassword);
            }
            else
            {
                MessageBox.Show(Properties.Resources.tryAgain, Properties.Resources.sorry);
            }
        }
    }
}
