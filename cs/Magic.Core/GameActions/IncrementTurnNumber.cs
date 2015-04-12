using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class IncrementTurnNumber : SetAction<int>
    {
        public override void Apply(GameState gs)
        {
            this.From = gs.TurnNumber;
            this.To = gs.TurnNumber + 1;
            gs.TurnNumber = this.To;
        }

        public override void Reverse(GameState gs)
        {
            gs.TurnNumber = this.From;
        }

        public override string ToString()
        {
            return string.Format("Incrementing turn number from '{0}' to '{1}'", this.From, this.To);
        }
    }
}
