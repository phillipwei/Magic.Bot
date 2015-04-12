using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.GameActions
{
    public class Untap : PlayerGameAction
    {
        private List<int> _indices;

        public Untap(Player p)
            : base(p)
        { }
        
        public override void Apply(GameState gs)
        {
            this._indices = gs.Battlefield.Objects.IndicesWhere(o => o.Controller == this.Player && o.Status.Tapped).ToList();
            gs.Battlefield.Objects.WithIndices(_indices).ForEach(o => o.Status.Tapped = false);
        }

        public override void Reverse(GameState gs)
        {
            gs.Battlefield.Objects.WithIndices(_indices).ForEach(o => o.Status.Tapped = true);
        }

        public override string ToString()
        {
            return string.Format("Untapping permanents for '{0}'", this.Player);
        }
    }
}
