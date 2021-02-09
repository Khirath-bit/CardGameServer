using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CardGame.Enums;
using CardGame.Extensions;
using CardGame.Models;
using Utility.MVVM;

namespace CardGame.Managers
{
    public static class CommandManager
    {
        /// <summary>
        /// Executes a command
        /// </summary>
        public static void ExecuteCommand(string cmd)
        {
            var segments = cmd.Split(':').ToList();

            if (!segments.Any())
                return;

            if (segments[0].EqualsIgnoreCase("list"))
                ExecuteListCommand(segments);

            if (segments[0].EqualsIgnoreCase("id"))
                ConnectionManager.ConnectionId = Guid.Parse(segments[1]);

            if (segments[0].EqualsIgnoreCase("message"))
                Mediator.NotifyEnumColleagues(Operations.AddMessage, new Message { User = segments[1], Value = segments[2] });

            if (segments[0].EqualsIgnoreCase("game"))
                ExecuteGameCommand(segments);

            if (segments[0].EqualsIgnoreCase("action"))
                ExecuteActionCommand(segments);

        }

        /// <summary>
        /// Executes actions commands
        /// </summary>
        public static void ExecuteActionCommand(List<string> segments)
        {
            segments.RemoveAt(0);

            if (segments.Count < 1)
                return;

            if (segments[0].EqualsIgnoreCase("swimming"))
            {
                segments.RemoveAt(0);

                if (segments.Count < 1)
                    return;

                if (segments[0].EqualsIgnoreCase("middlecards"))
                {
                    segments.RemoveAt(0);
                    if (segments.Count < 1)
                        return;
                    Mediator.NotifyEnumColleagues(Operations.SetMiddleCardsSwimming, string.Join(":", segments));
                }

                if (segments[0].EqualsIgnoreCase("playercards"))
                {
                    segments.RemoveAt(0);
                    if (segments.Count < 1)
                        return;
                    Mediator.NotifyEnumColleagues(Operations.SetPlayerCardsSwimming, string.Join(":", segments));
                }

                if (segments[0].EqualsIgnoreCase("start"))
                {
                    Mediator.NotifyEnumColleagues(Operations.StartGame, null);
                }
            }
        }

        /// <summary>
        /// Executes game commands
        /// </summary>
        public static void ExecuteGameCommand(List<string> segments)
        {
            segments.RemoveAt(0);

            if (segments.Count < 1)
                return;

            if (segments[0].EqualsIgnoreCase("type") && Enum.TryParse(segments[1], true, out GameType type))
                Mediator.NotifyEnumColleagues(Operations.SetGameType, type);
        }


        /// <summary>
        /// Executes listing commands
        /// </summary>
        /// <param name="segments"></param>
        public static void ExecuteListCommand(List<string> segments)
        {
            segments.RemoveAt(0);

            if (segments.Count < 2)
                return;

            if (segments[0].EqualsIgnoreCase("connections"))
                Mediator.NotifyEnumColleagues(ListClients.ListAllConnections, segments[1]);
        }
    }
}
