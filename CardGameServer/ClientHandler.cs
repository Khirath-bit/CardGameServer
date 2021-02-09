using CardGameServer.Extensions;
using CardGameServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGameServer
{
    public static class ClientHandler
    {
        public static List<Client> ActiveClients { get; set; } = new List<Client>();

        /// <summary>
        /// Broadcasts to all clients
        /// </summary>
        /// <param name="content"></param>
        public static void Broadcast(string content, string id = null)
        {
            if (!ActiveClients.Any())
                return;

            foreach (var client in ActiveClients)
            {
                if (Guid.TryParse(id, out var filter) && client.Id == filter)
                    continue;

                client.WorkingSocket.Send(Encoding.ASCII.GetBytes(content));
            }
        }

        /// <summary>
        /// Broadcasts a message to all clients in the chat
        /// </summary>
        public static void BroadcastServerMessage(string message)
        {
            if (!ActiveClients.Any())
                return;

            foreach (var client in ActiveClients)
            {
                
                client.WorkingSocket.Send(Encoding.ASCII.GetBytes($"message:SERVER:{message}"));
            }
        }

        /// <summary>
        /// Removes a client
        /// </summary>
        public static void Remove(Client client)
        {
            ActiveClients.Remove(client);

            Broadcast($"list:connections:{ActiveClients.ToCommaSeparatedString()}");
            Broadcast($"list:participants:{GameManager.Participants.ToCommaSeparatedString()}");
            Broadcast($"list:spectators:{GameManager.Spectators.ToCommaSeparatedString()}");

            if (GameManager.Participants.Count == 0)
                GameManager.CurrentGameType = GameType.None;
        }

        /// <summary>
        /// Adds a client
        /// </summary>
        public static void Add(Client client)
        {
            ActiveClients.Add(client);
            Console.WriteLine($"Incoming connection from {client.WorkingSocket.RemoteEndPoint} | Id: {client.Id}");
        }

        /// <summary>
        /// Sets the clients name
        /// </summary>
        public static void SetName(Guid id, string name)
        {
            if (!ActiveClients.Exists(a => a.Id == id))
                return;

            ActiveClients.First(f => f.Id == id).Name = name;

            Console.WriteLine($"{id} registered as {name}");

            Broadcast($"list:connections:{ActiveClients.ToCommaSeparatedString()}", id.ToString());
            Broadcast($"list:participants:{GameManager.Participants.ToCommaSeparatedString()}", id.ToString());
            Broadcast($"list:spectators:{GameManager.Spectators.ToCommaSeparatedString()}", id.ToString());
        }

        /// <summary>
        /// Gets the name or null
        /// </summary>
        public static string GetName(Guid id)
        {
            if (!ActiveClients.Exists(a => a.Id == id))
                return null;

            return ActiveClients.First(f => f.Id == id).Name;
        }

        /// <summary>
        /// Gets client by id
        /// </summary>
        public static Client GetById(Guid id)
        {
            return ActiveClients.FirstOrDefault(f => f.Id == id);
        }

        /// <summary>
        /// Sends message
        /// </summary>
        public static void SendMessage(Guid id, string message)
        {
            if (!ActiveClients.Exists(a => a.Id == id))
                return;

            var client = ActiveClients.First(a => a.Id == id);

            if(CommandManager.Debug)
                Console.WriteLine($"Send message {message} to {client.Name}");

            client.WorkingSocket.Send(Encoding.ASCII.GetBytes(message));
        }
    }
}
