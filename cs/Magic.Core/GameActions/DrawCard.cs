using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class DrawCard : PlayerGameAction
    {
        public DrawCard(Player p)
            : base(p)
        { }

        public override void Apply(GameState gs)
        {
            var index = gs.Players.IndexOf(this.Player);
            gs.Hands[index].Objects.Add(gs.Libraries[index].Objects[0]);
            gs.Libraries[index].Objects.RemoveAt(0);
        }

        public override void Reverse(GameState gs)
        {
            var index = gs.Players.IndexOf(this.Player);
            gs.Libraries[index].Objects.Insert(0, gs.Hands[index].Objects.Last());
            gs.Hands[index].Objects.RemoveAt(gs.Hands[index].Objects.Count - 1);
        }

        public override string ToString()
        {
            return string.Format("'{0}' draws a card", this.Player);
        }

        public static GameAction X(Player p, int x)
        {
            return new CompoundAction(
                string.Format("{0} draws {1}", p, x),
                Enumerable.Repeat<GameAction>(new DrawCard(p), x).ToArray()
            );
        }
    }
}
