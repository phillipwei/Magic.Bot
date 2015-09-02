using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 403.1. Most of the area between the players represents the battlefield. The battlefield starts out empty. 
    // Permanents a player controls are normally kept in front of him or her on the battlefield, though there are some 
    // cases (such as an Aura attached to another player's permanent) when a permanent one player controls is kept 
    // closer to a different player.
    public class Battlefield : Zone<Permanent>, IDuplicatable
    {
        public Battlefield()
            : base()
        {
        }

        public Battlefield(Battlefield o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Battlefield(this);
        }
    }
}
