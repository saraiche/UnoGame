﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UnoEntitys;
using Utilities;

namespace Services
{
        /// <summary>
        /// El contexto es Single lo que quiere decir que todos los usuarios conectados al chat van a acceder a
        /// las mismas variables de la clase ChatEngine
        /// </summary>
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public partial class ServiceImplementation : IChatService
        {
            private ChatEngine chatEngine = new ChatEngine();

            /// <summary>
            /// Funcion que conecta un usuario al chat
            /// </summary>
            /// <param name="Username"></param>
            /// <returns>ChatUser</returns>
            /// <exception cref="NotImplementedException"></exception>
            public ChatUser ClientConnect(string username)
            {
                return chatEngine.AddNewChatUser(new ChatUser() { UserName = username });
            }

            public List<ChatUser> GetChatUsers()
            {
                return chatEngine.ConnecteUsers;
            }

            public void RemoveUser(ChatUser user)
            {
                throw new NotImplementedException();
            }

            public void SendNewMessage(ChatMessage chatMessage)
            {
                 chatEngine.AddNewMessage(chatMessage);
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


        
        }
}