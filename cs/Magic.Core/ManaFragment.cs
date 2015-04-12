using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class ManaFragment
    {
        public ManaType ManaType { get; private set; }
        public ManaRestriction ManaRestriction { get; private set; }

        private ManaFragment(ManaType type, ManaRestriction restriction)
        {
            this.ManaType = type;
            this.ManaRestriction = restriction;
        }

        private static Dictionary<Tuple<ManaType, ManaRestriction>, ManaFragment> _dict = new Dictionary<Tuple<ManaType, ManaRestriction>, ManaFragment>();

        public static ManaFragment Get(ManaType type, ManaRestriction restriction = null)
        {
            var key = new Tuple<ManaType, ManaRestriction>(type, restriction);
            ManaFragment val;
            if(!_dict.TryGetValue(key, out val))
            {
                val = new ManaFragment(type, restriction);
                _dict[key] = val;
            }
            return val;
        }
    }
}
