using System;
using System.Collections.Generic;
using System.Configuration;
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

        /// <summary>
        /// Une a un usuario a una partida que ya ha sido creada
        /// </summary>
        /// <param name="username"></param>
        /// <param name="code"></param>
        /// <returns>
        /// True si se unió correctamente el usuario a la partida
        /// Falso si no existe la partida o no se pudo crear
        /// </returns>
        public bool Join(string username, string code)
        {
            bool flag = false;
            DTOUserChat newUser = new DTOUserChat();
            List<DTOUserChat> users = new List<DTOUserChat>();
            if (Rooms.Keys.Contains(code) && Rooms[code].FirstOrDefault(x => x.UserName == username) == null)
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

        /// <summary>
        /// Envía un mensaje a todos los usuarios de una partida, 
        /// incluyendo al emisor del mensaje. Verifica que todos los jugadores 
        /// mantengan una conexión activa y si no, los elimina de la partida.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <param name="invitationCode"></param>
        public void SendMessage(string username, string message, string invitationCode)
        {
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();

            if (Rooms.Keys.Contains(invitationCode) && Rooms[invitationCode].FirstOrDefault(x => x.UserName == username) != null)
            {

                foreach (var other in Rooms[invitationCode])
                {
                    try
                    {
                        other.Connection.RecieveMessage(username, message);

                    }
                    catch (System.ServiceModel.CommunicationException)
                    {
                        playersToDelete.Add(other);
                    }
                }
            }
            foreach (var player in playersToDelete)
            {
                DeletePlayer(invitationCode, player.UserName);
            }
        }
        /// <summary>
        /// Envía el usuario recibido a cada usuario de la partida seleccionada
        /// </summary>
        /// <param name="code"></param>
        /// <param name="username"></param>
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
        /// <summary>
        /// Crea una nueva partida, genera su código y verifica que no exista una 
        /// partida con ese código y une al jugador que la creó a la partida
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// Una cadena con el código de invitación de 6 dígitos si fue correcto
        /// Null en caso contrario
        /// </returns>
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
        /// <summary>
        /// Regresa una lista con los nombres de usuario de los jugadores de una 
        /// partida en específico
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns>Lista de strings con los nombres de usuario</returns>
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
        /// <summary>
        /// Solicita a los clientes abrir el juego una vez que ha iniciado la partida
        /// </summary>
        /// <param name="invitationCode"></param>
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
                    catch (CommunicationObjectAbortedException)
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
        /// <summary>
        /// Coloca una carta en el centro de una partida
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="card"></param>
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
        /// <summary>
        /// Pasa el turno actual de una partida al jugador recibido
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="username"></param>
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
        /// <summary>
        /// Reparte una carta al jugador de una partida, agregandola a su mazo.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="card"></param>
        /// <param name="invitationCode"></param>
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
        /// <summary>
        /// Cambia el sentido de la partida a todos los jugadores cuando se ha puesto
        /// una carta de reversa
        /// </summary>
        /// <param name="invitationCode"></param>
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
        /// <summary>
        /// Envía el color y turno actual de una partida a todos los jugadores
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="color"></param>
        /// <param name="actualTurn"></param>
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
        /// <summary>
        /// Avisa quién ha ganado la partida a todos los jugadores de la partida
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="username"></param>
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
        /// <summary>
        /// Elimina a un jugador de la lista de jugadores de la partida.
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="username"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Avisa a todos los jugadores que un jugador ha presionado el botón Uno y que 
        /// solo le queda una carta
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="username"></param>
        /// <param name="hasUno"></param>
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
        /// <summary>
        /// Valida que los usuarios de una partida sigan conectados
        /// </summary>
        /// <param name="invitationCode"></param>
        public void ValidateConnection(string invitationCode)
        {
            List<DTOUserChat> playersToDelete = new List<DTOUserChat>();
            foreach (var other in Rooms[invitationCode])
            {
                try
                {
                    other.Connection.RecieveMessage("", "");

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
        /// <summary>
        /// Actualiza la lista de jugadores de una partida cuando algún jugador se ha salido del juego
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="pastList"></param>
        private void UpdatePlayers(string invitationCode, List<DTOUserChat> pastList)
        {
            foreach(var player in pastList.Select(player => player.UserName))
            {
                DeletePlayer(invitationCode, player);
                SendDeletePlayerFromGame(invitationCode, player);
            }
        }
        /// <summary>
        /// Avisa a los jugadores que el jugador recibido ha dejado la partida
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <param name="username"></param>
        private void SendDeletePlayerFromGame(string invitationCode, string username)
        {
            foreach(var player in Rooms[invitationCode])
            {
                player.Connection.DeletePlayerFromGame(username, GetUserListFromDtoList(Rooms[invitationCode]));
            }
        }
        /// <summary>
        /// Extrae los nombres de usuario de una lista de DTOUserChat para agregarlos a una lista
        /// de strings
        /// </summary>
        /// <param name="dtoUserChats"></param>
        /// <returns>
        /// Lista de strings con los nombres de usuario de los jugadores de una partida
        /// </returns>
        private List<string> GetUserListFromDtoList(List<DTOUserChat> dtoUserChats)
        {
            List<string> users = new List<string>();
            foreach (var user in dtoUserChats)
            {
                users.Add(user.UserName);
            }
            return users;
        }
    }


    public partial class ServiceImplementation : IDataService
    {
        /// <summary>
        /// Agrega las credenciales de un jugador a la base de datos
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns> 
        /// 1 si se guardaron correctamente los cambios,
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
        /// <summary>
        /// Busca una coincidencia según un nombre de usuario y contraseña en la 
        /// base de datos
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>
        /// True si se encontró una coincidencia
        /// False en otro caso
        /// </returns>
        public bool IsUser(DTOCredentials credentials)
        {
            bool flag = false;
            credentials.Password = Security.ComputeSHA256Hash(credentials.Password);
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    //buscar credenciales
                    Credentials findCredentials = dataBase.CredentialsSet1
                        .Where(x => x.username == credentials.Username && x.password == credentials.Password)
                        .FirstOrDefault();
                    if (findCredentials != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (EntityException)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// Busca un usuario en la base de datos según su nombre de usuario
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>
        /// True si existe
        /// False en caso contrario
        /// </returns>
        /// <exception cref="EntityException"></exception>
        public bool SearchUser(DTOCredentials credentials)
        {
            bool flag = false;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
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
        /// <summary>
        /// Agrega un jugador a la lista de amigos del jugador indicado
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="friendName"></param>
        /// <returns>
        /// True si el amigo existe, el jugador existe y se pudo agregar correctamente
        /// False en otro caso
        /// </returns>
        public bool AddFriend(string playerName, string friendName)
        {
            bool flag = false;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    Credentials findCredentialsFriend = dataBase.CredentialsSet1.Where(x => x.username == friendName).FirstOrDefault();
                    if (findCredentialsPlayer != null && findCredentialsFriend != null)
                    {
                        Player playerDb = findCredentialsPlayer.Player;
                        Player friendDb = findCredentialsFriend.Player;
                        dataBase.PlayerSet1.Attach(friendDb);
                        playerDb.Friends.Add(friendDb);
                        dataBase.PlayerSet1.Attach(playerDb);
                        dataBase.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch (EntityException)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// Retorna los nombres de usuario de los jugadores de la lista de amigos del
        /// jugador recibido
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>
        /// Lista de nombres de usuario
        /// </returns>
        /// <exception cref="EntityException"></exception>
        public List<string> GetFriends(string playerName)
        {
            List<string> friends = new List<string>();
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    if (findCredentialsPlayer != null)
                    {
                        Player playerDb = findCredentialsPlayer.Player;
                        List<Player> friendsDb = playerDb.Friends.ToList();
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
        /// <summary>
        /// Busca un jugador por su nombre de usuario y regresa sus credenciales, sin incluir su contraseña
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// DTOCredentials con el nombre de usuario y correo electrónico del jugador encontrado
        /// </returns>
        /// <exception cref="EntityException"></exception>
        public DTOCredentials SearchUserByUsername(string username)
        {
            DTOCredentials userFound = new DTOCredentials();
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
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
        /// <summary>
        /// Cambia la contraseña de un usuario registrado
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="password"></param>
        /// <returns>
        /// True si se pudo actualizar la contraseña con éxito
        /// False en caso contrario
        /// </returns>
        /// <exception cref="DbUpdateException"></exception>
        public bool ModifyPassword(string playerName, string password)
        {
            bool result = false;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    if (findCredentialsPlayer != null)
                    {
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
        /// <summary>
        /// Obtiene el perfil de un jugador según su nombre de usuario
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>
        /// DTOPlayer con el email, nombre de usuario, contraseña e imagen del jugador recibido
        /// </returns>
        /// <exception cref="EntityException"></exception>
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
        /// Modifica los datos de un jugador
        /// </summary>
        /// <param name="player"></param>
        /// <param name="username"></param>
        /// <returns>
        /// 1 si la modificacion es exitosa 
        /// 2 si el username ya esta ocupado
        /// </returns>
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
                    if (playerDb != null)
                    {
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
                    }
                    return flag;
                }
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
        }
        /// <summary>
        /// Elimina a un jugador de la lista de amigos del jugador recibido
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="friendName"></param>
        /// <returns>
        /// True si se realizó de manera exitosa la eliminación
        /// False en caso contrario
        /// </returns>
        /// <exception cref="EntityException"></exception>
        public bool DeleteFriend(string playerName, string friendName)
        {
            bool flag = false;
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    Credentials findCredentialsPlayer = dataBase.CredentialsSet1.Where(x => x.username == playerName).FirstOrDefault();
                    Credentials findCredentialsFriend = dataBase.CredentialsSet1.Where(x => x.username == friendName).FirstOrDefault();
                    if (findCredentialsPlayer != null && findCredentialsFriend != null)
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
        /// <summary>
        /// Convierte un data transfer object a entity para conectarse con la capa de DataAccess
        /// </summary>
        /// <param name="dTOCredentials"></param>
        /// <returns></returns>
        private Credentials DtoCredentialsToEntity(DTOCredentials dTOCredentials)
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
