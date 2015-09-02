using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 404.1. A player's graveyard is his or her discard pile. Any object that's countered, discarded, destroyed, or 
    // sacrificed is put on top of its owner's graveyard, as is any instant or sorcery spell that's finished resolving. 
    // Each player's graveyard starts out empty.
    public class Graveyard : PlayerZone<Card>, IDuplicatable
    {
        public Graveyard(Player player)
            : base(player)
        {
        }

        public Graveyard(Graveyard o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Graveyard(this);
        }
    }
}
