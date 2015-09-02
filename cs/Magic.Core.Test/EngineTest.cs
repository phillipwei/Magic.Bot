using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core;
using Magic.Core.GameActions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magic.Core.Test
{
    [TestClass]
    public class EngineTest
    {
        public class RandomAgent : Agent
        {
            public Player Player ;

            public RandomAgent(Player player)
            {
                this.Player = player;
            }

            public override Choice Choose(GameState gs, params Choice[] choicesArray)
            {
                var choices = choicesArray.ToList();

                // Mulligan decisions: always keep
                if (choices.Exists(c => c == Choice.Keep))
                {
                    var landCount = gs.Hands[gs.IndexOf(Player)].Objects.Count(c => c.IsLand);
                    Console.WriteLine("Land Count = " + landCount);
                    if (landCount > 0 && landCount < 7)
                    {
                        return choices.Find(c => c == Choice.Keep);
                    }                    
                }
                
                // If you can play a land, do it.
                if (choices.Exists(c => c is PlayCardChoice))
                    return choices.First(c => c is PlayCardChoice);

                // If you can cast a creature, do it.
                if (choices.Exists(c => c is PlayCardChoice && (c as PlayCardChoice).Card.IsCreature))
                    return choices.First(c => c is PlayCardChoice && (c as PlayCardChoice).Card.IsCreature);

                // If you can't play a land, pass priority.
                if (!choices.Exists(c => c is PlayCardChoice && (c as PlayCardChoice).Card.IsLand) 
                    && choices.Exists(c => c == Choice.PassPriority))
                    return choices.First(c => c == Choice.PassPriority);
                
                return choicesArray[RNG.Next(choicesArray.Length)];
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
                var agents = new List<Agent>() { new RandomAgent(players[0]), new RandomAgent(players[1]) };
                var engine = new Engine(players, decks, agents, ga => Console.WriteLine(ga));

                var x = 0;
                while (!engine.GameOver() && x++ < 20)
                {
                    engine.Tick();
                    // Console.WriteLine(engine.States.Last().ToString());
                }
            }
        }
    }
}
