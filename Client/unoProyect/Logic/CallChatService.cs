﻿using unoProyect.Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace unoProyect.Logic
{
    public class CallChatService : IChatServiceCallback
    {
        public InstanceContext InstanceContext { get; set; }
        public ChatServiceClient ChatServiceClient { get; set; }
        public Lobby Lobby { get; set; }
    
        public ObservableCollection<string> Users { get; set; }
        public CallChatService()
        {
            InstanceContext = new InstanceContext(this);
            ChatServiceClient = new ChatServiceClient(InstanceContext);
            Users = new ObservableCollection<string>();
            Lobby = new Lobby();
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
            Lobby.LvChat.Items.Add(user + " : " + message);
        }

        public void GetUsers(string user)
        {
            Lobby.LvFriendList.Items.Add(user);
        }
    }
}