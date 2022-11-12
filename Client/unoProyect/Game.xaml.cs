using System;
using System.Collections.Generic;
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

        public void IsValidCard(string card)
        {
            Console.WriteLine("Centro: " + Center);
            Console.WriteLine("Carta: " + card);
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
                IsValidCard(card);
                if (GameLogic.IsValidCard(card, Center, ActualColor))
                {
                    PutCardOnCenter(card);
                    if (card.Contains("wildcard"))
                    {
                        // elegir color 
                        // avisarle a los demás el nuevo color
                        //jugar comodín
                    } 
                    else if (card.Contains("draw4"))
                    {
                        //elegir color
                        //jugar +4
                    }
                    else if (card.Contains("draw2"))
                    {
                        //jugar +2
                    }
                    else if (card.Contains("reverse")){
                        //CallCharService.ChangeDirection
                    }
                    else if (card.Contains("skip"))
                    {

                    }
                    //CallChatService.ChangeColor(color, invitationCode);
                    CallChatService.PutCardInCenter(InvitationCode, card);
                }
            }
        }
    }
}
