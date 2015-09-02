using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public abstract class Agent
    {
        public abstract Choice Choose(GameState gs, params Choice[] choicesArray);

        public abstract IEnumerable<Choice> ChooseMany(GameState gs, params Choice[] choices);
        
        public abstract void Acknowledge(GameState gs, GameAction a);
    }
}
