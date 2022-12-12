using unoProyect.Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Markup;
using System.Windows;

namespace unoProyect.Logic
{
    public class CallChatService : IChatServiceCallback
    {
        public InstanceContext InstanceContext { get; set; }
        public ChatServiceClient ChatServiceClient { get; set; }
        public Lobby LobbyView { get; set; }
        public Login LoginView { get; set; }
        public Game GameView { get; set; }
        public ObservableCollection<string> Users { get; set; }
        public string[] players = new string[10];
        public CallChatService()
        {
            InstanceContext = new InstanceContext(this);
            ChatServiceClient = new ChatServiceClient(InstanceContext);
            Users = new ObservableCollection<string>();
            LobbyView = new Lobby();
        }

        public void SendMessage(string username, string message, string invitationCode)
        {
            ChatServiceClient.SendMessage(username, message, invitationCode);

        }
        public bool Join(string username, string code)
        {
            return ChatServiceClient.Join(username, code);
        }

        public void GetUsersChat(string code, string username)
        {
            ChatServiceClient.GetUsersChat(code, username);
        }
        public string NewRoom(string username)
        {
            return ChatServiceClient.NewRoom(username);
        }

        public void RecieveMessage(string user, string message)
        {
            LobbyView.LvChat.Items.Add(user + " : " + message);
        }

        public void GetUsers(string user)
        {
            LobbyView.LvFriendList.Items.Add(user);
        }
        public void ReceiveCenter(Card center)
        {
            GameView.PutCardOnCenter(center);
        }

        public void OpenGame(string username, string[] players)
        {
            if (this.LobbyView != null)
            {
                GameView = new Game(username, LobbyView.InvitationCode);
                GameView.PutUsernames(players);
                if (username == players.First())
                {
                    GameView.DealFirstCards();
                    GameView.InitTurn();
                }
                LobbyView.NavigationService.Navigate(GameView);
            }
        }

        public void RequestOpenGame(string invitationCode)
        {
            ChatServiceClient.RequestOpenGame(invitationCode);
        }

        public string[] GetPlayersByInvitationCode(string invitationCode)
        {
            string[] players = ChatServiceClient.GetPlayersByInvitationCode(invitationCode);
            return players;
        }

        public void PutCardInCenter(string invitationCode, Card card)
        {
            try
            {
                ChatServiceClient.PutCardInCenter(invitationCode, card);
            }
            catch(System.ServiceModel.CommunicationException)
            {
                //do something
            }
        }


        public void itsMyTurn(bool myturn)
        {
            GameView.BtnUseCard.IsEnabled = myturn;
            GameView.BtnStack.IsEnabled = myturn;
            GameView.BtnPaso.Visibility = Visibility.Hidden;
        }
        public void NextTurn(string invitationCode, string username)
        {
            ChatServiceClient.NextTurn(invitationCode, username);
        }
        public void ReceiveCard(Card card)
        {
            GameView.AddCard(card);
        }

        public void DealCard(string username, Card card, string invitationCode)
        {
            ChatServiceClient.DealCard(username, card, invitationCode);
        }

        public void RequestChangeDirection(string invitationCode)
        {
            ChatServiceClient.RequestChangeDirection(invitationCode);
        }
        public void ChangeDirection()
        {
            GameView.ChangeDirection();
        }

        public void SendTurnInformation(string invitationCode, string color, string actualTurn)
        {
            ChatServiceClient.SendTurnInformation(invitationCode, color, actualTurn);
        }

        public void ReceiveTurnInformation(string color, string actualTurn)
        {
            GameView.UpdateTurnInformation(color, actualTurn);
        }

        public void ReceiveWinner(string username)
        {
            GameView.ShowWinner(username);
            GameView.NavigationService.Navigate(LobbyView);
        }

        public void SendWinner(string invitationCode, string username)
        {
            ChatServiceClient.SendWinner(invitationCode, username);
        }
        public bool DeletePlayer(string code, string username)
        {
            return ChatServiceClient.DeletePlayer(code, username);
        }
        public void SendPlayerUno(string invitationCode, string username, bool hasUno)
        {
            ChatServiceClient.SendPlayerUno(invitationCode, username, hasUno);
        }

        public void ReceivePlayerUno(string username, bool hasUno)
        {
            if (hasUno == true)
            {
                GameView.PlayerSaidUno(username);
            }
            else
            {
                GameView.PlayerWithoutUno(username);
            }
        }

        public void DeletePlayerFromGame(string username, string[] playersUpdated)
        {
            GameView.Players = playersUpdated;
            GameView.PlayerLeftGame(username);
        }
    }
}
