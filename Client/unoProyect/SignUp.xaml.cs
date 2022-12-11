using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using unoProyect.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para CallDataService.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        CallDataService callDataService = new CallDataService();
        public SignUp()
        {
            InitializeComponent();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            var username = tbUser.Text;
            var password = pbPassword.Password.ToString();
            var email = tbEmail.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(email))
            {
                MessageBox.Show(Properties.Resources.notEmptyFields,
                            Properties.Resources.error);
            }
            else
            {
                if(!ExistUsername(username) && ValidateEmailAndPassword(password, email))
                {
                    password = Utilities.ComputeSHA256Hash(password);
                    VerifyAccount verifyAccount = new VerifyAccount(username, password, email);
                    this.NavigationService.Navigate(verifyAccount);
                }
            }
        }

        private bool ExistUsername(string username)
        {
            bool existUsername = callDataService.SearchUser(username);
            if (existUsername)
            {
                MessageBox.Show(Properties.Resources.informationUsernameDuplicate, "");
            }
            return existUsername;

        }
        private bool ValidateEmailAndPassword(string password, string email)
        {
            bool isValid = Utilities.ValidatePassword(password) && Utilities.ValidateEmail(email);
            if (!isValid)
            {
                MessageBox.Show(Properties.Resources.invalidPasswordOrEmail, "");
            }
            return isValid;
        }

    }
}
