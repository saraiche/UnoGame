using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
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
            }catch(System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.error, "");
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
            catch (EntityException)
            {
                MessageBox.Show(Properties.Resources.invalidPasswordOrEmail, "");
            }
            return result;
        }

        public string SendMail(string email, string emailSubject)
        {
            string code = "";
            code = (new Random().Next(100000, 999999)).ToString();
            bool result = dataServiceClient.SendMail(email, emailSubject, "El código es: " + code);
            if (!result)
            {
                code = "";
            }
            return code;
        }
        public bool AddFriend(string playerName, string friendName)
        {
            return dataServiceClient.AddFriend(playerName, friendName);
        }
        public List<string> GetFriends(string playerName)
        {
            return dataServiceClient.GetFriends(playerName).ToList();
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
