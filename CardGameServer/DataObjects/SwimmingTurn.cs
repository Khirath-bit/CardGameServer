using System.Collections.Generic;

namespace CardGameServer.DataObjects
{
    public class SwimmingTurn
    {
        /// <summary>
        /// Get or sets whether the player has skipped
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// Gets or sets whether the player has passed
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// Gets or sets the hand cards
        /// </summary>
        public List<Card> HandCards { get; set; }
    }
}
