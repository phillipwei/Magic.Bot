using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class Card : Object
    {
        public CardDefinition Definition { get; private set; }
        public bool FaceDown { get; set; }
        public override string Name { get { return Definition.Name; } }
        public override ManaCost ManaCost { get { return this.Definition.ManaCost; } }
        public override Color Color { get { throw new NotImplementedException(); } }
        public override Color ColorIndicator { get { throw new NotImplementedException(); } }
        public override List<CardType> CardTypes { get { return this.Definition.Types; } }
        public override List<SubType> SubTypes { get { return this.Definition.SubTypes; } }
        public override SuperType SuperType { get { return this.Definition.SuperType; } }
        public override string RulesText { get { return this.Definition.Text; } }
        public override List<Ability> Abilities { get { throw new NotImplementedException(); } }
        public override string Power { get { return this.Definition.Power; } }
        public override string Toughness { get { return this.Definition.Toughness; } }
        public override int Loyalty { get { return this.Definition.Loyalty; } }
        public bool IsLand { get { return CardTypes.Contains(CardType.Land); } }
        public bool IsCreature { get { return CardTypes.Contains(CardType.Creature);  } }
        public bool IsTargetting { get { return CardTypes.Exists(t => t == CardType.Instant || t == CardType.Sorcery) 
            && RulesText.IndexOf("target", StringComparison.CurrentCultureIgnoreCase) >= 0; } }

        public Card(Player owner, CardDefinition definition)
        {
            this.Owner = owner;
            this.Definition = definition;
            this.FaceDown = false;
        }

        public Card(Card o)
        {
            this.Definition = o.Definition;
            this.FaceDown = o.FaceDown;
        }

        public override string ToString()
        {
            return Name;
        }

        public override object Duplicate()
        {
            return new Card(this);
        }
    }
}
