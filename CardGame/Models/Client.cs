using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Models
{
    public class Client
    {
        /// <summary>
        /// Gets or sets the client name
        /// </summary>
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}
