using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using CardGameServer.Extensions;
using CardGameServer.Managers;

namespace CardGameServer
{
    public static class CommandManager
    {
        /// <summary>
        /// Executes a command
        /// </summary>
        public static void Execute(Guid client, string command)
        {
            var segments = command.Split(' ').ToList();

            if (segments.Count < 1)
                return;

            if(segments[0].EqualsIgnoreCase("setname"))
                ClientHandler.SetName(client, segments[1]);

            if(segments[0].EqualsIgnoreCase("game"))
                ExecuteGameCommand(client, segments);

            if(segments[0].EqualsIgnoreCase("list"))
                ExecuteListCommand(client, segments);

            if(segments[0].EqualsIgnoreCase("id"))
                ClientHandler.SendMessage(client, $"id:{client}");

        }

        /// <summary>
        /// Executes game commands
        /// </summary>
        private static void ExecuteGameCommand(Guid client, List<string> commandSegments)
        {
            commandSegments.RemoveAt(0);

            if(commandSegments[0].EqualsIgnoreCase("start"))
                Console.WriteLine($"{client} | {ClientHandler.GetName(client)} requested the game to start");

            if(commandSegments[0].EqualsIgnoreCase("join"))
                GameManager.AddParticipant(client);

            if(commandSegments[0].EqualsIgnoreCase("spectate"))
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

            if(commandSegments.Count < 1)
                return;

            if(commandSegments[0].EqualsIgnoreCase("connections"))
                ClientHandler.SendMessage(client, $"list:connections:{ClientHandler.ActiveClients.ToCommaSeparatedString()}");

            if (commandSegments[0].EqualsIgnoreCase("participants"))
                ClientHandler.SendMessage(client, $"list:participants:{GameManager.Participants.ToCommaSeparatedString()}");

            if (commandSegments[0].EqualsIgnoreCase("spectators"))
                ClientHandler.SendMessage(client, $"list:spectators:{GameManager.Spectators.ToCommaSeparatedString()}");
        }
    }
}
