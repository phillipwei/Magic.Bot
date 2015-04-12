using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    // {400.1}
    public abstract class Zone<T> where T : Object, IDuplicatable
    {
        public bool Public { get; private set; }
        public List<T> Objects { get; private set; }
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
