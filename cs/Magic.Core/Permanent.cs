using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 110.1. A permanent is a card or token on the battlefield. A permanent remains on the battlefield indefinitely. 
    // A card or token becomes a permanent as it enters the battlefield and it stops being a permanent as it's moved 
    // to another zone by an effect or rule.
    public class Permanent : Object
    {
        public Status Status { get; set; }
        public Object Object { get; set; }
        public override string Name { get { return Object.Name; } }
        public override ManaCost ManaCost { get { return Object.ManaCost; } }
        public override Color Color { get { return Object.Color; } }
        public override Color ColorIndicator { get { return Object.ColorIndicator; } }
        public override List<CardType> CardTypes { get { return Object.CardTypes; } }
        public override List<SubType> SubTypes { get { return Object.SubTypes; } }
        public override SuperType SuperType { get { return Object.SuperType; } }
        public override string RulesText { get { return Object.RulesText; } }
        public override List<Ability> Abilities { get { return Object.Abilities; } }
        public override string Power { get { return Object.Power; } }
        public override string Toughness { get { return Object.Toughness; } }
        public override int Loyalty { get { return Object.Loyalty; } }
        public bool HasManaAbility { get { return Object.SuperType == SuperType.Basic; } }

        public Permanent(Card card)
        {
            this.Object = card;
        }

        public override object Duplicate()
        {
            throw new NotImplementedException();
        }
    }
}
