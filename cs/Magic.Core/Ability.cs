using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Ability
    {
    }

    public class StackAbility : Object
    {
        public override string Name { get { throw new NotImplementedException(); } }
        public override ManaCost ManaCost { get { throw new NotImplementedException(); } }
        public override Color Color { get { throw new NotImplementedException(); } }
        public override Color ColorIndicator { get { throw new NotImplementedException(); } }
        public override List<CardType> CardTypes { get { throw new NotImplementedException(); } }
        public override List<SubType> SubTypes { get { throw new NotImplementedException(); } }
        public override SuperType SuperType { get { throw new NotImplementedException(); } }
        public override string RulesText { get { throw new NotImplementedException(); } }
        public override List<Ability> Abilities { get { throw new NotImplementedException(); } }
        public override string Power { get { throw new NotImplementedException(); } }
        public override string Toughness { get { throw new NotImplementedException(); } }
        public override int Loyalty { get { throw new NotImplementedException(); } }

        public override object Duplicate()
        {
            throw new NotImplementedException();
        }
    }
}
