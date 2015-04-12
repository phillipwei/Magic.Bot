using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public abstract class SetValueForPlayer<T, V> : SetToAction<V>
    {
        Player _player;

        public SetValueForPlayer(Player p, V to, Func<GameState, IList<T>> accessor, Func<T, V> get, Action<T, V> set)
            : base(to, (gs) => get(accessor(gs)[gs.Players.IndexOf(p)]), (gs, val) => set(accessor(gs)[gs.Players.IndexOf(p)], val))
        {
            this._player = p;
        }

        public override string ToString()
        {
            return string.Format("{0} for {1} to '{2}' (was '{3}')", GetType().Name, _player, To, From);
        }
    }
}
