using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public abstract class Token : Object
    {
        public override object Duplicate()
        {
            throw new NotImplementedException();
        }
    }
}
