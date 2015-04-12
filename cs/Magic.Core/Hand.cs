using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Hand : PlayerZone<Card>, IDuplicatable
    {
        public Hand(Player player)
            : base(player, false)
        {
        }

        public Hand(Hand o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Hand(this);
        }
    }
}
