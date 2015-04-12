using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class PlayerState : IDuplicatable
    {
        public int LifeTotal { get; set; }
        public int PoisonCounters { get; set; }
        public ManaPool ManaPool { get; set; }

        public PlayerState()
        {
            this.ManaPool = new ManaPool();
        }

        public PlayerState(PlayerState o)
        {
            this.LifeTotal = o.LifeTotal;
            this.PoisonCounters = o.PoisonCounters;
            this.ManaPool = o.ManaPool.Duplicate() as ManaPool;
        }

        public object Duplicate()
        {
            return new PlayerState(this);
        }
    }
}
