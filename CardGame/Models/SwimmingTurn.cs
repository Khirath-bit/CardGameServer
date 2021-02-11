using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Models
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
        public List<Card> HandCards { get; set; } = new List<Card>();
    }
}
