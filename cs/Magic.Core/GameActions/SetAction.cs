using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public abstract class SetAction<T> : GameAction
    {
        public T To { get; protected set; }
        public T From { get; protected set; }
    }
}
