using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using unoProyect.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para VerifyAccount.xaml
    /// </summary>
    public partial class VerifyAccount : Page
    {
        string ValidationCode { get; set; }
        CallDataService callDataService = new CallDataService();
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        public VerifyAccount()
        {
            InitializeComponent();
        }

        public VerifyAccount(string username, string password, string email) : this(){
            Username = username;
            Password = password;
            Email = email;
            SendCode(Email);
        }

        public int AddCredentials()
        {
            int result = callDataService.AddCredentials(Username, Password, Email);
            switch (result)
            {
                case 0:
                    MessageBox.Show(Properties.Resources.informationWrongSignUp, "");
                    break;
                case 1:
                    MessageBox.Show(Properties.Resources.informationSuccesfullSignUp, "");
                    break;
                case 2:
                    MessageBox.Show(Properties.Resources.informationUsernameDuplicate, "");
                    break;
            }
            return result;
        }

        public void SendCode(string email)
        {
            string code = "";
            code = (new Random().Next(100000, 999999)).ToString();
            ValidationCode = code;
            try
            {
                bool result = Utilities.SendMail(email, Properties.Resources.welcome, "Bienvenido a UNOGame. \n El código es: " + code + "\nSi no solicitó " +
                "una cuenta, no es necesario realizar alguna otra acción.\n Saludos, UNOGame");
                if (!result)
                {
                    MessageBox.Show(Properties.Resources.tryAgain, Properties.Resources.sorry);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.instructionSendConfirmCode);
                }
            }
            catch (SmtpException )
            {
                throw new SmtpException();
            }
        }

        private void BtnSendCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendCode(Email);
            }
            catch (SmtpException)
            {
                MessageBox.Show(Properties.Resources.mailServerOutOfService, Properties.Resources.sorry);
            }
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateEmptyField() && ValidateCode())
            {
                if (AddCredentials() == 1)
                {
                    MessageBox.Show("Todo correcto, inicia sesión para continuar");
                    Login login = new Login();
                    this.NavigationService.Navigate(login);
                }
            }
        }

        private bool ValidateEmptyField()
        {
            bool emptyCode = string.IsNullOrEmpty(TbValidationCode.Text);
            if (emptyCode)
            {
                MessageBox.Show(Properties.Resources.notEmptyFields);
            }
            return emptyCode;
        }

        private bool ValidateCode()
        {
            if (ValidationCode != TbValidationCode.Text)
            {
                MessageBox.Show(Properties.Resources.wrongCode);
            }
            return ValidationCode == TbValidationCode.Text;
        }
    }
}
