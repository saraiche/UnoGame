using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace unoProyect.Security
{
    public static class Utilities
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
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// Politicas de seguridad
        /// Contiene aunque sea un numero
        /// Una letra upper camel case
        /// Una letra lower camel case
        /// Un digito
        /// Un caracter especial
        /// Longitud de al menos 8 caracteres
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            Match match = regex.Match(password);
            return match.Success;
        }

        public static bool ValidateInvitationCode(string invitationCode)
        {
            int code = 0;
            bool isNumber = int.TryParse(invitationCode, out code);
            bool result = false;
            if (isNumber && code >= 100000 && code <= 999999)
            {
                result = true;
            }
            return result;
        }

        public static bool ValidateField(string content)
        {
            return string.IsNullOrWhiteSpace(content);
        }
        public static bool SendMail(string to, string emailSubject, string message)
        {
            bool status = false;
            string from = "uno.game@hotmail.com";
            string displayName = "Uno Game";
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(from, displayName);
                mailMessage.To.Add(to);

                mailMessage.Subject = emailSubject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.Credentials = new NetworkCredential(from, ConfigurationManager.AppSettings["Password"]);
                client.EnableSsl = true;

                client.Send(mailMessage);
                status = true;
            }
            catch (SmtpException ex)
            {
                throw new SmtpException(ex.Message);
            }
            return status;
        }
    }

}
