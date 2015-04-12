using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
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
