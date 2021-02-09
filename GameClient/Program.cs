using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameClient
{
    class Program
    {
        static void WorkWithServer(Socket server)
        {
            const int maxMessageSize = 1024;
            byte[] response;
            int received;
            Task.Run(() =>
            {
                while (true)
                {
                    response = new byte[maxMessageSize];
                    received = server.Receive(response);
                    if (received == 0)
                    {
                        Console.WriteLine("Server closed connection.");
                        return;
                    }

                    List<byte> respBytesList = new List<byte>(response);
                    respBytesList.RemoveRange(received, maxMessageSize - received); // truncate zero end
                    Console.WriteLine(Encoding.ASCII.GetString(respBytesList.ToArray()));
                }
            });

            while (true)
            {

                try
                {
                    server.Send(Encoding.ASCII.GetBytes(Console.ReadLine()));
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return;
                }
            }

        }

        static void Main(string[] args)
        {

            var serverEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2222);

            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ReceiveTimeout = -1;

            // Connect to the server.
            try
            {
                server.Connect(serverEp);
            }
            catch (Exception)
            {
                Console.WriteLine("Establish connection with server (" + serverEp + ") failed!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Connection with server (" + serverEp + ") established!");
            WorkWithServer(server);

            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }
    }
}
