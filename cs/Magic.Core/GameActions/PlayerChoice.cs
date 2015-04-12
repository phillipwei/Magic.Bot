using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class PlayerChoice : Notification
    {
        public Player Player { get; private set; }
        public Choice Choice { get; private set; }

        public PlayerChoice(Player p, Choice c)
            : base(string.Format("'{0}' chooses '{1}'", p, c))
        {
            this.Player = p;
            this.Choice = c;
        }
    }
}
