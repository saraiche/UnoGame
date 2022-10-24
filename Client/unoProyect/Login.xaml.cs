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
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //var username = tbUser.Text;
            //var password = pbPassword.Password.ToString();
            //if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            //{
            //    MessageBox.Show(Properties.Resources.notEmptyFields,
            //                Properties.Resources.error);
            //}
            //else
            //{
            //    password = Utilities.ComputeSHA256Hash(password);
            //    var result = logic.ItsAUser(username, password);
            //    if (!logic.ItsAUser(username, password))
            //    {
            //        MessageBox.Show(Properties.Resources.wrongCredentials, 
            //            Properties.Resources.error);    
            //    }
            //    else
            //    {
            //        MessageBox.Show(Properties.Resources.welcome + " " + username,
            //            "");
            //    }
            //}
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            this.NavigationService.Navigate(signUp);

        }
    }
}
