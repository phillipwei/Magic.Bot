using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class PassPriority : SetAction<Player>
    {
        public override void Apply(GameState gs)
        {
            this.From = gs.Priority;
            this.To = gs.PlayersFrom(gs.Priority).Skip(1).First();
            gs.Priority = this.To;
        }

        public override void Reverse(GameState gs)
        {
            gs.Priority = this.From;
        }

        public override string ToString()
        {
            return string.Format("Passing priority from '{0}' to '{1}'", this.From, this.To);
        }
    }
}
