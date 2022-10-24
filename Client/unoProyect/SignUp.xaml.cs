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
using unoProyect.Security;


namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para CallDataService.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        Logic.CallDataService logic = new Logic.CallDataService();
        public SignUp()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
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
                
                if(Utilities.ValidatePassword(password) && Utilities.ValidateEmail(email))
                {
                    password = Utilities.ComputeSHA256Hash(password);
                    
                    logic.AddCredentials(username,password,email);
                        //abrir ventana para ingresar código de email
                        MessageBox.Show("Registro okei", "");
                    
                }
                else
                {
                    MessageBox.Show("Contraseña o email inválidos", "");
                }
            }
        }
    }
}
