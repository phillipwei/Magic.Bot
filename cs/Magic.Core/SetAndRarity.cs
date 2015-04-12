using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class SetAndRarity : Tuple<Set, Rarity>
    {
        public Set Set { get { return this.Item1; }}
        public Rarity Rarity { get { return this.Item2; } }

        public SetAndRarity(Set set, Rarity rarity)
            : base(set, rarity)
        {
        }
    }
}
