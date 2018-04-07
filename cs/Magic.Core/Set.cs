using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class Set
    {
        public static Set M11 = new Set("Magic 2011", "M11", null);
        public static Set M14 = new Set("Magic 2014", "M14", null);
        public static Set ReturnToRavnica = new Set("Return to Ravnica", "RTR", Block.ReturnToRavnica);
        public static Set GateCrash = new Set("Gatecrash", "GTC", Block.ReturnToRavnica);
        public static Set DragonsMaze = new Set("Dragon's Maze", "DGM", Block.ReturnToRavnica);
        public static Set Theros = new Set("Theros", "THS", Block.Theros);
        public static Set Kaladesh = new Set("Kaladesh", "KLD", Block.Kaladesh);

        public string Name { get; private set; }
        public string Code { get; private set; }
        public Block Block { get; private set; }
        public bool IsCore { get { return this.Block == null; } }

        public Set(string name, string code, Block block)
        {
            this.Name = name;
            this.Code = code;
            this.Block = block;
        }
    }
}
