using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unoProyect.Proxy;
using unoProyect.Security;


namespace unoProyect.Logic
{
     public class CallDataService
    {
        public Lobby LobbyView { get; set; }
        public Login LoginView { get; set; }

        Proxy.DataServiceClient dataServiceClient = new Proxy.DataServiceClient();
        public int AddCredentials(string username, string password, string email)
        {
            int result = 0;
            Proxy.DTOCredentials dTOcredentials = new Proxy.DTOCredentials();
            dTOcredentials.Username = username;
            dTOcredentials.Password = Utilities.ComputeSHA256Hash(password);
            dTOcredentials.Email = email;
            result = dataServiceClient.AddCredentials(dTOcredentials);
            return result;
        }

        public bool IsUser(string username, string password)
        {
            bool result = false;
            Proxy.DTOCredentials isUser = new Proxy.DTOCredentials();
            isUser.Password = password;
            isUser.Username = username;
            try
            {
                result = dataServiceClient.IsUser(isUser);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
       public bool SearchUser(string username)
        {
            bool result = false;
            Proxy.DTOCredentials searchUser = new Proxy.DTOCredentials();
            searchUser.Username = username;
            try
            {
                result = dataServiceClient.SearchUser(searchUser);
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
            return result;
        }
        public bool AddFriend(string playerName, string friendName)
        {
            return dataServiceClient.AddFriend(playerName, friendName);
        }
        public List<string> GetFriends(string playerName)
        {
            return dataServiceClient.GetFriends(playerName).ToList();
        }
        public DTOCredentials SearchUserByUsername(string username)
        {
            DTOCredentials result = dataServiceClient.SearchUserByUsername(username);
            return result;
        }
        public bool ModifyPassword(string username, string password)
        {
            return dataServiceClient.ModifyPassword(username, password);
        }
    }
}
