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

        public static int CountValues(this ObservableCollection<Card> cards)
        {
            var sum = 0;

            cards.ToList().ForEach(c => sum += c.Value);

            return sum;
        }
    }
}
