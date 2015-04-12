using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Exile : Zone<Card>, IDuplicatable
    {
        public Exile()
            : base()
        {
        }

        public Exile(Exile o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Exile(this);
        }
    }
}
