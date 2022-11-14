using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public string Center { get; set; }
        public string ActualColor { get; set; }
        public bool Reverse { get; set; }
        public Game(string username, string code)
        {
            InitializeComponent();
            Username = username;
            InvitationCode = code;
            lblPlayer1.Content = Username;
            CallChatService = new Logic.CallChatService();
        }
        /*
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            CallChatService.NextTurn(InvitationCode, Username);
        }
        */
        public void PutCardOnCenter(string card)
        {
            this.Center = card;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("GraphicResources/" + card + ".png", UriKind.Relative);
            bi3.EndInit();
            imgCenter.Stretch = Stretch.Fill;
            imgCenter.Source = bi3;
        }


        public void DealFirstCards()
        {
            List<string> cards = new List<string>();
            cards = GameLogic.GetCards();
            int index = 0;
            int cardsSize = cards.Count;
            string card = "";
            Random random = new Random();
            for (int i = 0; i < Players.Length; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    index = random.Next(cardsSize);
                    card = cards.ElementAt(index);
                    CallChatService.DealCard(Players[i], card, InvitationCode);
                    cards.RemoveAt(index);
                    cardsSize--;   
                }
            }
        }
        public void PutUsernames(string[] players)
        {
            this.Players = players;
            int labelIterator = 2;
            int indexThisPlayer = 0;

            indexThisPlayer = GameLogic.GetIndexPlayer(Username, players);

            int iterador = indexThisPlayer + 1;
            /// Simula un comportamiento circular
            while(iterador%players.Length != indexThisPlayer)
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

        private void BtnUseCard_Click(object sender, RoutedEventArgs e)
        {
            string card = (string)LvCards.SelectedItem;
            if (card == null)
            {
                MessageBox.Show("Please select a card from the list");
            }
            else
            {
                if (GameLogic.IsValidCard(card, Center, ActualColor))
                {
                    Center = card;
                    if (card.Contains("wildcard") || card.Contains("draw4"))
                    {
                        MessageBox.Show("Elige un color");
                        GrdColors.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        PlayCard("");
                    }
                }
                else
                {
                    MessageBox.Show("Carta inválida");
                }
            }
        }

        private string SkipPlayer()
        {
            string auxiliar = GameLogic.NextPlayer(Username, Players, Reverse);
            string nextPlayer = GameLogic.NextPlayer(auxiliar, Players, Reverse);
            return nextPlayer;
        }

        public void IsReverse(bool isReverse)
        {
            Reverse = isReverse;
            if (isReverse)
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

        private void PlayCard(string color)
        {
            string nextPlayer = GameLogic.NextPlayer(Username, Players, Reverse);
            if (color == "")
            {
                if (Center.Contains("draw2"))
                {
                    CallChatService.DealCard(nextPlayer, GameLogic.GetRandomCard(), InvitationCode);
                    CallChatService.DealCard(nextPlayer, GameLogic.GetRandomCard(), InvitationCode);
                    nextPlayer = SkipPlayer();
                    MessageBox.Show("El siguiente jugador es: " + nextPlayer);
                }
                else if (Center.Contains("reverse"))
                {
                    nextPlayer = GameLogic.NextPlayer(Username, Players, !(Reverse));
                    //CallCharService.ChangeDirection
                }
                else if (Center.Contains("skip"))
                {
                    nextPlayer = SkipPlayer();
                    //saltar turno
                }
            }
            else
            {
                MessageBox.Show("Se eligió el color: " + color);
                GrdColors.Visibility = Visibility.Hidden;
                if (Center.Contains("wildcard"))
                {
                    // ya se eligió color
                    ActualColor = color;
                    // avisarle a los demás el nuevo color
                    //jugar comodín
                }
                else if (Center.Contains("draw4"))
                {
                    ActualColor = color;
                    //elegir color
                    //jugar +4
                }
                //CallChatService.ChangeColor(color, invitationCode);
            }
            CallChatService.PutCardInCenter(InvitationCode, Center);
            CallChatService.NextTurn(InvitationCode, nextPlayer);
        }

        private void BtnBlue_Click(object sender, RoutedEventArgs e)
        {
            PlayCard("blue");
        }

        private void BtnRed_Click(object sender, RoutedEventArgs e)
        {
            PlayCard("red");
        }

        private void BtnGreen_Click(object sender, RoutedEventArgs e)
        {
            PlayCard("green");
        }

        private void BtnYellow_Click(object sender, RoutedEventArgs e)
        {
            PlayCard("yellow");
        }

    }
    }
