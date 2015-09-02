using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 111.1. A spell is a card on the stack. As the first step of being cast (see rule 601, "Casting Spells"), 
    // the card becomes a spell and is moved to the top of the stack from the zone it was in, which is usually 
    // its owner's hand. (See rule 405, "Stack.") A spell remains on the stack as a spell until it resolves 
    // (see rule 608, "Resolving Spells and Abilities"), is countered (see rule 701.5), or otherwise leaves 
    // the stack. For more information, see section 6, "Spells, Abilities, and Effects."
    public class Spell : Object
    {
        public Card Card;
        public override string Name { get { return Card.Name; } }
        public override ManaCost ManaCost { get { return Card.ManaCost; } }
        public override Color Color { get { return Card.Color; } }
        public override Color ColorIndicator { get { return Card.ColorIndicator; } }
        public override List<CardType> CardTypes { get { return Card.CardTypes; } }
        public override List<SubType> SubTypes { get { return Card.SubTypes; } }
        public override SuperType SuperType { get { return Card.SuperType; } }
        public override string RulesText { get { return Card.RulesText; } }
        public override List<Ability> Abilities { get { return Card.Abilities; } }
        public override string Power { get { return Card.Power; } }
        public override string Toughness { get { return Card.Toughness; } }
        public override int Loyalty { get { return Card.Loyalty; } }

        public Spell(Card c)
        {
            Card = c;
        }

        public override object Duplicate()
        {
            throw new NotImplementedException();
        }
    }
}
