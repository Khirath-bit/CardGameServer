using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGameServer.Extensions
{
    public static class ConnectionExtensions
    {
        public static string ToCommaSeparatedString(this List<Client> clients)
        {
            if (!clients.Any())
                return "";

            var list = clients.Aggregate("", (current, c) => current + (c.Name + "{"+c.Id+","));

            return list.Remove(list.Length - 1);
        }
    }
}
