using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class PhaseStep : Tuple<Phase, Step>
    {
        public Phase Phase { get { return this.Item1; } }
        public Step Step { get { return this.Item2; } }

        public PhaseStep(Phase p, Step s)
            : base(p, s)
        {
        }
    }
}
