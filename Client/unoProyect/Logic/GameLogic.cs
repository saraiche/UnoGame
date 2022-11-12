using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace unoProyect.Logic
{
    class GameLogic
    {
        public static List<string> GetCards()
        {
            List<string> cards = new List<string>();
            List<string> colors = new List<string>();
            colors.Add("green");
            colors.Add("blue");
            colors.Add("red");
            colors.Add("yellow");
            foreach (string color in colors)
            {
                for (int i = 0; i < 10; i++)
                {
                    cards.Add("color_" + color + "_" + i.ToString());
                }
                cards.Add("color_" + color + "_draw2");
                cards.Add("color_" + color + "_reverse");
                cards.Add("color_" + color + "_skip");
            }
            cards.Add("color_wildcard");
            cards.Add("color_draw4");
            return cards;
        }

        public static string GetRandomCard()
        {
            Random random = new Random();
            List<string> cards = GetCards();
            string card = cards.ElementAt(random.Next(cards.Count)).ToString();
            return card;
        }


        public static string GetRandomCenter()
        {
            string card = "";
            do
            {
                card = GetRandomCard();
            } while (card.Contains("draw") || card.Contains("reverse") || card.Contains("skip") || card.Contains("wildcard"));

            return card;
        }
        public static bool IsValidCard(string card, string center, string actualColor)
        {
            bool isValid = false;
            
            string[] descomposeCard = card.Split('_');
            string[] descomposeCenter = center.Split('_');

            string colorCard = descomposeCard[1];
            string colorCenter = descomposeCenter[1];

            string numberCard = "card";
            string numberCenter = "center";
            if (descomposeCard.Length == 3)
            {
                numberCard = descomposeCard[2];
            }

            if (descomposeCenter.Length == 3)
            {
                numberCenter = descomposeCenter[2];
            }

            /// 1: es comodín?
            /// 2: es del mismo color?
            /// 3: había un comodín en el centro y estoy poniendo una carta del color que se había seleccionado?
            /// 4: es la misma carta aunque el color no sea el mismo?
            if (colorCard == "draw4" || colorCard == "wildcard" || 
                colorCenter == colorCard || 
                (colorCard == actualColor && (colorCenter == "draw4" || colorCenter == "wildcard")) ||
                numberCard == numberCenter)
            {
                isValid = true;
            }
            return isValid;
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
