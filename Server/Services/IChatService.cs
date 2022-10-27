using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    internal interface IChatService
    {
        
        [OperationContract]
        ChatUser ClientConnect(string username);
        [OperationContract]
        List<ChatUser> GetChatUsers();
        [OperationContract]
        void SendNewMessage(ChatMessage chatMessage);
        [OperationContract]
        void RemoveUser(ChatUser user);

    }
    [DataContract]
    public class ChatUser{
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public string Hostname { get; set; }
    }
    [DataContract]
    public class ChatMessage
    {
        public string Message { get; set; }
        public ChatUser User { get; set; }
    }

}
