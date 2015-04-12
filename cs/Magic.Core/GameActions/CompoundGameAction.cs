using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class CompoundAction : GameAction
    {
        private const string Prefix = "-";

        string _desc;
        List<GameAction> _actions;

        public CompoundAction(string desc, params GameAction[] actions)
        {
            if (actions.Length == 0)
            {
                throw new ArgumentException("CompoundAction cannot be empty.");
            }
            _desc = desc;
            _actions = new List<GameAction>(actions);
        }

        public override void Apply(GameState game)
        {
            _actions.ForEach(a => a.Apply(game));
        }

        public override void Reverse(GameState game)
        {
            _actions.Reverse<GameAction>().ForEach(a => a.Reverse(game));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_desc);
            foreach (var a in _actions)
            {
                foreach (var s in a.ToString().SplitIntoLines())
                {
                    sb.Append(Prefix);
                    sb.AppendLine(s);
                }
            }
            sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            return sb.ToString();
        }
    }
}
