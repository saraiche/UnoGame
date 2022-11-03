using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Logic.Proxy;

namespace Logic
{
    public class CallChatService : IChatServiceCallback
    {
        public InstanceContext InstanceContext { get; set; }
        public ChatServiceClient ChatServiceClient { get; set; }   
        public List<string> Messages { get; set; }

        public List<string> Users { get; set; }
        public CallChatService()
        {
            InstanceContext = new InstanceContext(this);
            ChatServiceClient = new ChatServiceClient(InstanceContext);
            Messages = new List<string>();
            Users = new List<string>();
        }

        public void RecieveMessage(string user, string message)
        {
            Messages.Add(user+ ": " + message);
        }

        public void SendMessage(string username, string message, string invitationCode)
        {
            ChatServiceClient.SendMessage(username,message, invitationCode);

        }
        public void Join(string username, string code)
        {
             ChatServiceClient.Join(username, code);
        }

        public void GetUsers(string user)
        {
            Users.Add(user);
        }

        public string NewRoom(string username)
        {
           return ChatServiceClient.NewRoom(username);
        }
    }
}
