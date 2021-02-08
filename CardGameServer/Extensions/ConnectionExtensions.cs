using System.Collections.Generic;
using System.Linq;
using CardGameServer.DataObjects;

namespace CardGameServer.Extensions
{
    public static class ConnectionExtensions
    {
        public static string ToCommaSeparatedString(this List<Client> clients)
        {
            if (!clients.Any())
                return "";

            var list = clients.Aggregate("", (current, c) => current + (c.Name + "{" + c.Id + ","));

            return list.Remove(list.Length - 1);
        }

        public static string ToCommaSeparatedString(this List<Message> message)
        {
            if (!message.Any())
                return "";

            var list = message.Aggregate("", (current, c) => current + (c.Value + ":" + c.User + ","));

            return list.Remove(list.Length - 1);
        }
    }
}
