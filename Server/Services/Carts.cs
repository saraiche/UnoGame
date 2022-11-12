using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class Carts
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
            foreach (string card in cards)
            {
                Console.WriteLine(card);
            }
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
    }
}
