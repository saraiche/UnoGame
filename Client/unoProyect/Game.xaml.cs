using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using unoProyect.Logic;
using unoProyect.Proxy;
using Image = System.Windows.Controls.Image;

namespace unoProyect
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public string Username { get; set; }
        public string InvitationCode { get; set; }
        public Logic.CallChatService CallChatService { get; set; }
        public string[] Players { get; set; }
        public Card Center { get; set; }
        public string ActualColor { get; set; }
        public bool Reverse { get; set; }

        private const string NUMERIC_SKIP_REVERSE = ""; 
        /// <summary>
        /// Inicia la pantalla del juego, inicializando los parámetros de username y code. 
        /// Coloca el nombre de usuario del jugador en un label e inicia el sentido del juego en "Sentido del reloj"
        /// </summary>
        /// <param name="username"></param>
        /// <param name="code"></param>
        public Game(string username, string code)
        {
            InitializeComponent();
            Username = username;
            InvitationCode = code;
            lblPlayer1.Content = Username;
            CallChatService = new Logic.CallChatService();
            Reverse = false;
            LblActualDirection.Content = "Sentido del reloj";
        }

        /// <summary>
        /// Coloca graficamente una imagen en el centro de la partida, recibiendo como parámetro la carta de tipo Card
        /// </summary>
        /// <param name="card"></param>
        public void PutCardOnCenter(Card card)
        {
            this.Center = card;
            ActualColor = card.Color;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(card.Url, UriKind.Relative);
            bi3.EndInit();
            imgCenter.Stretch = Stretch.Fill;
            imgCenter.Source = bi3;
        }

        /// <summary>
        /// Crea el mazo inicial de los jugadores de 7 cartas y lo reparte a cada uno de los jugadores de la partida
        /// </summary>
        public void DealFirstCards()
        {
            List<Card> cards = new List<Card>();
            cards = GameLogic.GetCards();
            int index = 0;
            int cardsSize = cards.Count;
            Random random = new Random();
            try
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        Card card;
                        index = random.Next(cardsSize);
                        card = cards.ElementAt(index);
                        CallChatService.DealCard(Players[i], card, InvitationCode);
                        cards.RemoveAt(index);
                        cardsSize--;
                    }
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
            }
            
        }

        /// <summary>
        /// Coloca los nombres de usuario de los contricantes de la partida, itera el arreglo con el módulo para simular un comportamiento
        /// circular por el orden de los jugadores en el arreglo.
        /// </summary>
        /// <param name="players"></param>
        public void PutUsernames(string[] players)
        {
            this.Players = players;
            int labelIterator = 2;
            int indexThisPlayer = 0;

            indexThisPlayer = GameLogic.GetIndexPlayer(Username, players);

            int iterador = indexThisPlayer + 1;
            while (iterador % players.Length != indexThisPlayer)
            {
                switch (labelIterator)
                {
                    case 2:
                        lblPlayer2.Content = players.ElementAt(iterador % players.Length);
                        labelIterator++;
                        iterador++;
                        break;
                    case 3:
                        lblPlayer3.Content = players.ElementAt(iterador % players.Length);
                        labelIterator++;
                        iterador++;
                        break;
                    case 4:
                        lblPlayer4.Content = players.ElementAt(iterador % players.Length);
                        iterador++;
                        break;

                }
            }

        }

        /// <summary>
        /// Comportamiento de selección de una carta por parte del usuario, manda a llamar al método de validar y si es comodín 
        /// presenta la interfaz para escoger un color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseCard_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image imageCard = LvCards.SelectedValue as System.Windows.Controls.Image;
            if (imageCard == null)
            {
                MessageBox.Show(Properties.Resources.instructionSelectACard);
            }
            else
            {
                Card card = GameLogic.GetCardById(imageCard.Name);
                if (GameLogic.IsValidCard(card, Center, ActualColor))
                {
                    Center = card;
                    if (card.Type == "wildcard" || card.Type == "draw4")
                    {
                        MessageBox.Show(Properties.Resources.chooseColor);
                        GrdColors.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        PlayCard("", imageCard);
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Resources.invalidCard);
                }
            }
        }

        /// <summary>
        /// Elige aleatoriamente a un jugador de la partida para jugar el primer turno
        /// </summary>
        public void InitTurn()
        {
            int PlayersOnGame = Players.Length;
            Random random = new Random();
            int indexFirstPlayer = random.Next(0, PlayersOnGame);
            CallChatService.NextTurn(InvitationCode, Players[indexFirstPlayer]);
        }

        /// <summary>
        /// Según el orden de los turnos, salta al siguiente turno, regresando al jugador siguiente del siguiente del actual
        /// </summary>
        /// <returns></returns>
        private string SkipPlayer()
        {
            string auxiliar = GameLogic.NextPlayer(Username, Players, Reverse);
            string nextPlayer = GameLogic.NextPlayer(auxiliar, Players, Reverse);
            return nextPlayer;
        }

        /// <summary>
        /// Cambia graficamente el sentido de la partida, cambiando las flechas de dirección y cambiando el label de dirección
        /// </summary>
        public void ChangeDirection()
        {
            Reverse = !(Reverse);
            if (Reverse)
            {
                imgDirectionNormalLeft.Visibility = Visibility.Visible;
                imgDirectionNormalRight.Visibility = Visibility.Visible;
                imgDirectionReverseLeft.Visibility = Visibility.Hidden;
                imgDirectionReverseRight.Visibility = Visibility.Hidden;
                LblActualDirection.Content = "Contrario al reloj";
            }
            else
            {
                imgDirectionNormalLeft.Visibility = Visibility.Hidden;
                imgDirectionNormalRight.Visibility = Visibility.Hidden;
                imgDirectionReverseLeft.Visibility = Visibility.Visible;
                imgDirectionReverseRight.Visibility = Visibility.Visible;
                LblActualDirection.Content = "Sentido del reloj";
            }
        }

        /// <summary>
        /// Actualiza el color y jugador actual del turno actual.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="actualPlayer"></param>
        public void UpdateTurnInformation(string color, string actualPlayer)
        {
            ActualColor = color;
            LblActualColor.Content = color;
            LblNowPlaying.Content = actualPlayer;
        }

        /// <summary>
        /// Lógica de jugar una carta +2, reversa o skip
        /// </summary>
        /// <param name="nextPlayer"></param>
        /// <returns>nextPlayer el jugador siguiente según la carta recibida</returns>
        private string PlayNumericSkipReverseCard(string nextPlayer)
        {
            if (Center.Type == "draw2")
            {
                try
                {
                    for (int i = 0; i < 2; i++)
                    {
                        CallChatService.DealCard(nextPlayer, GameLogic.GetRandomCard(), InvitationCode);
                    }
                    RemoveUno(nextPlayer);
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
                }
                nextPlayer = SkipPlayer();
            }
            else if (Center.Type == "reverse")
            {
                nextPlayer = GameLogic.NextPlayer(Username, Players, !(Reverse));
                CallChatService.RequestChangeDirection(InvitationCode);
            }
            else if (Center.Type == "skip")
            {
                nextPlayer = SkipPlayer();
            }
            return nextPlayer;
        }

        private string PlayWildcard(string nextPlayer)
        {
            if (Center.Type == "draw4")
            {
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        CallChatService.DealCard(nextPlayer, GameLogic.GetRandomCard(), InvitationCode);
                    }
                    RemoveUno(nextPlayer);
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(Properties.Resources.temporalityInaviable, Properties.Resources.sorry);
                }
                nextPlayer = SkipPlayer();
            }
            return nextPlayer;
        }

        /// <summary>
        /// Dependiendo del tipo de carta, realiza la acción que esta indica.
        /// Recibe una cadena vacía si es una carta de color y una cadena diferente de vacía si es una carta comodín
        /// </summary>
        /// <param name="color"></param>
        /// <param name="imageCard"></param>
        private void PlayCard(string color, Image imageCard)
        {
            string nextPlayer = GameLogic.NextPlayer(Username, Players, Reverse);
            string newColor = "";
            //Carta que no es comodín
            if (color == NUMERIC_SKIP_REVERSE)
            {
                newColor = Center.Color;
                nextPlayer = PlayNumericSkipReverseCard(nextPlayer);
            }
            else
            {
                GrdColors.Visibility = Visibility.Hidden;
                nextPlayer = PlayWildcard(nextPlayer);
                newColor = color;
            }
            LvCards.Items.Remove(imageCard);
            CallChatService.PutCardInCenter(InvitationCode, Center);
            HasUno();
            if (!IsWinner())
            {
                CallChatService.NextTurn(InvitationCode, nextPlayer);
                CallChatService.SendTurnInformation(InvitationCode, newColor, nextPlayer);
            }
        }

        /// <summary>
        /// Determina si el jugador ya ganó la partida al deshacerse de todas sus cartas
        /// </summary>
        /// <returns> true si el jugador ya no tiene cartas en su mazo </returns>
        /// <returns> false en caso contrario </returns>
        private bool IsWinner()
        {
            bool isWinner = false;
            if (LvCards.Items.Count == 0)
            {
                isWinner = true;
                CallChatService.SendWinner(InvitationCode, Username);
            }
            return isWinner;
        }

        /// <summary>
        /// Determina si el jugador solo se ha quedado con 1 carta en su lista de cartas. Si solo tiene 1 pero no presionó el 
        /// botón "uno", se le agregan 2 cartas a su mazo
        /// </summary>
        /// <returns> true si solo tiene 1 carta en su mazo</returns>
        /// <returns> false en caso contrario </returns>
        private void HasUno()
        {
            if (LvCards.Items.Count == 1 && BtnUno.IsEnabled)
            {
                MessageBox.Show("No dijiste UNO, debes comer 2 cartas");
                for (int i = 0; i < 2; i++)
                {
                    AddCard(GameLogic.GetRandomCard());
                }
            }
        }

        /// <summary>
        /// Selecciona el color azul al jugar un comodín, manda a llamar al método playCard para que realice la lógica de la carta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBlue_Click(object sender, RoutedEventArgs e)
        {
            Image image = LvCards.SelectedValue as System.Windows.Controls.Image;
            PlayCard("blue", image);
        }

        /// <summary>
        /// Selecciona el color rojo al jugar un comodín, manda a llamar al método playCard para que realice la lógica de la carta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRed_Click(object sender, RoutedEventArgs e)
        {
            Image image = LvCards.SelectedValue as System.Windows.Controls.Image;
            PlayCard("red", image);
        }

        /// <summary>
        /// Selecciona el color verde al jugar un comodín, manda a llamar al método playCard para que realice la lógica de la carta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGreen_Click(object sender, RoutedEventArgs e)
        {
            Image image = LvCards.SelectedValue as System.Windows.Controls.Image;
            PlayCard("green", image);
        }

        /// <summary>
        /// Selecciona el color amarillo al jugar un comodín, manda a llamar al método playCard para que realice la lógica de la carta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnYellow_Click(object sender, RoutedEventArgs e)
        {
            Image image = LvCards.SelectedValue as System.Windows.Controls.Image;
            PlayCard("yellow", image);
        }

        /// <summary>
        /// Toma una carta de la pila para robar. Manda al llamar al método AddCard para añadirla a la lista.
        /// Si esta carta coincide con el centro, el jugador puede guardarla 
        /// presionando el botón "paso". De lo contrario, se pasa el turno al siguiente jugador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStack_Click(object sender, RoutedEventArgs e)
        {
            Card card = GameLogic.GetRandomCard();
            AddCard(card);
            if (!GameLogic.IsValidCard(card, Center, ActualColor))
            {
                CallChatService.NextTurn(InvitationCode, GameLogic.NextPlayer(Username, Players, Reverse));
            }
            else
            {
                BtnPaso.Visibility = Visibility.Visible;
            }
            BtnStack.IsEnabled = false;
            RemoveUno(Username);
        }

        /// <summary>
        /// Agrega una carta a la lista de cartas del jugador
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            System.Windows.Controls.Image image = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(card.Url, UriKind.Relative);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;
            image.Name = card.Id;
            LvCards.Items.Add(image);
        }

        /// <summary>
        /// Pasa el turno al siguiente jugador. Se habilita solo si antes se ha tomado una carta de la pila para robar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPaso_Click(object sender, RoutedEventArgs e)
        {
            if (BtnStack.IsEnabled)
            {
                MessageBox.Show(Properties.Resources.warningSkip);
            }
            else
            {
                CallChatService.NextTurn(InvitationCode, GameLogic.NextPlayer(Username, Players, Reverse));
            }
        }

        /// <summary>
        /// Muestra un mensaje con el ganador de la partida.
        /// </summary>
        /// <param name="usernameWinner"></param>
        public void ShowWinner(string usernameWinner)
        {
            MessageBox.Show("The winner is: " + usernameWinner);
        }
        /// <summary>
        /// Avisa que el jugador actual ya tiene solo una carta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUno_Click(object sender, RoutedEventArgs e)
        {
            if (LvCards.Items.Count == 2)
            {
                lblUnoPlayer1.Visibility = Visibility.Visible;
                BtnUno.IsEnabled = false;
                CallChatService.SendPlayerUno(InvitationCode, Username, true);
            }
        }

        public void PlayerSaidUno(string username)
        {
            if (lblPlayer2.Content.ToString() == username)
            {
                lblUnoPlayer2.Visibility = Visibility.Visible;
            }
            else if (lblPlayer3.Content.ToString() == username)
            {
                lblUnoPlayer3.Visibility = Visibility.Visible;
            }
            else if (lblPlayer4.Content.ToString() == username)
            {
                lblUnoPlayer4.Visibility = Visibility.Visible;
            }
        }


        public void PlayerWithoutUno(string username)
        {
            if (username == Username)
            {
                lblUnoPlayer1.Visibility = Visibility.Hidden;
                BtnUno.IsEnabled = true;
            }
            else if (lblPlayer2.Content.ToString() == username)
            {
                lblUnoPlayer2.Visibility = Visibility.Hidden;
            }
            else if (lblPlayer3.Content.ToString() == username)
            {
                lblUnoPlayer3.Visibility = Visibility.Hidden;
            }
            else if (lblPlayer4.Content.ToString() == username)
            {
                lblUnoPlayer4.Visibility = Visibility.Hidden;
            }
        }

        public void PlayerLeftGame(string username)
        {
            if (Players.Length == 1)
            {
                CallChatService.SendWinner(InvitationCode, Username);
            }
            if (username == Username)
            {
                lblUnoPlayer1.Content = username + " left the game";
                lblUnoPlayer1.Visibility = Visibility.Visible;
                lblPlayer1.Visibility = Visibility.Hidden;
            }
            else if (lblPlayer2.Content.ToString() == username)
            {
                lblUnoPlayer2.Content = username + " left the game";
                lblUnoPlayer2.Visibility = Visibility.Visible;
                lblPlayer2.Visibility = Visibility.Hidden;
            }
            else if (lblPlayer3.Content.ToString() == username)
            {
                lblUnoPlayer3.Content = username + " left the game";
                lblUnoPlayer3.Visibility = Visibility.Visible;
                lblPlayer3.Visibility = Visibility.Hidden;
            }
            else if (lblPlayer4.Content.ToString() == username)
            {
                lblUnoPlayer4.Content = username + " left the game";
                lblUnoPlayer4.Visibility = Visibility.Visible;
                lblPlayer4.Visibility = Visibility.Hidden;
            }
        }

        public void RemoveUno(string username)
        {
            lblUnoPlayer1.Visibility = Visibility.Hidden;
            BtnUno.IsEnabled = true;
            CallChatService.SendPlayerUno(InvitationCode, username, false);
        }

        private void LvCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallChatService.ValidateConnection(InvitationCode);
        }
    }
}