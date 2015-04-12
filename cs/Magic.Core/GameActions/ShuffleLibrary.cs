using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class ShuffleLibrary : GameAction
    {
        public Player Player { get; private set; }
        ReversibleShuffle _shuffle;

        public ShuffleLibrary(Player player)
        {
            Player = player;
        }

        public override void Apply(GameState gs)
        {
            _shuffle = new ReversibleShuffle(gs.Libraries[gs.Players.IndexOf(Player)].Size);
            _shuffle.Apply(gs.Libraries[gs.Players.IndexOf(Player)].Objects);
        }

        public override void Reverse(GameState gs)
        {
            _shuffle.Undo(gs.Libraries[gs.Players.IndexOf(Player)].Objects);
        }

        public override string ToString()
        {
            return string.Format("Shuffling {0}'s library", Player);
        }
    }
}
