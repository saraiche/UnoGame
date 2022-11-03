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
        [OperationContract(IsOneWay = true)]
        void ReceiveCenter(string center);
        [OperationContract(IsOneWay = true)]
        void OpenGame(string username);
        

    }
    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatService
    {
        [OperationContract]
        bool Join(string username, string code);
        [OperationContract(IsOneWay = true)]
        void SendMessage(string username,string message, string invitationCode);
        [OperationContract]
        void GetUsersChat(string code);

        [OperationContract]
        string NewRoom(string username);

        [OperationContract]
        List<string> GetPlayersByInvitationCode(string invitationCode);

        [OperationContract]
        void RequestOpenGame(string invitationCode);

        [OperationContract]
        void PutCardInCenter(string invitationCode, string card);

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
