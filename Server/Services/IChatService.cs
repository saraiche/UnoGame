using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
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
        void OpenGame(string username, List<string> players);
        [OperationContract(IsOneWay = true)]
        void itsMyTurn(bool myturn);
        [OperationContract(IsOneWay = true)]
        void ReceiveCard(string card);
        [OperationContract(IsOneWay = true)]
        void ChangeDirection();
        [OperationContract(IsOneWay = true)]
        void ReceiveTurnInformation(string color, string actualTurn);
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
        [OperationContract]
        void NextTurn(string invitationCode, string username);

        [OperationContract]
        void DealCard(string username, string card, string invitationCode);

        [OperationContract]
        void RequestChangeDirection(string invitationCode);
        [OperationContract]
        void SendTurnInformation(string invitationCode, string color, string actualTurn);
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
