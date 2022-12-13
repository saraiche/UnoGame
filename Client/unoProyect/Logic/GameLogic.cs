using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using unoProyect.Proxy;

namespace unoProyect.Logic
{
    static class GameLogic
    {
        private static List<Card> Cards = GetCards();
        public static List<Card> GetCards()
        {
            List<Card> cards = new List<Card>();
            List<string> colors = new List<string>();
            colors.Add("green");
            colors.Add("blue");
            colors.Add("red");
            colors.Add("yellow");
            foreach (string color in colors)
            {
                for (int i = 0; i < 10; i++)
                {
                    Card card = new Card();
                    card.Color = color;
                    card.Type = i.ToString();
                    card.Url = "GraphicResources/color_" + color + "_" + i + ".png";
                    card.Id = "color_" + color + "_" + i;
                    cards.Add(card);
                }
                Card cardDraw2 = new Card();
                cardDraw2.Color = color;
                cardDraw2.Type = "draw2";
                cardDraw2.Url = "GraphicResources/color_" + color + "_" + "draw2" + ".png";
                cardDraw2.Id = "color_" + color + "_" + "draw2";
                cards.Add(cardDraw2);
                Card cardReverse = new Card();
                cardReverse.Color = color;
                cardReverse.Type = "reverse";
                cardReverse.Url = "GraphicResources/color_" + color + "_" + "reverse" + ".png";
                cardReverse.Id = "color_" + color + "_" + "reverse";
                cards.Add(cardReverse);
                Card cardSkip = new Card();
                cardSkip.Color = color;
                cardSkip.Type = "skip";
                cardSkip.Url = "GraphicResources/color_" + color + "_" + "skip" + ".png";
                cardSkip.Id = "color_" + color + "_" + "skip";
                cards.Add(cardSkip);
            }
            Card cardWildcard = new Card();
            cardWildcard.Type = "wildcard";
            cardWildcard.Url = "GraphicResources/color_wildcard.png";
            cardWildcard.Id = "color_wildcard";
            cards.Add(cardWildcard);
            Card cardDraw4 = new Card();
            cardDraw4.Type = "draw4";
            cardDraw4.Url = "GraphicResources/color_draw4.png";
            cardDraw4.Id = "color_draw4";
            cards.Add(cardDraw4);
            return cards;
        }

        public static Card GetCardById(string idCard)
        {
            List<Card> cards = GetCards();
            Card card = new Card();
            int iterator = 0;
            while (iterator < cards.Count && cards[iterator].Id != idCard)
            {
                iterator++;
            }
            try
            {
                card = cards[iterator];
            }
            catch(IndexOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
            }
            return card;
        }

        public static Card GetRandomCard()
        {
            Random random = new Random();
            return Cards.ElementAt(random.Next(Cards.Count));
        }

        public static Card GetRandomCenter()
        {
            Card card;
            do
            {
                card = GetRandomCard();
            } while (card.Type.Contains("draw") || card.Type.Contains("reverse") || card.Type.Contains("skip") || card.Type.Contains("wildcard"));

            return card;
        }

        public static bool IsValidCard(Card card, Card center, string actualColor)
        {
            /// 1: es comodín?
            /// 2: es del mismo color? o había un comodín en el centro y estoy poniendo una carta del color que se había seleccionado?
            /// 3: es la misma carta aunque el color no sea el mismo?
            return (IsWildcard(card) || IsSameColor(card, actualColor) || IsSameType(card, center));
        }

        public static bool IsWildcard(Card card)
        {
            return (card.Type == "wildcard" || card.Type == "draw4");
        }

        public static bool IsSameColor(Card card, string actualColor)
        {
            bool isTheSame = false;

            if (card.Color == actualColor)
            {
                isTheSame = true;
            }

            return isTheSame;
        }

        public static bool IsSameType(Card card, Card center)
        {
            return (card.Type == center.Type);
        }

        public static int GetIndexPlayer(string actualPlayer, string[] players)
        {
            int indexPlayer = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].Equals(actualPlayer))
                {
                    indexPlayer = i;
                }
            }
            return indexPlayer;
        }

        public static string NextPlayer(string actualPlayer, string[] players, bool isReverse)
        {
            string nextPlayer = "";
            int indexActualPlayer = GetIndexPlayer(actualPlayer, players);
            int indexLastPlayer = players.Length - 1;
            if (isReverse)
            {
                if (indexActualPlayer == 0)
                {
                    nextPlayer = players[indexLastPlayer];
                }
                else
                {
                    nextPlayer = players[indexActualPlayer - 1];
                }
            }
            else
            {
                if (indexActualPlayer == indexLastPlayer)
                {
                    nextPlayer = players[0];
                }
                else
                {
                    nextPlayer=players[indexActualPlayer + 1];
                }
            }

            return nextPlayer;
        }
    }
}
