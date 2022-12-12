using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            return string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(content);
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
                client.Credentials = new NetworkCredential(from, "tecnologiasConstruccion1234");
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
    public class NotEmptyValidationRule : ValidationRule
    {
        public string Message { get; set; }

        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }
}
