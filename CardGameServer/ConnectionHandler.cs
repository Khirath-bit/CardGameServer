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

        public void Listen()
        {
            try
            {
                _serverSocket.ReceiveTimeout = -1;
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 2222));
                _serverSocket.Listen(-1);
                Console.WriteLine("Start listening...");

                while (true)
                {
                    var client = _serverSocket.Accept();
                    Console.WriteLine($"Incoming connection from {client.RemoteEndPoint}");
                    ClientHandler.ActiveClients.Add(client);
                    ClientHandler.Broadcast("bla");
                    new System.Threading.Thread(() =>
                    {
                        try
                        {
                            ProcessClient(client);
                        }
                        catch (Exception ex)
                        {
                            ClientHandler.ActiveClients.Remove(client);
                            Console.WriteLine("Client connection processing error: " + ex.Message);
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

        private async void ProcessClient(Socket client)
        {
            Console.ReadLine();
            await Task.Run(() =>
            {
                while (true)
                {
                    var response = new byte[1024];
                    var received = client.Receive(response);
                    if (received == 0)
                    {
                        ClientHandler.ActiveClients.Remove(client);
                        Console.WriteLine("Client closed connection!");
                        return;
                    }

                    List<byte> respBytesList = new List<byte>(response);
                    respBytesList.RemoveRange(received, 1024 - received); // truncate zero end
                    Console.WriteLine("Client (" + client.RemoteEndPoint + "+: " + Encoding.ASCII.GetString(respBytesList.ToArray()));
                }
            });
        }
    }
}
