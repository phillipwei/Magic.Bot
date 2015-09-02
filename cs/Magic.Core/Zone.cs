using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    // 400.1. A zone is a place where objects can be during a game. There are normally seven zones: library, hand, 
    // battlefield, graveyard, stack, exile, and command. Some older cards also use the ante zone. Each player has his 
    // or her own library, hand, and graveyard. The other zones are shared by all players.
    public abstract class Zone<T> where T : Object, IDuplicatable
    {
        public List<T> Objects { get; private set; }
        
        // 400.2. Public zones are zones in which all players can see the cards' faces, except for those cards that 
        // some rule or effect specifically allow to be face down. Graveyard, battlefield, stack, exile, ante, and 
        // command are public zones. Hidden zones are zones in which not all players can be expected to see the cards' 
        // faces. Library and hand are hidden zones, even if all the cards in one such zone happen to be revealed.
        public bool Public { get; private set; }

        public int Size { get { return Objects.Count; } }

        public Zone(bool isPublic = true, IEnumerable<T> objects = null)
        {
            this.Public = isPublic;
            this.Objects = new List<T>(objects != null ? objects : Enumerable.Empty<T>());
            foreach(var obj in Objects)
            {
                obj.Zone = this;
            }
        }

        public Zone(Zone<T> o)
        {
            this.Public = o.Public;
            this.Objects = o.Objects.Duplicate();
        }
    }
}
