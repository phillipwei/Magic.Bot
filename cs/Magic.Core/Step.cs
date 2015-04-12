using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public enum Step
    {
        None,
        Untap,
        Upkeep,
        Draw,
        BeginningofCombat,
        DeclareAttackers,
        DeclareBlockers,
        CombatDamage,
        EndOfCombat,
        End,
        Cleanup
    }
}
