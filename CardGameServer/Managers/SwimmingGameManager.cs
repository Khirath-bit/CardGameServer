using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CardGameServer.DataObjects;
using Newtonsoft.Json;

namespace CardGameServer.Managers
{
    public class SwimmingGameManager
    {
        /// <summary>
        /// Contains the card manager
        /// </summary>
        private CardManager _cardManager;

        /// <summary>
        /// Creates a new instance of <see cref="SwimmingGameManager"/>
        /// </summary>
        public SwimmingGameManager()
        {
            
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
        /// Gets or sets the player that is allowed to do its turn
        /// </summary>
        public Guid CurrentTurn { get; set; }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start()
        {
            //Reset everything
            _cardManager = new CardManager(GameType.Schwimmen);
            PlayerCards = new Dictionary<Guid, List<Card>>();
            //RoundBeginner = Guid.Empty;
            CurrentTurn = Guid.Empty;
            MiddleCards = new List<Card>();

            //Update clients
            ClientHandler.Broadcast("action:swimming:start");

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

            if (!left)
            {
                cards = PlayerCards.First(f => f.Key == RoundBeginner).Value.Take(3).ToList();
                PlayerCards.First(f => f.Key == RoundBeginner).Value.RemoveRange(0, 3);
            }
            else
            {
                cards = PlayerCards.First(f => f.Key == RoundBeginner).Value.TakeLast(3).ToList();
                PlayerCards.First(f => f.Key == RoundBeginner).Value.RemoveRange(3, 3);
            }

            //Set middle cards and send them to the clients
            MiddleCards = cards;
            ClientHandler.SendMessage(RoundBeginner, $"action:swimming:playercards:{JsonConvert.SerializeObject(PlayerCards.First(f => f.Key == RoundBeginner).Value)}");
            ClientHandler.Broadcast($"action:swimming:middlecards:{JsonConvert.SerializeObject(MiddleCards)}");
            
            //Set next player
            var beginnerPlayer = GameManager.Participants.First(f => f.Id == RoundBeginner);
            var nextIndex = GameManager.Participants.IndexOf(beginnerPlayer) + 1;
            if (nextIndex >= GameManager.Participants.Count)
                nextIndex = 0;
            CurrentTurn = GameManager.Participants[nextIndex].Id;
            ClientHandler.SendMessage(CurrentTurn, "action:swimming:onturn");
            Thread.Sleep(50);
            ClientHandler.Broadcast($"action:swimming:infotext:{GameManager.Participants[nextIndex].Name} ist am Zug.");
        }
    }
}
