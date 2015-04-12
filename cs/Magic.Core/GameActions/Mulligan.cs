using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class ReverseGameAction : GameAction
    {
        string _desc;
        GameAction _ga;

        public ReverseGameAction(string desc, GameAction ga)
        {
            this._desc = desc;
            this._ga = ga;
        }

        public override void Apply(GameState gs)
        {
            _ga.Reverse(gs);
        }

        public override void Reverse(GameState gs)
        {
            _ga.Apply(gs);
        }

        public override string ToString()
        {
            return _desc;
        }
    }

    public class Mulligan : CompoundAction
    {
        public Mulligan(Player p, int x)
            : base(
                string.Format("'{0}' mulligans to {1}", p, x),
                Enumerable.Repeat<GameAction>(new ReverseGameAction(string.Format("'{0}' undraws", p), new DrawCard(p)), x + 1)
                .Concat(new ShuffleLibrary(p).Yield<GameAction>())
                .Concat(DrawCard.X(p, x).Yield<GameAction>())
                .ToArray())
        { }
    }
}
