using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Managers
{
    public static class ConnectionManager
    {
        /// <summary>
        /// Gets or sets the server
        /// </summary>
        public static Socket Server { get; set; }

        /// <summary>
        /// Gets or sets the connection id
        /// </summary>
        public static Guid ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the sending task
        /// </summary>
        private static Task SendingTask { get; set; }

        /// <summary>
        /// Connects this client to the game server
        /// </summary>
        public static bool Connect()
        {
            var serverEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2222);

            Server =
                new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {ReceiveTimeout = -1};

            // Connect to the server.
            try
            {
                Server.Connect(serverEp);
            }
            catch (Exception)
            {
                Console.WriteLine($"Establish connection with server ({serverEp}) failed!");
                return false;
            }

            Console.WriteLine($"Connection with server ({serverEp}) established!");

            SendCommand("id");

            Listen();

            return true;
        }

        /// <summary>
        /// Listens for server messages
        /// </summary>
        public static void Listen()
        {
            if(Server == null || !Server.Connected)
                return;

            const int maxMessageSize = 1024;
            byte[] response;
            int received;
            Task.Run(() =>
            {
                while (true)
                {
                    response = new byte[maxMessageSize];
                    received = Server.Receive(response);
                    if (received == 0)
                    {
                        Console.WriteLine("Server closed connection.");
                        return;
                    }

                    var respBytesList = new List<byte>(response);
                    respBytesList.RemoveRange(received, maxMessageSize - received); // truncate zero end
                    CommandManager.ExecuteCommand(Encoding.ASCII.GetString(respBytesList.ToArray()));
                }
            });
        }

        /// <summary>
        /// Send command
        /// </summary>
        public static void SendCommand(string command)
        {
            if (Server == null || !Server.Connected)
                return;

            try
            {
                Server.Send(Encoding.ASCII.GetBytes(command));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
