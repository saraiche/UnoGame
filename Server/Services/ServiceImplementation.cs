using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using UnoEntitys;
using Utilities;

namespace Services
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public partial class ServiceImplementation : IChatService
        {
        Dictionary<string, List<DTOUserChat>> Rooms { get; set; }
        

        public bool Join(string username, string code)
        {
            bool flag = false;
            DTOUserChat newUser = new DTOUserChat();
            List<DTOUserChat> users = new List<DTOUserChat>();

            if (Rooms.Keys.Contains(code))
            {
                //recuperar la lista de los juagadores conectados
                if (Rooms.TryGetValue(code, out List<DTOUserChat> usersConnect))
                {
                    users = usersConnect;
                }
                //crear el nuevo usuario
                var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
                newUser.UserName = username;
                newUser.Connection = connection;
                users.Add(newUser);

                //modificar el diccionario
                Rooms[code] = users;
                
                flag = true;
            }
            return flag;

        }

        public void SendMessage(string username, string message, string invitationCode)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    con.RecieveMessage(username, message);
                }
            }       
        }
       
        public string NewRoom(string username)
        {          
            string invitationCode = new Random().Next(100000, 999999).ToString();
            if (Rooms.Keys.Contains(invitationCode))
            {
                do
                    invitationCode = new Random().Next(100000, 999999).ToString();
                while (!Rooms.Keys.Contains(invitationCode));
            }

            var connection = OperationContext.Current.GetCallbackChannel<IChatClient>();
            List<DTOUserChat> dTOUserChats = new List<DTOUserChat>();
            DTOUserChat dTOUser = new DTOUserChat();
            dTOUser.Connection = connection;
            dTOUser.UserName = username;

            dTOUserChats.Add(dTOUser);
            Rooms.Add(invitationCode, dTOUserChats);
                     
            return invitationCode;
        }

        public void GetUsersChat(string code)
        {
            IChatClient con;
            foreach (var user in Rooms[code])
            {
                con = user.Connection;
                con.GetUsers(user.UserName);
            }
        }

        public ServiceImplementation()
        {
            Rooms = new Dictionary<string, List<DTOUserChat>>();
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
