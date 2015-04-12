using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public enum StartStateStatus
    {
        Unstarted,
        Shuffled,
        StartingPlayerDecided,
        LifeTotalsSet,
        InitialDraw,
        HandsKept
    }
}
