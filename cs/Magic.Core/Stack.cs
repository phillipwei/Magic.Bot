using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Stack : Zone<Spell>, IDuplicatable
    {
        public Stack()
            : base()
        {
        }

        public Stack(Stack o)
            : base(o)
        {
        }

        public object Duplicate()
        {
            return new Stack(this);
        }

        public bool Empty { get { return this.Size == 0; } }
    }
}
