using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public abstract class PlayerZone<T> : Zone<T> where T : Object, IDuplicatable
    {
        public Player Player { get; private set; }
        
        public PlayerZone(Player player, bool isPublic = true, IEnumerable<T> objects = null)
            : base(isPublic, objects)
        {
            this.Player = player;
        }

        public PlayerZone(PlayerZone<T> o)
            : base(o)
        {
            this.Player = o.Player;
        }
    }
}
