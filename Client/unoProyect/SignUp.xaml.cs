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
                
                if(Utilities.ValidatePassword(password) && Utilities.ValidateEmail(email))
                {
                    
                    int result = logic.AddCredentials(username,password,email);
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
                        //TODO: abrir ventana para ingresar código de email
                    
                }
                else
                {
                    MessageBox.Show("Contraseña o email inválidos", "");
                }
            }
        }
    }
}
