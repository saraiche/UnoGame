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
using System.CodeDom;

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

        private const int SUCCESFUL = 1;
        private const int ERROR = 0;
        private const int EXCEPTION = 2;
        public CallChatService()
        {
            InstanceContext = new InstanceContext(this);
            ChatServiceClient = new ChatServiceClient(InstanceContext);
            Users = new ObservableCollection<string>();
            LobbyView = new Lobby();
        }

        public void SendMessage(string username, string message, string invitationCode)
        {
            try
            {
                ChatServiceClient.SendMessage(username, message, invitationCode);
            }
            catch(EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }
        public int Join(string username, string code)
        {
            int result = ERROR;
            try
            {
                if (ChatServiceClient.Join(username, code))
                {
                    result = SUCCESFUL;
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
                result = EXCEPTION;
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
                result = EXCEPTION;
            }
            return result;
        }

        public void GetUsersChat(string code, string username)
        {
            try
            {
                ChatServiceClient.GetUsersChat(code, username);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }
        public string NewRoom(string username)
        {
            string codeRoom = null;
            try
            {
                codeRoom = ChatServiceClient.NewRoom(username);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
            return codeRoom;
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
            try
            {
                ChatServiceClient.RequestOpenGame(invitationCode);
            }
            catch(EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }

        public string[] GetPlayersByInvitationCode(string invitationCode)
        {
            string[] activePlayers = null;
            try
            {
                activePlayers = ChatServiceClient.GetPlayersByInvitationCode(invitationCode);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
            return activePlayers;
        }

        public void PutCardInCenter(string invitationCode, Card card)
        {
            try
            {
                ChatServiceClient.PutCardInCenter(invitationCode, card);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
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
            try
            {
                ChatServiceClient.NextTurn(invitationCode, username);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }
        public void ReceiveCard(Card card)
        {
            GameView.AddCard(card);
        }

        public void DealCard(string username, Card card, string invitationCode)
        {
            try
            {
                ChatServiceClient.DealCard(username, card, invitationCode);
            }
            catch (EndpointNotFoundException)
            {
                throw new EndpointNotFoundException();
            }
            catch (CommunicationObjectFaultedException)
            {
                throw new EndpointNotFoundException();
            }
        }

        public void RequestChangeDirection(string invitationCode)
        {
            try
            {
                ChatServiceClient.RequestChangeDirection(invitationCode);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }
        public void ChangeDirection()
        {
            GameView.ChangeDirection();
        }

        public void SendTurnInformation(string invitationCode, string color, string actualTurn)
        {
            try
            {
                ChatServiceClient.SendTurnInformation(invitationCode, color, actualTurn);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
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
            try
            {
                ChatServiceClient.SendWinner(invitationCode, username);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }
        public bool DeletePlayer(string code, string username)
        {
            bool result = false;
            try
            {
                result = ChatServiceClient.DeletePlayer(code, username);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
            return result;
        }
        public void SendPlayerUno(string invitationCode, string username, bool hasUno)
        {
            try
            {
                ChatServiceClient.SendPlayerUno(invitationCode, username, hasUno);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }

        public void ReceivePlayerUno(string username, bool hasUno)
        {
            if (hasUno)
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

        public void ValidateConnection(string invitationCode)
        {
            try
            {
                ChatServiceClient.ValidateConnection(invitationCode);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);

            }
            catch (CommunicationObjectFaultedException)
            {
                MessageBox.Show(Properties.Resources.somethingWrong);
            }
        }
    }
}
