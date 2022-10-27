using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ChatEngine
    {
        
        /// <summary>
        /// Almacena los usuarios conectados
        /// </summary>
        private List<ChatUser> connectedUsers = new List<ChatUser>();
        /// <summary>
        /// Diccionario que guarda los mensajes a entregar
        /// </summary>
        private Dictionary<string, List<ChatMessage>> incommingMessage = new Dictionary<string, List<ChatMessage>>();
        public List<ChatUser> ConnecteUsers
        {
            get { return connectedUsers; }


            set { connectedUsers = value; }
        }
        /// <summary>
        /// Este metodo retorna a los usuarios para poderlos agregar a una lista de usuarios conectados
        /// </summary>
        /// <param name="user"></param>
        /// <returns>ChatUser</returns>
        public ChatUser AddNewChatUser(ChatUser user)
        {
            var exist = from ChatUser e in this.ConnecteUsers
                        where e.UserName == user.UserName
                        select e;
            if (exist.Count() == 0)
            {
                this.ConnecteUsers.Add(user);
                incommingMessage.Add(user.UserName, new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        User = user,
                        Message = "Enjoy memento mory"
                    }
                });
                Console.WriteLine("\nNew user connected" + user.UserName);
                return user;
            }
            else
                return null;
        }
        /// <summary>
        /// Este metodo agregar al diccionario los mensajes
        /// </summary>
        /// <param name="newMessage"></param>
        public void AddNewMessage(ChatMessage newMessage)
        {
            foreach (var user in this.ConnecteUsers)
            {
                if (!newMessage.User.UserName.Equals(user.UserName))
                {
                    incommingMessage[user.UserName].Add(newMessage);
                }
            }
        }

        public List<ChatMessage> GetNewMessages(ChatUser user)
        {
            List<ChatMessage> myNewMessages = incommingMessage[user.UserName];
            incommingMessage[user.UserName] = new List<ChatMessage>();
            if(myNewMessages.Count > 0)
            {
                return myNewMessages;
            }else
                return null;
        }
    }
}
