using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public abstract class SetToAction<T> : SetAction<T>
    {
        private Func<GameState, T> _get;
        private Action<GameState, T> _set;
        public T To { get; private set; }
        public T From { get; private set; }

        public SetToAction(T to, Func<GameState, T> get, Action<GameState, T> set)
        {
            this.To = to;
            this._get = get;
            this._set = set;
        }

        public override void Apply(GameState gs)
        {
            this.From = _get(gs);
            _set(gs, this.To);
        }

        public override void Reverse(GameState gs)
        {
            this._set(gs, this.From);
        }

        public override string ToString()
        {
            return string.Format("{0} to '{1}' (was '{2}')", GetType().Name, To, From);
        }
    }
}
