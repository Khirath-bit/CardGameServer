using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.Enums;
using CardGame.Views;

namespace CardGame.Managers
{
    public class GameManager
    {
        /// <summary>
        /// Gets or sets the game type controls
        /// </summary>
        public Dictionary<GameType, Type> GameTypeControls { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="GameManager"/>
        /// </summary>
        public GameManager()
        {
            GameTypeControls = new Dictionary<GameType, Type>
            {
                {GameType.None, null },
                {GameType.Schwimmen, typeof(SwimmingViewViewModel) },
                //{GameType.Durak, typeof(DurakViewViewModel) }
            };
        }

        /// <summary>
        /// Communicates the new game type to the server
        /// </summary>
        public void CommunicateGameType(GameType type)
        {
            ConnectionManager.SendCommand($"game type {type}");
        }
    }
}
