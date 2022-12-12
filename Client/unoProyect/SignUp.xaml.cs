using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
        private const int SUCCESFUL = 1;
        private const int NOT_MATCHES = 0;
        public SignUp()
        {
            InitializeComponent();
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            var username = tbUser.Text;
            var password = pbPassword.Password.ToString();
            var email = tbEmail.Text;
            if (Utilities.ValidateField(username) || Utilities.ValidateField(password) || Utilities.ValidateField(email))
            {
                MessageBox.Show(Properties.Resources.notEmptyFields,
                            Properties.Resources.error);
            }
            else
            {
                if(ExistUsername(username) ==  NOT_MATCHES && ValidateEmailAndPassword(password, email))
                {
                    password = Utilities.ComputeSHA256Hash(password);
                    try
                    {
                        VerifyAccount verifyAccount = new VerifyAccount(username, password, email);
                        this.NavigationService.Navigate(verifyAccount);
                    }
                    catch (SmtpException)
                    {
                        MessageBox.Show(Properties.Resources.mailServerOutOfService, Properties.Resources.sorry);
                    }
                }
            }
        }

        private int ExistUsername(string username)
        {
            int existUsername = callDataService.SearchUser(username);
            if (existUsername == SUCCESFUL)
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
