using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace unoProyect.Security
{
    public class Utilities
    {
        public static string ComputeSHA256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder hashedPassword = new StringBuilder();
                for (int i = 0; i < (bytes.Length); i++)
                {
                    hashedPassword.Append(bytes[i].ToString("x2"));
                }
                return hashedPassword.ToString();
            }
        }

        public static bool ValidateEmail(string email)
        {
            try
            {
                var emailValidated = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool ValidatePassword(string password)
        {
            return (password.Length > 8);
        }
    }
}
