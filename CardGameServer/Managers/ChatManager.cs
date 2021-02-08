using System;
using System.Collections.Generic;
using System.Text;
using CardGameServer.DataObjects;

namespace CardGameServer.Managers
{
    public static class ChatManager
    {
        public static List<Message> Messages { get; set; } = new List<Message>();
    }
}
