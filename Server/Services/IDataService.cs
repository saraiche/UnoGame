using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Services
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        int AddCredentials(DTOCredentials credentials);

        [OperationContract]
        bool IsUser(DTOCredentials credentials);

        [OperationContract]
        bool SearchUser(DTOCredentials credentials);

        [OperationContract]
        bool AddFriend(string playerName, string friendName);
        [OperationContract]
        List<string> GetFriends(string playerName);
        [OperationContract]
        DTOCredentials SearchUserByUsername(string username);
        [OperationContract]
        bool ModifyPassword(string playerName, string password);
        [OperationContract]
        DTOPlayer GetPlayer(string playerName);
        [OperationContract]
        int SetPlayer(DTOPlayer player, string username);
        [OperationContract]
        bool DeleteFriend(string playerName, string friendName);
    }
    [DataContract]
    public class DTOCredentials
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }

        public DTOCredentials()
        {
            Id = 0;
            Password = "";
            Username = "";
            Email = "";
        }
    }
    [DataContract]
    public class DTOPlayer
    {
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public DTOCredentials Credentials { get; set; }

        public DTOPlayer()
        {
            Image = "";
            Credentials = new DTOCredentials();
        }
    }
}
