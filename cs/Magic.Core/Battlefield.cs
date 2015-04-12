using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
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
