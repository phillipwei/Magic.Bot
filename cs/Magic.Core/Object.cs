using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 109.1. An object is an ability on the stack, a card, a copy of a card, a token, a spell, a permanent, or an emblem.
    public abstract class Object : ICharacteristics, IDuplicatable
    {
        public int Id { get; set; }

        public object Zone { get; set; }

        // 111.2. A spell's owner is the same as the owner of the card that represents it, unless it's a copy. In that case, 
        // the owner of the spell is the player under whose control it was put on the stack. A spell's controller is, by 
        // default, the player who put it on the stack. Every spell has a controller.
        public Player Owner { get; set; }
        
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
