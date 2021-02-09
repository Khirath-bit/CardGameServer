using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGameServer.DataObjects;
using Newtonsoft.Json;

namespace CardGameServer.Managers
{
    public class SwimmingGameManager
    {
        /// <summary>
        /// Contains the card manager
        /// </summary>
        private readonly CardManager _cardManager;

        /// <summary>
        /// Creates a new instance of <see cref="SwimmingGameManager"/>
        /// </summary>
        public SwimmingGameManager()
        {
            _cardManager = new CardManager(GameType.Schwimmen);
        }

        /// <summary>
        /// Gets or sets the middle cards
        /// </summary>
        public List<Card> MiddleCards { get; set; }

        /// <summary>
        /// Gets or sets the player cards
        /// </summary>
        public Dictionary<Guid, List<Card>> PlayerCards { get; set; } = new Dictionary<Guid, List<Card>>();

        /// <summary>
        /// Gets or sets the round beginner
        /// </summary>
        public Guid RoundBeginner { get; set; }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start()
        {
            ClientHandler.BroadcastServerMessage($"Die Partie 'Schwimmen' hat begonnen");

            //Update round beginner
            var currentId = RoundBeginner == Guid.Empty ? 0 : GameManager.Participants.IndexOf(GameManager.Participants.First(f => f.Id == RoundBeginner));
            var nextId = GameManager.Participants.Count - 1 <= currentId ? 0 : currentId + 1;
            RoundBeginner = GameManager.Participants[nextId].Id;

            //Set players cards and send them to each one of them
            GameManager.Participants.ForEach(f => PlayerCards.Add(f.Id, _cardManager.GetCards(f.Id == RoundBeginner ? 6 : 3, true)));
            foreach (var (guid, cards) in PlayerCards)
                ClientHandler.SendMessage(guid,
                    $"action:swimming:playercards:{JsonConvert.SerializeObject(cards)}");


        }

        /// <summary>
        /// This occurs when the first player selected his main hand
        /// </summary>
        public void SetMiddleCards(bool left)
        {
            var cards = new List<Card>();

            if (left)
            {
                cards = PlayerCards.First(f => f.Key == RoundBeginner).Value.Take(3).ToList();
                PlayerCards.First(f => f.Key == RoundBeginner).Value.RemoveRange(0, 3);
            }
            else
            {
                cards = PlayerCards.First(f => f.Key == RoundBeginner).Value.Take(3).ToList();
                PlayerCards.First(f => f.Key == RoundBeginner).Value.RemoveRange(3, 3);
            }

            //Set middle cards and send them to the clients
            MiddleCards = cards;
            ClientHandler.Broadcast($"action:swimming:middlecards:{JsonConvert.SerializeObject(MiddleCards)}");
        }
    }
}
