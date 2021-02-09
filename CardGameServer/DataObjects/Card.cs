using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CardGameServer.DataObjects
{
    public class Card
    {
        public CardSigns CardSigns { get; set; }

        public string Description { get; set; }

        public int Value { get; set; }

        public string Img { get; set; }

        public bool Show { get; set; }
    }
}
