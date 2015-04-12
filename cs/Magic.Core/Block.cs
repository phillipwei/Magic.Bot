using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class Block
    {
        public static Block ReturnToRavnica = new Block("Return To Ravnica");
        public static Block Theros = new Block("Theros");

        public string Name { get; private set; }
        
        public Block(string name)
        {
            this.Name = name;
        }
    }
}
