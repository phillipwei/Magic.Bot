using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magic.Core.Test
{
    [TestClass]
    public class EngineTest
    {
        public class RandomAgent : Agent
        {
            public RandomAgent()
            {
            }

            public override Choice Choose(GameState gs, params Choice[] choices)
            {
                return choices[RNG.Next(choices.Length)];
            }

            public override IEnumerable<Choice> ChooseMany(GameState gs, params Choice[] choices)
            {
                var list = new List<Choice>();
                foreach(var choice in choices)
                {
                    if(RNG.Flip() == HeadsOrTails.Heads)
                    {
                        list.Add(choice);
                    }
                }
                return list;
            }

            public override void Acknowledge(GameState gs, GameAction a)
            { }
        }

        [TestMethod]
        public void RandomPlayerEngineTest()
        {
            int simulations = 1;
            for (var i = 0; i < simulations; i++)
            {
                var players = new List<Player>() { new Player("A"), new Player("B")};
                var decks = new List<Deck>() { 
                    Deck.LoadFromFile(@"Data\Decks\BoltsAndBearsTestDeck.txt"),
                    Deck.LoadFromFile(@"Data\Decks\BoltsAndBearsTestDeck.txt")
                };
                var agents = new List<Agent>() { new RandomAgent(), new RandomAgent() };
                var engine = new Engine(players, decks, agents, ga => Console.WriteLine(ga));

                while (true)
                {
                    throw new NotImplementedException();
                    engine.Tick();
                    Console.ReadKey();
                }
            }
        }
    }
}
