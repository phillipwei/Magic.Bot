using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public abstract class PlayerGameAction : GameAction
    {
        public Player Player { get; private set; }

        public PlayerGameAction(Player p)
        {
            this.Player = p;
        }
    }
}
