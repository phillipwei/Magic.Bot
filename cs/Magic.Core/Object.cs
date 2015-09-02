using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 109.1. An object is an ability on the stack, a card, a copy of a card, a token, a spell, a permanent, or an 
    // emblem.
    public abstract class Object : ICharacteristics, IDuplicatable
    {
        public int Id { get; set; }

        // 400.1. A zone is a place where objects can be during a game. There are normally seven zones: library, hand, 
        // battlefield, graveyard, stack, exile, and command. Some older cards also use the ante zone. Each player has 
        // his or her own library, hand, and graveyard. The other zones are shared by all players.
        public object Zone { get; set; }

        // 111.2. A spell's owner is the same as the owner of the card that represents it, unless it's a copy. In that 
        // case, the owner of the spell is the player under whose control it was put on the stack. A spell's controller 
        // is, by default, the player who put it on the stack. Every spell has a controller.
        public Player Owner { get; set; }

        // 110.2. A permanent's owner is the same as the owner of the card that represents it (unless it's a token; see 
        // rule 110.5a). A permanent's controller is, by default, the player under whose control it entered the 
        // battlefield. Every permanent has a controller.
        public Player Controller { get; set; }
        
        public List<Counter> Counters { get; set; }

        // Characteristics
        public abstract string Name { get; }
        public abstract ManaCost ManaCost { get; }
        public abstract Color Color { get; }
        public abstract Color ColorIndicator { get; }
        public abstract List<CardType> CardTypes { get; }
        public abstract List<SubType> SubTypes { get; }
        public abstract SuperType SuperType { get; }
        public abstract string RulesText { get; }
        public abstract List<Ability> Abilities { get; }
        public abstract string Power { get; }
        public abstract string Toughness { get; }
        public abstract int Loyalty { get; }

        public abstract object Duplicate();

        public override bool Equals(object obj)
        {
            Object o = obj as Object;
            if (o == null) return false;
            return this.Id == o.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
