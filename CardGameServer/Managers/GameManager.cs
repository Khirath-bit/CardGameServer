using System;
using System.Collections.Generic;

namespace CardGameServer.Managers
{
    public static class GameManager
    {
        /// <summary>
        /// Gets or sets the current game object
        /// </summary>
        public static GameType CurrentGameType { get; set; }

        /// <summary>
        /// Gets or sets the current participants
        /// </summary>
        public static List<Client> Participants { get; set; } = new List<Client>();

        /// <summary>
        /// Gets or sets the spectators
        /// </summary>
        public static List<Client> Spectators { get; set; } = new List<Client>();

        /// <summary>
        /// Adds a spectator
        /// </summary>
        public static void AddSpectator(Client client)
        {
            if (Spectators.Contains(client))
                return;

            if (Participants.Contains(client))
            {
                Participants.Remove(client);
                Console.WriteLine($"Removed {client.Id}|{client.Name} from the game of {CurrentGameType} as Participant");
            }
            
            Spectators.Add(client);

            Console.WriteLine($"Spectator {client.Id}|{client.Name} added to the game of {CurrentGameType}");
        }

        /// <summary>
        /// Adds a spectator
        /// </summary>
        public static void AddSpectator(Guid id)
        {
            var client = ClientHandler.GetById(id);

            if(client == null)
                return;

            if(Spectators.Contains(client))
                return;

            if (Participants.Contains(client))
            {
                Console.WriteLine($"Removed {client.Id}|{client.Name} from the game of {CurrentGameType} as Participant");
                Participants.Remove(client);
            }
            
            Spectators.Add(client);

            Console.WriteLine($"Spectator {client.Id}|{client.Name} added to the game of {CurrentGameType}");
        }

        /// <summary>
        /// Adds a participant
        /// </summary>
        public static void AddParticipant(Client client)
        {
            if (Participants.Contains(client))
                return;

            if (Spectators.Contains(client))
            {
                Console.WriteLine($"Removed {client.Id}|{client.Name} from the game of {CurrentGameType} as Spectator");
                Spectators.Remove(client);
            }
            
            Participants.Add(client);

            Console.WriteLine($"Participant {client.Id}|{client.Name} added to the game of {CurrentGameType}");
        }

        /// <summary>
        /// Adds a participant
        /// </summary>
        public static void AddParticipant(Guid id)
        {
            var client = ClientHandler.GetById(id);

            if (client == null)
                return;

            if (Participants.Contains(client))
                return;

            if (Spectators.Contains(client))
            {
                Console.WriteLine($"Removed {client.Id}|{client.Name} from the game of {CurrentGameType} as Spectator");
                Spectators.Remove(client);
            }

            Participants.Add(client);

            Console.WriteLine($"Participant {client.Id}|{client.Name} added to the game of {CurrentGameType}");
        }

        /// <summary>
        /// Sets the game type
        /// </summary>
        public static void SetGameType(Guid id, GameType type)
        {
            if(CurrentGameType == type)
                return;

            var client = ClientHandler.GetById(id);

            if(client == null)
                return;

            Console.WriteLine($"{client.Id} | {client.Name} changed the game type from {CurrentGameType} to {type}");

            CurrentGameType = type;
        }
    }
}
