using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CardGameServer.DataObjects;
using CardGameServer.Extensions;
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
        /// Gets or sets the play who called pass
        /// </summary>
        public Guid CalledPass { get; set; }

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
            
            //Sets the next turn
            SetNextTurn();
        }

        /// <summary>
        /// Current players turn
        /// </summary>
        public void MakeTurn(Guid playerId, string jsonTurn)
        {
            if(CurrentTurn != playerId)
                return;

            try
            {
                var data = JsonConvert.DeserializeObject<SwimmingTurn>(jsonTurn);

                if (data.Pass)
                {
                    if (CalledPass != Guid.Empty)
                        CalledPass = playerId;

                    SetNextTurn();
                    return;
                }

                if (data.Skip)
                {
                    SetNextTurn();
                    return;
                }

                //Swap cards
                var diff = PlayerCards.First(w => w.Key == playerId).Value.Where(v => !data.HandCards.Contains(v)).ToList();
                MiddleCards.AddRange(diff);
                var middleDiff = MiddleCards.Where(w => 
                    data.HandCards.Exists(e => e.Show == w.Show 
                                               && e.Value == w.Value && e.CardSigns == w.CardSigns 
                                               && e.Description == w.Description)).ToList();
                middleDiff.ForEach(f => MiddleCards.Remove(f));
                PlayerCards[CurrentTurn] = data.HandCards;

                if (CheckForWin())
                {
                    return;
                }

                if(diff.Any())
                    ClientHandler.Broadcast($"action:swimming:middlecards:{JsonConvert.SerializeObject(MiddleCards)}");

                SetNextTurn();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Invalid json turn data {e}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        /// <summary>
        /// Gets if current player has won yet
        /// </summary>
        private bool CheckForWin()
        {
            var points = PlayerCards[CurrentTurn].CountValues();

            if (points == 33)
            {
                ClientHandler.Broadcast($"action:swimming:infotext:{GameManager.Participants.First(f => f.Id == CurrentTurn).Name} hat einen BLITZ!");
                return true;
            }
            else if (points == 31)
            {
                ClientHandler.Broadcast($"action:swimming:infotext:{GameManager.Participants.First(f => f.Id == CurrentTurn).Name} hat Knack!");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the next turn
        /// </summary>
        private void SetNextTurn()
        {
            //Set next player
            var beginnerPlayer = GameManager.Participants.First(f => f.Id == RoundBeginner);
            var nextIndex = GameManager.Participants.IndexOf(beginnerPlayer) + 1;
            if (nextIndex >= GameManager.Participants.Count)
                nextIndex = 0;
            CurrentTurn = GameManager.Participants[nextIndex].Id;

            if (CalledPass == CurrentTurn)
            {
                ClientHandler.Broadcast($"action:swimming:infotext:Das spiel ist vorbei.");
                return;
            }

            ClientHandler.SendMessage(CurrentTurn, "action:swimming:onturn");
            Thread.Sleep(100);
            ClientHandler.Broadcast($"action:swimming:infotext:{GameManager.Participants[nextIndex].Name} ist am Zug.");
        }
    }
}
