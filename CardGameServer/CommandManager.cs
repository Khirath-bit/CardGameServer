using CardGameServer.Extensions;
using CardGameServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameServer
{
    public static class CommandManager
    {
        public static bool Debug { get; set; }

        /// <summary>
        /// Executes a command
        /// </summary>
        public static void Execute(Guid client, string command)
        {
            if (Debug)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{ClientHandler.GetById(client).Name} sent {command}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            var segments = command.Split(' ').ToList();

            if (segments.Count < 1)
                return;

            if (segments[0].EqualsIgnoreCase("debug"))
                Debug = !Debug;

            if (segments[0].EqualsIgnoreCase("setname"))
                ClientHandler.SetName(client, segments[1]);

            if (segments[0].EqualsIgnoreCase("game"))
                ExecuteGameCommand(client, segments);

            if (segments[0].EqualsIgnoreCase("list"))
                ExecuteListCommand(client, segments);

            if (segments[0].EqualsIgnoreCase("id"))
                ClientHandler.SendMessage(client, $"id:{client}");

            if(segments[0].EqualsIgnoreCase("action"))
                ExecuteActionCommand(segments);

            var c = ClientHandler.GetById(client);

            if (segments[0].EqualsIgnoreCase("message"))
            {
                var msg = command.Remove(0, 8);
                ClientHandler.Broadcast($"message:{c.Name}:{msg}", client.ToString());
            }
        }

        /// <summary>
        /// Executes action commands
        /// </summary>
        /// <param name="commandSegments"></param>
        private static void ExecuteActionCommand(List<string> commandSegments)
        {
            commandSegments.RemoveAt(0);

            if(commandSegments.Count < 1)
                return;

            if (commandSegments[0].EqualsIgnoreCase("swimming"))
            {
                commandSegments.RemoveAt(0);

                if (commandSegments.Count < 1)
                    return;

                if(commandSegments[0].EqualsIgnoreCase("beginnerhand"))
                    GameManager.SwimmingGameManager.SetMiddleCards(commandSegments[1].EqualsIgnoreCase("left"));

            }
        }

        /// <summary>
        /// Executes game commands
        /// </summary>
        private static void ExecuteGameCommand(Guid client, List<string> commandSegments)
        {
            commandSegments.RemoveAt(0);

            if (commandSegments[0].EqualsIgnoreCase("start"))
            {
                Console.WriteLine($"{client} | {ClientHandler.GetName(client)} requested the game to start");
                GameManager.StartGame();
            }

            if (commandSegments[0].EqualsIgnoreCase("join"))
                GameManager.AddParticipant(client);

            if (commandSegments[0].EqualsIgnoreCase("spectate"))
                GameManager.AddSpectator(client);

            if (commandSegments[0].EqualsIgnoreCase("type"))
            {
                if (commandSegments.Count < 2)
                {
                    ClientHandler.SendMessage(client, $"The current game type is {GameManager.CurrentGameType}");
                    return;
                }

                if (!Enum.TryParse(commandSegments[1], true, out GameType type))
                {
                    ClientHandler.SendMessage(client, $"Game type {commandSegments[1]} does not exist. Possible values: [{GameType.Durak}], [{GameType.Schwimmen}]");
                    return;
                }

                GameManager.SetGameType(client, type);
            }
        }

        /// <summary>
        /// Executes the list commands
        /// </summary>
        private static void ExecuteListCommand(Guid client, List<string> commandSegments)
        {
            commandSegments.RemoveAt(0);

            if (commandSegments.Count < 1)
                return;

            if (commandSegments[0].EqualsIgnoreCase("connections"))
                ClientHandler.SendMessage(client, $"list:connections:{ClientHandler.ActiveClients.ToCommaSeparatedString()}");

            if (commandSegments[0].EqualsIgnoreCase("participants"))
                ClientHandler.SendMessage(client, $"list:participants:{GameManager.Participants.ToCommaSeparatedString()}");

            if (commandSegments[0].EqualsIgnoreCase("spectators"))
                ClientHandler.SendMessage(client, $"list:spectators:{GameManager.Spectators.ToCommaSeparatedString()}");
        }
    }
}
