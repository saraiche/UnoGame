using unoProyect.Logic;
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
using System.Net;
using System.Configuration;
using System.Reflection;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        Logic.CallDataService logic = new Logic.CallDataService();
        private readonly Configuration appConfiguration;
        public Login()
        {
            InitializeComponent();
            appConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = tbUser.Text;
            var password = pbPassword.Password.ToString();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(Properties.Resources.notEmptyFields,
                            Properties.Resources.error);
            }
            else
            {
                if (logic.IsUser(username, password))
                {
                    MainMenu mainMenu = new MainMenu(username);
                    this.NavigationService.Navigate(mainMenu);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.wrongCredentials);
                }


            }
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            this.NavigationService.Navigate(signUp);

        }

        private void btnForgotMyPassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            this.NavigationService.Navigate(changePassword);
        }

        private void btnAsGuest_Click(object sender, RoutedEventArgs e)
        {
            GameLogic.GetCards();

        }

        private void BtnEnglish_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("en");
        }

        private void BtnSpanish_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("es");
            
        }

        public void ChangeLanguage(string languaje)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(languaje);
            appConfiguration.Save();
            ConfigurationManager.RefreshSection("appSettings");
            Login login = new Login();
            this.NavigationService.Navigate(login);
        }
    }
}
