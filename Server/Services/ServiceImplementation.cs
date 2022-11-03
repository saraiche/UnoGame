﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UnoEntitys;
using Utilities;

namespace Services
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public partial class ServiceImplementation : IChatService
        {
        Dictionary<IChatClient, string> _users = new Dictionary<IChatClient, string>();
        public int InvitationCode { get; set; }

        public ServiceImplementation()
        {
            InvitationCode = new Random().Next(100000, 999999);
        }

        public int Join(string username, int code)
        {
            int result = -1;
            if (code == InvitationCode)
            {
                var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
                _users[connection] = username;
                result = 1;
            }
            return result;

        }

        public void SendMessage(string message)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
            string user;
            if (!_users.TryGetValue(connection, out user))
                return;
            foreach (var other in _users.Keys)
            {
                if (other == connection)
                    continue;
                other.RecieveMessage(user, message);
            }
        }

        public int GetInvitationCode()
        {
            return InvitationCode;
        }

    }
        public partial class ServiceImplementation : IDataService
        {
            /// <summary>
            /// Agrega las credenciales de un jugador a la base de datos
            /// </summary>
            /// <param name="credentials"></param>
            /// <returns> 1 si se guardaron correctamente los cambios, 
            /// 2 si ya existía el username registrado, 
            /// 0 si ocurrió un error al guardar en la base de datos
            /// </returns>
            /// <exception cref="DbUpdateException"></exception>
            public int AddCredentials(DTOCredentials credentials)
            {
                int result = 0;
                Credentials entityCredential = this.DtoCredentialsToEntity(credentials);

                try
                {
                    using (unoDbModelContainer context = new unoDbModelContainer())
                    {
                        Player findPlayer = context.PlayerSet1.Where(x => x.Credentials.username == credentials.Username).FirstOrDefault();
                        if (findPlayer == null)
                        {
                            Images images = new Images { Id = 1 };
                            context.ImagesSet1.Attach(images);
                            Player player = new Player { wins = 0, losts = 0, Images = images };
                            entityCredential.Player = player;
                            context.CredentialsSet1.Add(entityCredential);
                            context.SaveChanges();
                            result = 1;
                        }
                        else
                        {
                            result = 2;
                        }

                    }
                }
                catch (DbUpdateException exception)
                {
                    throw new DbUpdateException(exception.Message);
                }
                return result;
            }

            public bool AddImages()
            {
                throw new NotImplementedException();
            }


            public bool IsUser(DTOCredentials credentials)
            {
                bool flag = false;
                credentials.Password = Security.ComputeSHA256Hash(credentials.Password);
                Credentials entityCredential = this.DtoCredentialsToEntity(credentials);
                try
                {
                    using (unoDbModelContainer dataBase = new unoDbModelContainer())
                    {
                        DTOPlayer dTOPlayer = new DTOPlayer();
                        //buscar credenciales
                        Credentials findCredentials = dataBase.CredentialsSet1.Where(x => x.username == credentials.Username && x.password == credentials.Password).FirstOrDefault();
                        if (findCredentials != null)
                        {
                            flag = true;
                        }
                        return flag;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }

            /// <summary>
            /// Convierte un data transfer object a entity para conectarse con la capa de DataAccess
            /// </summary>
            /// <param name="dTOCredentials"></param>
            /// <returns></returns>
            public Credentials DtoCredentialsToEntity(DTOCredentials dTOCredentials)
            {
                Credentials result = new Credentials();
                result.Id = 0;
                result.username = dTOCredentials.Username;
                result.email = dTOCredentials.Email;
                result.password = dTOCredentials.Password;
                return result;
            }

        public bool SearchUser(DTOCredentials credentials)
        {
            bool flag = false;
            Credentials entityCredential = this.DtoCredentialsToEntity(credentials);
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    DTOPlayer dTOPlayer = new DTOPlayer();
                    Credentials findCredentials = dataBase.CredentialsSet1.Where(x => x.username == credentials.Username).FirstOrDefault();
                    if (findCredentials != null)
                    {
                        flag = true;
                    }
                    return flag;
                }
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
        }

        public bool SendMail(string to, string emailSubject, string message)
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
}
