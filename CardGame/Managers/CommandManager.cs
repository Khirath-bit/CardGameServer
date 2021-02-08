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

            if(!segments.Any())
                return;

            if(segments[0].EqualsIgnoreCase("list"))
                ExecuteListCommand(segments);

            if (segments[0].EqualsIgnoreCase("id"))
                ConnectionManager.ConnectionId = Guid.Parse(segments[1]);

        }


        /// <summary>
        /// Executes listing commands
        /// </summary>
        /// <param name="segments"></param>
        public static void ExecuteListCommand(List<string> segments)
        {
            segments.RemoveAt(0);

            if(segments.Count < 2)
                return;

            if (segments[0].EqualsIgnoreCase("connections"))
                Mediator.NotifyEnumColleagues(ListClients.ListAllConnections, segments[1]);
        }
    }
}
