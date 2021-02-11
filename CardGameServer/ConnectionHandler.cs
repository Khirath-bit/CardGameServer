using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CardGameServer
{
    public class ConnectionHandler
    {
        private readonly Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// Listens for new clients
        /// </summary>
        public void Listen()
        {
            try
            {
                _serverSocket.ReceiveTimeout = -1;
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 2222));
                _serverSocket.Listen(-1);
                Console.WriteLine("Start listening to port 2222...");

                while (true)
                {
                    var clientSocket = _serverSocket.Accept();
                    var client = new Client
                    {
                        WorkingSocket = clientSocket
                    };

                    ClientHandler.Add(client);
                    new System.Threading.Thread(() =>
                    {
                        try
                        {
                            ProcessClient(client);
                        }
                        catch (Exception ex)
                        {
                            ClientHandler.Remove(client);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Client connection processing error: " + ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }).Start();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

        /// <summary>
        /// Processes the clients commands
        /// </summary>
        private async void ProcessClient(Client client)
        {
            await Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        var response = new byte[2048];
                        var received = client.WorkingSocket.Receive(response);
                        if (received == 0)
                        {
                            ClientHandler.Remove(client);
                            Console.WriteLine("Client closed connection!");
                            return;
                        }

                        var respBytesList = new List<byte>(response);
                        respBytesList.RemoveRange(received, 2048 - received); // truncate zero end
                        CommandManager.Execute(client.Id, Encoding.UTF8.GetString(respBytesList.ToArray()));
                    }
                }
                catch (Exception e)
                {
                    ClientHandler.Remove(client);
                    Console.WriteLine($"{client.Id} | {client.Name} closed the connection.");
                }
            });
        }
    }
}
