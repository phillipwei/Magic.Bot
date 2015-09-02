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

        // 601.2. To cast a spell is to take it from where it is (usually the hand), put it on the stack, and pay its 
        // costs, so that it will eventually resolve and have its effect. Casting a spell follows the steps listed 
        // below, in order. If, at any point during the casting of a spell, a player is unable to comply with any of 
        // the steps listed below, the casting of the spell is illegal; the game returns to the moment before that 
        // spell started to be cast (see rule 717, "Handling Illegal Actions"). Announcements and payments can't be 
        // altered after they've been made.

        // 601.2a The player announces that he or she is casting the spell. That card (or that copy of a card) moves 
        // from where it is to the stack. It becomes the topmost object on the stack. It has all the characteristics of 
        // the card (or the copy of a card) associated with it, and that player becomes its controller. The spell 
        // remains on the stack until it's countered, it resolves, or an effect moves it elsewhere.

        // 601.2c The player announces his or her choice of an appropriate player, object, or zone for each target the 
        // spell requires. A spell may require some targets only if an alternative or additional cost (such as a 
        // buyback or kicker cost), or a particular mode, was chosen for it; otherwise, the spell is cast as though it 
        // did not require those targets. If the spell has a variable number of targets, the player announces how many 
        // targets he or she will choose before he or she announces those targets. The same target can't be chosen 
        // multiple times for any one instance of the word "target" on the spell. However, if the spell uses the word 
        // "target" in multiple places, the same object, player, or zone can be chosen once for each instance of the 
        // word "target" (as long as it fits the targeting criteria). If any effects say that an object or player must 
        // be chosen as a target, the player chooses targets so that he or she obeys the maximum possible number of 
        // such effects without violating any rules or effects that say that an object or player can't be chosen as a 
        // target. The chosen players, objects, and/or zones each become a target of that spell. (Any abilities that 
        // trigger when those players, objects, and/or zones become the target of a spell trigger at this point; 
        // they'll wait to be put on the stack until the spell has finished being cast.)
        // Example: If a spell says "Tap two target creatures," then the same creature can't be chosen twice; the spell 
        // requires two different legal targets. A spell that says "Destroy target artifact and target land," however, 
        // can target the same artifact land twice because it uses the word "target" in multiple places.

        // 601.2f If the total cost includes a mana payment, the player then has a chance to activate mana abilities 
        // (see rule 605, "Mana Abilities"). Mana abilities must be activated before costs are paid.

        // 601.2h Once the steps described in 601.2a-g are completed, the spell becomes cast. Any abilities that 
        // trigger when a spell is cast or put onto the stack trigger at this time. If the spell's controller had 
        // priority before casting it, he or she gets priority.
        public override void Apply(GameState gs)
        {
            gs.Hands[gs.IndexOf(Player)].Objects.Remove(Card);
            gs.Stack.Objects.Add(new Spell(Card));

            this.Card.Zone = gs.Stack;
            this.Card.Controller = this.Card.Owner;
        }

        public override void Reverse(GameState gs)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("'{0}' Plays '{1}'", Player, Card);
        }
    }

    public class PlayLandAction : PlayCardAction
    {
        public PlayLandAction(Player p, Card c)
            : base(p, c)
        {
        }

        public override void Apply(GameState gs)
        {
            // 115.2a Playing a land is a special action. To play a land, a player puts that land onto the battlefield 
            // from the zone it was in (usually that player's hand). By default, a player can take this action only 
            // once during each of his or her turns. A player can take this action any time he or she has priority and 
            // the stack is empty during a main phase of his or her turn. See rule 305, "Lands."
            gs.Hands[gs.IndexOf(Player)].Objects.Remove(Card);
            gs.Battlefield.Objects.Add(new Permanent(Card));

            Card.Zone = gs.Battlefield;

            // 110.2. A permanent's owner is the same as the owner of the card that represents it (unless it's a token; 
            // see rule 110.5a). A permanent's controller is, by default, the player under whose control it entered the 
            // battlefield. Every permanent has a controller.
            Card.Controller = Card.Owner;
        }

        public override void Reverse(GameState gs)
        {
            this.Card.Zone = gs.Hands[gs.IndexOf(this.Card.Owner)];
            this.Card.Controller = null;
        }
    }
}
