using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 402.1. The hand is where a player holds cards that have been drawn. Cards can be put into a player's hand by 
    // other effects as well. At the beginning of the game, each player draws a number of cards equal to that player's 
    // starting hand size, normally seven. (See rule 103, "Starting the Game.")
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
