using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CardGameServer.DataObjects;
using CardGameServer.Extensions;

namespace CardGameServer.Managers
{
    public class CardManager
    {

        private Dictionary<string, int> _names = new Dictionary<string, int>()
        {
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "10", 10 },
            { "Bube", 10 },
            { "Dame", 10 },
            { "König", 10 },
            { "Ass", 11 },
        };

        private List<Card> Cards { get; set; }

        public CardManager(GameType gameType)
        {
            GenerateCards(gameType);
        }

        private void GenerateCards(GameType gameType)
        {
            Cards = new List<Card>();

            var cardAmount = gameType switch
            {
                GameType.Schwimmen => 32,
                GameType.Durak => 32,
                GameType.None => 1 //dividing and zero u know
            };

            var count = (int)cardAmount / 4;

            for (var i = _names.Count-1; i > _names.Count - 1-count; i--)
            {
                Cards.Add(new Card { CardSigns = CardSigns.Herz, Description = _names.Keys.ElementAt(i), Value = _names.Values.ElementAt(i), Img = "CardImages/" + (int.TryParse(_names.Keys.ElementAt(i), out int v) ? v + "H.jpg" : _names.Keys.ElementAt(i)[0] + "H.jpg") });
                Cards.Add(new Card { CardSigns = CardSigns.Karo, Description = _names.Keys.ElementAt(i), Value = _names.Values.ElementAt(i), Img = "CardImages/" + (int.TryParse(_names.Keys.ElementAt(i), out int v2) ? v2 + "D.jpg" : _names.Keys.ElementAt(i)[0] + "D.jpg") });
                Cards.Add(new Card { CardSigns = CardSigns.Kreuz, Description = _names.Keys.ElementAt(i), Value = _names.Values.ElementAt(i), Img = "CardImages/" + (int.TryParse(_names.Keys.ElementAt(i), out int v3) ? v3 + "C.jpg" : _names.Keys.ElementAt(i)[0] + "C.jpg") });
                Cards.Add(new Card { CardSigns = CardSigns.Pik, Description = _names.Keys.ElementAt(i), Value = _names.Values.ElementAt(i), Img = "CardImages/" + (int.TryParse(_names.Keys.ElementAt(i), out int v4) ? v4 + "S.jpg" : _names.Keys.ElementAt(i)[0] + "S.jpg") });
            }

            Cards.Shuffle();
        }


        public List<Card> GetCards(int count, bool show = false)
        {
            var ret = Cards.Take(count).ToList();
            ret.ForEach(p => p.Show = show);
            Cards.RemoveRange(0, count);
            return ret;
        }

        public void AddCards(List<Card> cards)
        {
            Cards.AddRange(cards);
        }
    }
}
