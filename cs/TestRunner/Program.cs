using Magic.Core.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core.Test;

namespace TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineTest.RandomPlayerEngineTest();
            //DeckTest.LoadAndDisplayDecksTest();
            //CardDefinitionTest.DisplayAllTest();
            //ManaCostTest.BorosReckonerTest();
            Console.ReadKey();
        }
    }
}
