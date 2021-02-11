using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CardGameServer.DataObjects;

namespace CardGameServer.Extensions
{
    public static class CardExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static double CountValues(this List<Card> cards)
        {
            //3 equal
            if (cards.All(a => a.Description == cards.First().Description))
                return cards.First().Value == 11 ? 33 : 30.5;

        

            var vals = new Dictionary<CardSigns, int>();

            vals.Add(CardSigns.Herz, cards.Count(f => f.CardSigns == CardSigns.Herz));
            vals.Add(CardSigns.Karo, cards.Count(f => f.CardSigns == CardSigns.Karo));
            vals.Add(CardSigns.Pik, cards.Count(f => f.CardSigns == CardSigns.Pik));
            vals.Add(CardSigns.Kreuz, cards.Count(f => f.CardSigns == CardSigns.Kreuz));

            if (vals.Max(m => m.Value) == 1) //Wenn alles unterschiedlich ist dann gewinnt höchste karte
            {
                return cards.Max(m => m.Value);
            }

            var mostUsedSign = vals.First(f => f.Value == vals.Max(m => m.Value)).Key; //get most used card sign

            return cards.Where(w => w.CardSigns == mostUsedSign).Sum(s => s.Value); //sum values
        }
    }
}
