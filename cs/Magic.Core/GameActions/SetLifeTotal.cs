using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class SetLifeTotal : SetValueForPlayer<PlayerState, int>
    {
        public SetLifeTotal(Player p, int to)
            : base(p, to, gs => gs.PlayerStates, ps => ps.LifeTotal, (ps, val) => ps.LifeTotal = val)
        { }
    }
}
