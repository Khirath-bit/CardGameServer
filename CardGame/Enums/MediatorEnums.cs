using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Enums
{
    public enum ListClients
    {
        ListAllConnections,
        ListAllParticipants,
        ListAllSpectators
    }

    public enum Operations
    {
        AddMessage,
        SetGameType,
        SetMiddleCardsSwimming,
        SetPlayerCardsSwimming
    }
}
