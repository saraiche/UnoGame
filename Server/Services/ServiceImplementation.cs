using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
                Console.WriteLine(username + " se unió a la sala " + code);
            }
            return flag;

        }
        public void GetUsersChat(string code, string username)
        {
         
            foreach (var user in Rooms[code])
            {
                user.Connection.GetUsers(username);
            }
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
            Console.WriteLine(username + " creó la sala "+ invitationCode);
            return invitationCode;
        }
        public bool DeletePlayer(string invitationCode, string username)
        {
            bool flag = false;
            int index = -1;
            foreach (var user in Rooms[invitationCode])
            {
                if (user.UserName == username)
                {
                    index = Rooms[invitationCode].IndexOf(user);
                }
            }
            if (index != -1)
            {
                Rooms[invitationCode].RemoveAt(index);
                flag = true;
            }
            return flag;
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

        public List<string> GetPlayersByInvitationCode(string invitationCode)
        {
            List<string> users = new List<string>();
            foreach (var user in Rooms[invitationCode])
            {
                users.Add(user.UserName);
            }
            return users;
        }

        public ServiceImplementation()
        {
            Rooms = new Dictionary<string, List<DTOUserChat>>();
        }

        public void PutCardInCenter(string invitationCode, Card card)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    con.ReceiveCenter(card);
                }
            }
        }

        public void RequestOpenGame(string invitationCode)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                List<string> users = GetUserListFromDtoList(Rooms[invitationCode]);
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    con.OpenGame(other.UserName, users);
                }
            }
        }

        private List<string> GetUserListFromDtoList(List<DTOUserChat> dtoUserChats)
        {
            List<string> users = new List<string>();
            foreach (var user in dtoUserChats)
            {
                users.Add(user.UserName);
            }
            return users;
        }

        public void DealCard(string username, Card card, string invitationCode)
        {
            IChatClient con;
            foreach(var user in Rooms[invitationCode])
            {
                if (user.UserName == username)
                {
                    con = user.Connection;
                    con.ReceiveCard(card);
                }
            }
        }
        public void NextTurn(string invitationCode, string username)
        {
            List<DTOUserChat> users = Rooms[invitationCode];
            foreach (var user in users)
            {
                if (user.UserName != username)
                {
                    user.Connection.itsMyTurn(false);
                }
                else
                {
                    user.Connection.itsMyTurn(true);
                }
            }
        }

        public void RequestChangeDirection(string invitationCode)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    con.ChangeDirection();
                }
            }
        }

        public void SendTurnInformation(string invitationCode, string color, string actualTurn)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    con.ReceiveTurnInformation(color, actualTurn);
                }
            }
        }

        public void SendWinner(string invitationCode, string username)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    con.ReceiveWinner(username);
                }
            }
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
        public bool AddFriend(string playerName, string friendName)
        {
            bool flag = false;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    DTOPlayer dTOPlayer = new DTOPlayer();
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    Credentials findCredentialsFriend = dataBase.CredentialsSet1.Where(x => x.username == friendName).FirstOrDefault();
                    if (findCredentialsPlayer != null || findCredentialsFriend != null)
                    {
                        Player playerDb = findCredentialsPlayer.Player;
                        Player friendDb = findCredentialsFriend.Player;
                        dataBase.PlayerSet1.Attach(friendDb);
                        playerDb.Friends.Add(friendDb);
                        dataBase.PlayerSet1.Attach(playerDb);
                        dataBase.SaveChanges();
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
        public List<string> GetFriends(string playerName)
        {
            List<string> friends = new List<string>();
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    DTOPlayer dTOPlayer = new DTOPlayer();
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    if (findCredentialsPlayer != null)
                    {
                        Player playerDb = findCredentialsPlayer.Player;
                        List<Player> friendsDb = new List<Player>();
                        friendsDb = playerDb.Friends.ToList();
                        foreach (Player friend in friendsDb)
                        {
                            friends.Add(friend.Credentials.username);
                        }
                    }
                }
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
            return friends;
        }
        public DTOCredentials SearchUserByUsername(string username)
        {
            DTOCredentials userFound = new DTOCredentials();
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    DTOPlayer dTOPlayer = new DTOPlayer();
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == username).FirstOrDefault();
                    if (findCredentialsPlayer != null)
                    {
                        Player playerDb = findCredentialsPlayer.Player;
                        userFound.Username = playerDb.Credentials.username;
                        userFound.Email = playerDb.Credentials.email;
                    }
                }
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
            return userFound;
        }
        public bool ModifyPassword(string playerName, string password)
        {
            bool result = false;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    DTOPlayer dTOPlayer = new DTOPlayer();
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    if (findCredentialsPlayer != null)
                    {
                        //findCredentialsPlayer.password = password;
                        Player playerDb = findCredentialsPlayer.Player;
                        playerDb.Credentials.password = password;
                        dataBase.PlayerSet1.Attach(playerDb);
                        dataBase.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (DbUpdateException exception)
            {
                throw new DbUpdateException(exception.Message);
            }
            return result;
        }
    }

}
