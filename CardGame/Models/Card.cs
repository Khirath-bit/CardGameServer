using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.Enums;

namespace CardGame.Models
{
    public class Card
    {
        public CardSigns CardSigns { get; set; }

        public string Description { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return CardSigns + " " + Description;
        }

        public string Img { get; set; }

        public string ImgSrc
        {
            get
            {
                if (Show)
                    return Path.GetFullPath(Img);
                else
                    return Back;
            }
        }

        public string Back { get; set; } = Path.GetFullPath("CardImages/Back.jpg");

        public bool Show { get; set; } = false;
    }
}
