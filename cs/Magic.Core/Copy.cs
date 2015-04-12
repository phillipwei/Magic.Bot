using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    // 111.1a A copy of a spell is also a spell, even if it has no card associated with it. See rule 706.10.
    // 111.2. A spell's owner is the same as the owner of the card that represents it, unless it's a copy. 
    // In that case, the owner of the spell is the player under whose control it was put on the stack. 
    // A spell's controller is, by default, the player who put it on the stack. Every spell has a controller.
    public class Copy<T> where T : Object
    {
        public Player Owner { get; set; }
    }
}
