using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Models
{
    public class Message
    {
        public string Value { get; set; }

        public string User { get; set; }

        public string Time { get; set; } = DateTime.Now.ToShortTimeString();
    }
}
