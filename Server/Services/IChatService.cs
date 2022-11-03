using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(String user, string message);
        [OperationContract(IsOneWay = true)]
        void GetUsers(string user);
        
    }
    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Join(string username, string code);
        [OperationContract(IsOneWay = true)]
        void SendMessage(string username,string message, string invitationCode);
       

        [OperationContract]
        string NewRoom(string username);
    }
    [DataContract]
    public class DTOUserChat
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public IChatClient Connection { get; set; } 
    }
    

}
