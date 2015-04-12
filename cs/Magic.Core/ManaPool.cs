using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class ManaPool : IDuplicatable
    {
        public IList<ManaFragment> ManaFragments { get; set; }

        public ManaPool()
        {
            this.ManaFragments = new List<ManaFragment>();
        }

        public ManaPool(ManaPool other)
        {
            this.ManaFragments = new List<ManaFragment>();
        }

        public object Duplicate()
        {
            return new ManaPool(this);
        }

        public void Add(ManaType type, int amount = 1, ManaRestriction restriction = null)
        {
            for(int i=0; i<amount; i++)
            {
                this.ManaFragments.Add(ManaFragment.Get(type, restriction));
            }
        }

        public void Add(ManaFragment f)
        {
            this.ManaFragments.Add(f);
        }
    }
}
