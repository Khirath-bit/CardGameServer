using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Enums
{
    public enum GameType
    {
        [Description("")]
        None,
        [Description("Schwimmen")]
        Schwimmen
    }

    public enum CardSigns
    {
        Kreuz,
        Pik,
        Herz,
        Karo
    }
}
