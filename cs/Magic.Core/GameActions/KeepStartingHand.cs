using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class KeepStartingHand : PlayerGameAction
    {
        public KeepStartingHand(Player p) : base(p) { }

        public override void Apply(GameState gs)
        {
            gs.StartState.Keeps.Add(this.Player);
        }

        public override void Reverse(GameState gs)
        {
            gs.StartState.Keeps.Remove(this.Player);
        }

        public override string ToString()
        {
            return string.Format("'{0}' decides to keep", this.Player);
        }
    }
}
