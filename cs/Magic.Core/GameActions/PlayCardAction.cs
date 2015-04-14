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
            this.Card.Zone = gs.Battlefield;

            // 110.2. A permanent's owner is the same as the owner of the card that represents it (unless it's a token; 
            // see rule 110.5a). A permanent's controller is, by default, the player under whose control it entered the 
            // battlefield. Every permanent has a controller.
            this.Card.Controller = this.Card.Owner;
        }

        public override void Reverse(GameState gs)
        {
            this.Card.Zone = gs.Hands[gs.IndexOf(this.Card.Owner)];
            this.Card.Controller = null;
        }
    }
}
