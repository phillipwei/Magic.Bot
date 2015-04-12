using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class PlayCardAction : PlayerGameAction
    {
        public Card Card;

        public PlayCardAction(Player p, Card c)
            : base(p)
        {
            this.Card = c;
        }

        public override void Apply(GameState gs)
        {
            throw new NotImplementedException();
        }

        public override void Reverse(GameState gs)
        {
            throw new NotImplementedException();
        }
    }

    public class PlayLandAction : PlayCardAction
    {
        public PlayLandAction(Player p, Card c)
            : base(p, c)
        {
        }

        // todo: consider incrementing lands played counter as opposed to historical state lookup.
        public override void Apply(GameState gs)
        {

        }
    }
}
