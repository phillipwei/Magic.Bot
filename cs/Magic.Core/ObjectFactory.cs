using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class ObjectFactory
    {
        public static ObjectFactory Instance = new ObjectFactory();

        private int _objIds = 0;

        public Card CreateCard(Player owner, CardDefinition def)
        {
            return new Card(owner, def) { Id = _objIds++ };
        }

        public Token CreateToken()
        {
            throw new NotImplementedException();
        }
    }
}
