using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 405.1. When a spell is cast, the physical card is put on the stack (see rule 601.2a). When an ability is 
    // activated or triggers, it goes on top of the stack without any card associated with it (see rules 602.2a and 
    // 603.3).
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
