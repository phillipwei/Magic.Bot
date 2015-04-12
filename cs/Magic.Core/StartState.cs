using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class StartState : IDuplicatable
    {
        public StartStateStatus Status { get; set; }
        public HashSet<Player> Keeps { get; set; }

        public StartState()
        {
            this.Status = StartStateStatus.Unstarted;
            this.Keeps = new HashSet<Player>();
        }

        public StartState(StartState o)
        {
            this.Status = o.Status;
            this.Keeps = new HashSet<Player>(o.Keeps);
        }

        public object Duplicate()
        {
            return new StartState(this);
        }
    }
}
