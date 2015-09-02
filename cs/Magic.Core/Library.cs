using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 401.1. When a game begins, each player's deck becomes his or her library.
    public class Library : PlayerZone<Card>, IDuplicatable
    {
        public Library(Player player, IEnumerable<Card> cards)
            : base(player, false, cards)
        {
        }

        public Library(Library o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Library(this);
        }
    }
}
