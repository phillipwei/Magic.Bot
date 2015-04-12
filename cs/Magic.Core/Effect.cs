using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Effect
    {
    }

    // {610}
    public class OneShotEffect : Effect
    {
    }

    public class DealDamage : OneShotEffect
    {
    }

    public class DestroyPermanent : OneShotEffect
    {
    }

    public class CreateToken : OneShotEffect
    {
    }

    public class MoveObject : OneShotEffect 
    { 
    }

    public class DelayTrigger : OneShotEffect
    {
    }

    // dealing damage, destroying a permanent, putting a token onto the battlefield, and moving an object from one zone to another
}
