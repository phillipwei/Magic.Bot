using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Notification : GameAction
    {
        string _desc;

        public Notification(string desc)
        {
            _desc = desc;
        }

        public override void Apply(GameState gs)
        { }

        public override void Reverse(GameState gs)
        { }

        public override string ToString()
        {
            return _desc;
        }
    }
}
