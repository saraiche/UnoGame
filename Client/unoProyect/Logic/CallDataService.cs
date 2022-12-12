using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using unoProyect.Proxy;
using unoProyect.Security;


namespace unoProyect.Logic
{
     public class CallDataService
    {
        public Lobby LobbyView { get; set; }
        public Login LoginView { get; set; }

        Proxy.DataServiceClient dataServiceClient = new Proxy.DataServiceClient();
        private const int SUCCESFUL = 1;
        private const int ERROR = 0;
        private const int EXCEPTION = 2;
        public int AddCredentials(string username, string password, string email)
        {
            int result = ERROR;
            Proxy.DTOCredentials dTOcredentials = new Proxy.DTOCredentials();
            dTOcredentials.Username = username;
            dTOcredentials.Password = password;
            dTOcredentials.Email = email;
            try
            {
                result = dataServiceClient.AddCredentials(dTOcredentials);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            return result;
        }

        public int IsUser(string username, string password)
        {
            int result = ERROR;
            Proxy.DTOCredentials isUser = new Proxy.DTOCredentials();
            isUser.Password = password;
            isUser.Username = username;
            try
            {
                if (dataServiceClient.IsUser(isUser))
                {
                    result = SUCCESFUL;
                }
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.informationWrongSignUp);
                result = EXCEPTION;
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
                result = EXCEPTION;
            }
            
            return result;
        }
       public int SearchUser(string username)
        {
            int result = ERROR;
            Proxy.DTOCredentials searchUser = new Proxy.DTOCredentials();
            searchUser.Username = username;
            try
            {
                if (dataServiceClient.SearchUser(searchUser))
                {
                    result = SUCCESFUL;
                }
                
            }
            catch (EntityException)
            {
                MessageBox.Show(Properties.Resources.invalidPasswordOrEmail, "");
                result = EXCEPTION;
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
                result = EXCEPTION;
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
        public DTOPlayer GetPlayer(string username)
        {
            return dataServiceClient.GetPlayer(username);
        }
        public int SetPlayer(DTOPlayer player, string username)
        {
            return dataServiceClient.SetPlayer(player, username);
        }
        public bool DeleteFriend(string playerName, string friendName)
        {
            return dataServiceClient.DeleteFriend(playerName, friendName);
        }
    }
}
