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
        public CallChatService()
        {
            InstanceContext = new InstanceContext(this);
            ChatServiceClient = new ChatServiceClient(InstanceContext);
        }

        public void RecieveMessage(string user, string message)
        {
            Messages.Add(user+ ": " + message);
        }
        public int GetInvitationCode()
        {
            return ChatServiceClient.GetInvitationCode();
        }
        public void SendMessage(string message)
        {
            ChatServiceClient.SendMessage(message);

        }
        public int Join(string username, int code)
        {
            return ChatServiceClient.Join(username, code);
        }


    }
}
