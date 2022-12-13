using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
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
        public Dictionary<string, List<DTOUserChat>> Rooms { get; set; }
        public ServiceImplementation()
        {
            Rooms = new Dictionary<string, List<DTOUserChat>>();
        }

        public bool Join(string username, string code)
        {
            bool flag = false;
            DTOUserChat newUser = new DTOUserChat();
            List<DTOUserChat> users = new List<DTOUserChat>();

            if (Rooms.Keys.Contains(code) && Rooms[code].Where(x => x.UserName == username).FirstOrDefault() == null)
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
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            foreach (var user in Rooms[code])
            {
                try
                {
                    user.Connection.GetUsers(username);

                }
                catch (System.ServiceModel.CommunicationException)
                {
                    playersToDelete.Add(user);
                }
                foreach (var player in playersToDelete)
                {
                    DeletePlayer(code, player.UserName);
                }
            }
        }

        public void SendMessage(string username, string message, string invitationCode)
        {
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();

            if (Rooms.Keys.Contains(invitationCode) && Rooms[invitationCode].Where(x => x.UserName == username).FirstOrDefault() != null)
            {

                foreach (var other in Rooms[invitationCode])
                {
                    try
                    {
                        other.Connection.RecieveMessage(username, message);

                    }
                    catch (System.ServiceModel.CommunicationException )
                    {
                        playersToDelete.Add(other);
                    }
                }
            }
            foreach(var player in playersToDelete)
            {
                DeletePlayer(invitationCode, player.UserName);
            }
        }
        public void ValidateConnection(string invitationCode)
        {
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            foreach (var other in Rooms[invitationCode])
            {
                try
                {
                    other.Connection.RecieveMessage("Dios", "Memento mori");

                }
                catch (System.ServiceModel.CommunicationException)
                {
                    playersToDelete.Add(other);
                }
            }
            foreach (var player in playersToDelete)
            {
                DeletePlayer(invitationCode, player.UserName);
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
            Console.WriteLine(username + " creó la sala " + invitationCode);
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
                Console.WriteLine(username + "dejó la partida " + invitationCode);
            }
            return flag;
        }



        public List<string> GetPlayersByInvitationCode(string invitationCode)
        {
            ValidateConnection(invitationCode);
            List<string> users = new List<string>();
            foreach (var user in Rooms[invitationCode])
            {
                users.Add(user.UserName);
            }
            return users;
        }

        

        public void PutCardInCenter(string invitationCode, Card card)
        {
            IChatClient con;
            if (Rooms.Keys.Contains(invitationCode))
            {
                bool playerLeftTheGame = false;
                List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    try
                    {
                        con.ReceiveCenter(card);
                    }
                    catch(CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(other);
                    }
                }
                if (playerLeftTheGame)
                {
                    UpdatePlayers(invitationCode, playersToDelete);
                }
            }
        }
        
        private void UpdatePlayers(string invitationCode, List<DTOUserChat> pastList)
        {
            foreach(var player in pastList)
            {
                DeletePlayer(invitationCode, player.UserName);
                SendDeletePlayerFromGame(invitationCode, player.UserName);
            }
        }

        private void SendDeletePlayerFromGame(string invitationCode, string username)
        {
            foreach(var player in Rooms[invitationCode])
            {
                player.Connection.DeletePlayerFromGame(username, GetUserListFromDtoList(Rooms[invitationCode]));
            }
        }

        public void RequestOpenGame(string invitationCode)
        {
            if (Rooms.Keys.Contains(invitationCode))
            {
                List<string> users = GetUserListFromDtoList(Rooms[invitationCode]);
                bool playerLeftTheGame = false;
                List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
                foreach (var other in Rooms[invitationCode])
                {
                    try
                    {
                        other.Connection.OpenGame(other.UserName, users);
                    }
                    catch(CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(other);
                    }
                }
                if (playerLeftTheGame)
                {
                    foreach (var player in playersToDelete)
                    {
                        DeletePlayer(invitationCode, player.UserName);
                    }
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
            bool playerLeftTheGame = false;
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    if (username == other.UserName)
                    {
                        con = other.Connection;
                        try
                        {
                            con.ReceiveCard(card);
                        }
                        catch (CommunicationObjectAbortedException)
                        {
                            playerLeftTheGame = true;
                            playersToDelete.Add(other);
                        }

                    }
                }
                if (playerLeftTheGame)
                {
                    UpdatePlayers(invitationCode, playersToDelete);
                }
            }
        }
        public void NextTurn(string invitationCode, string username)
        {
            List<DTOUserChat> users = Rooms[invitationCode];
            bool playerLeftTheGame = false;
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                if (user.UserName != username)
                {
                    users[i].ActiveTurn = false;
                    user.Connection.itsMyTurn(false);
                }
                else
                {
                    try
                    {
                        user.Connection.itsMyTurn(true);
                        users[i].ActiveTurn = true;
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(user);
                        if (i == users.Count - 1)
                        {
                            users[0].Connection.itsMyTurn(true);
                            users[0].ActiveTurn = true;
                        }
                        else
                        {
                            users[i + 1].Connection.itsMyTurn(true);
                            users[i + 1].ActiveTurn = true;
                        }
                    }
                }
            }
            if (playerLeftTheGame)
            {
                UpdatePlayers(invitationCode, playersToDelete);
            }
        }

        public void RequestChangeDirection(string invitationCode)
        {
            IChatClient con;
            bool playerLeftTheGame = false;
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    try
                    {
                        con.ChangeDirection();
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(other);
                    }
                }
                if (playerLeftTheGame)
                {
                    UpdatePlayers(invitationCode, playersToDelete);
                }
            }
        }

        public void SendTurnInformation(string invitationCode, string color, string actualTurn)
        {
            IChatClient con;
            bool playerLeftTheGame = false;
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    try
                    {
                        con.ReceiveTurnInformation(color, actualTurn);
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(other);
                    }
                }
                if (playerLeftTheGame)
                {
                    UpdatePlayers(invitationCode, playersToDelete);
                }
            }
        }

        public void SendWinner(string invitationCode, string username)
        {
            IChatClient con;
            bool playerLeftTheGame = false;
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    try
                    {
                        con.ReceiveWinner(username);
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(other);
                    }
                }
                if (playerLeftTheGame)
                {
                    UpdatePlayers(invitationCode, playersToDelete);
                }
            }
        }

        public void SendPlayerUno(string invitationCode, string username, bool hasUno)
        {
            IChatClient con;
            bool playerLeftTheGame = false;
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            if (Rooms.Keys.Contains(invitationCode))
            {
                foreach (var other in Rooms[invitationCode])
                {
                    con = other.Connection;
                    try
                    {
                        con.ReceivePlayerUno(username, hasUno);
                    }
                    catch (CommunicationObjectAbortedException)
                    {
                        playerLeftTheGame = true;
                        playersToDelete.Add(other);
                    }
                }
                if (playerLeftTheGame)
                {
                    UpdatePlayers(invitationCode, playersToDelete);
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
                    Player findPlayer = context.PlayerSet1.Where(x => x.Credentials.username == credentials.Username)
                        .FirstOrDefault();
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
                    Credentials findCredentials = dataBase.CredentialsSet1
                        .Where(x => x.username == credentials.Username && x.password == credentials.Password)
                        .FirstOrDefault();
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
                    Credentials findCredentials = dataBase.CredentialsSet1
                        .Where(x => x.username == credentials.Username).FirstOrDefault();
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


        public DTOPlayer GetPlayer(string playerName)
        {
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    Player playerDb = dataBase.PlayerSet1.Where(x => x.Credentials.username == playerName)
                        .FirstOrDefault();
                    if (playerDb != null)
                    {
                        DTOPlayer dTOPlayer = new DTOPlayer();
                        dTOPlayer.Credentials.Email = playerDb.Credentials.email;
                        dTOPlayer.Credentials.Username = playerDb.Credentials.username;
                        dTOPlayer.Credentials.Password = playerDb.Credentials.password;
                        dTOPlayer.Credentials.Id = playerDb.Credentials.Id;
                        dTOPlayer.Image = playerDb.Images.path;
                        return dTOPlayer;
                    }
                    else
                    {
                        return new DTOPlayer();
                    }


                }
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }


        }

        /// <summary>
        /// Esta funcion sirve para modificar los datos de un jugador
        /// </summary>
        /// <param name="player"></param>
        /// <param name="username"></param>
        /// <returns>1 si la modificacion es exitosa 2 si el username ya esta ocupado</returns>
        /// <exception cref="EntityException"></exception>
        public int SetPlayer(DTOPlayer player, string username)
        {
            Player playerDb = null;
            int flag = 1;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    //buscar el username
                    playerDb = dataBase.PlayerSet1.Where(x => x.Credentials.username == username).FirstOrDefault();
                    //modifificar emal
                    playerDb.Credentials.email = player.Credentials.Email;
                    dataBase.PlayerSet1.Attach(playerDb);
                    dataBase.SaveChanges();
                    //modificar password
                    playerDb.Credentials.password = player.Credentials.Password;
                    dataBase.PlayerSet1.Attach(playerDb);
                    dataBase.SaveChanges();

                    //modificar imagen
                    //buscar imagen
                    Images findImage = dataBase.ImagesSet1.Where(x => x.path == player.Image).FirstOrDefault();
                    if (findImage == null)
                    {
                        Images newImage = new Images();
                        newImage.path = player.Image;
                        Images imagesdb = dataBase.ImagesSet1.Add(newImage);
                        playerDb.Images = imagesdb;
                        dataBase.PlayerSet1.Attach(playerDb);
                        dataBase.SaveChanges();

                    }
                    else
                    {
                        playerDb.Images = findImage;
                        dataBase.PlayerSet1.Attach(playerDb);
                        dataBase.SaveChanges();
                    }

                    //modificar username
                    if (username != player.Credentials.Username)
                    {
                        Player findPlayer = dataBase.PlayerSet1
                            .Where(x => x.Credentials.username == player.Credentials.Username).FirstOrDefault();
                        if (findPlayer != null)
                        {
                            flag = 2;

                        }
                        else
                        {
                            playerDb.Credentials.username = player.Credentials.Username;
                            dataBase.PlayerSet1.Attach(playerDb);
                            dataBase.SaveChanges();

                        }
                    }

                    return flag;

                }
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
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
        public bool DeleteFriend(string playerName, string friendName)
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
                        playerDb.Friends.Remove(friendDb);
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
    }
}
