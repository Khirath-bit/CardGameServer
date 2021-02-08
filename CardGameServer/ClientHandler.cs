using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CardGameServer
{
    public static class ClientHandler
    {
        public static List<Socket> ActiveClients { get; set; } = new List<Socket>();

        /// <summary>
        /// Broadcasts to all clients
        /// </summary>
        /// <param name="content"></param>
        public static void Broadcast(string content)
        {
            if(!ActiveClients.Any())
                return;

            foreach (var client in ActiveClients)
            {
                client.Send(Encoding.ASCII.GetBytes(content));
            }
        }
    }
}
