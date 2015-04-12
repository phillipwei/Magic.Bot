using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Sideboard : PlayerZone<Card>, IDuplicatable
    {
        public Sideboard(Player player, IEnumerable<Card> cards)
            : base(player, false, cards)
        {
        }

        public Sideboard(Sideboard o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Sideboard(this);
        }
    }
}
