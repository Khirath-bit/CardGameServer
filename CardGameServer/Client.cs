using System;
using System.Net.Sockets;

namespace CardGameServer
{
    public class Client
    {
        /// <summary>
        /// Id of the client
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the working socket
        /// </summary>
        public Socket WorkingSocket { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
    }
}
