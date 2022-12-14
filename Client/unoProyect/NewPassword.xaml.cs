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
using unoProyect.Proxy;
using unoProyect.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para NewPassword.xaml
    /// </summary>
    public partial class NewPassword : Page
    {
        private string ValidationCode { get; set; }
        private CallDataService callDataService;
        private DTOCredentials credentials;
        private const int SUCCESFUL = 1;
        private const int ERROR = 0;
        public NewPassword()
        {
            InitializeComponent();
        }
        public NewPassword(string validationCode, DTOCredentials credentials) : this()
        {
            ValidationCode = validationCode;
            callDataService = new CallDataService();
            this.credentials = credentials;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LbWrongPasswords.Visibility = Visibility.Hidden;
            if (!ValidateEmptyFields() && ValidateCode() && ValidatePasswords())
            {
                int result = callDataService.ModifyPassword(credentials.Username, Utilities.ComputeSHA256Hash(PbRepeatPassword.Password));
                if (result == SUCCESFUL)
                {
                    MessageBox.Show("Todo correcto, inicia sesión para continuar");
                    Login login = new Login();
                    this.NavigationService.Navigate(login);
                }
                else if (result == ERROR)
                {
                    MessageBox.Show(Properties.Resources.error);
                }
            }
        }

        private bool ValidateEmptyFields()
        {
            bool emptyCode = string.IsNullOrWhiteSpace(TbValidationCode.Text);
            bool emptyNewPassword = string.IsNullOrWhiteSpace(PbNewPassword.Password.ToString());
            bool emptyRepeatPassword = string.IsNullOrWhiteSpace(PbRepeatPassword.Password.ToString());

            if (emptyCode || emptyNewPassword || emptyRepeatPassword)
            {
                MessageBox.Show(Properties.Resources.notEmptyFields);
            }
            return emptyCode || emptyNewPassword || emptyRepeatPassword;
        }

        private bool ValidateCode()
        {
            if (ValidationCode != TbValidationCode.Text)
            {
                MessageBox.Show(Properties.Resources.wrongCode);
            }
            return ValidationCode == TbValidationCode.Text;
        }

        private bool ValidatePasswords()
        {
            bool result = false;
            if (PbNewPassword.Password != PbRepeatPassword.Password)
            {
                LbWrongPasswords.Text = Properties.Resources.passwordsMayBeTheSame;
                LbWrongPasswords.Visibility = Visibility.Visible;
            }
            else
            {
                result = Utilities.ValidatePassword(PbNewPassword.Password);
                if (!result)
                {
                    LbWrongPasswords.Text = Properties.Resources.passwordPolicy;
                    LbWrongPasswords.Visibility = Visibility.Visible;
                }
            }
            return result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                SendCode(credentials.Email);
            }
            catch (SmtpException)
            {
                MessageBox.Show(Properties.Resources.mailServerOutOfService, Properties.Resources.sorry);
            }
        }

        public void SendCode(string email)
        {
            string code = "";
            code = (new Random().Next(100000, 999999)).ToString();
            ValidationCode = code;
            try
            {
                bool result = Utilities.SendMail(email, Properties.Resources.changePassword, "¡Atención!\n Está recibiendo este correo electrónico " +
                "porque recibimos una solicitud de restablecimiento de contraseña para su cuenta de UNOGame. \n El código es: " + code + "\nSi no solicitó " +
                "un restablecimiento de contraseña, no es necesario realizar ninguna otra acción.\n Saludos, UNOGame");
                if (!result)
                {
                    MessageBox.Show(Properties.Resources.tryAgain, Properties.Resources.sorry);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.instructionChangePasswordCode);
                }
            }
            catch (SmtpException)
            {
                throw new SmtpException();
            }
        }
    }
}
