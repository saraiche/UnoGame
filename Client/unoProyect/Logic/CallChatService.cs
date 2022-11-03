using unoProyect.Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace unoProyect.Logic
{
    public class CallChatService : IChatServiceCallback
    {
        public InstanceContext InstanceContext { get; set; }
        public ChatServiceClient ChatServiceClient { get; set; }
        public List<string> Messages { get; set; }
        public string Message { get; set; }

        public ObservableCollection<string> Users { get; set; }
        public CallChatService()
        {
            InstanceContext = new InstanceContext(this);
            ChatServiceClient = new ChatServiceClient(InstanceContext);
            Messages = new List<string>();
            Users = new ObservableCollection<string>();
        }

        public void SendMessage(string username, string message, string invitationCode)
        {
            ChatServiceClient.SendMessage(username, message, invitationCode);

        }
        public bool Join(string username, string code)
        {
            return ChatServiceClient.Join(username, code);
        }

        public void GetUsersChat(string code)
        {
            ChatServiceClient.GetUsersChat(code);
        }
        public string NewRoom(string username)
        {
            return ChatServiceClient.NewRoom(username);
        }

        public void RecieveMessage(string user, string message)
        {
            Message = user + ": " + message;
            Console.WriteLine(Message);
        }

        public void GetUsers(string user)
        {
            Console.WriteLine(user);
        }
    }
}
