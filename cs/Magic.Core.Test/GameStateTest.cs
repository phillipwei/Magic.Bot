using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.Test
{
    [TestFixture]
    public class GameStateTest
    {
        [Test]
        public void DuplicateTest()
        {
            /*
            var players = new List<Player>() { new Player("A"), new Player("B")};
            var decks = new List<Deck>() { 
                Deck.LoadFromFile(@"Data\Decks\BoltsAndBearsTestDeck.txt"),
                Deck.LoadFromFile(@"Data\Decks\BoltsAndBearsTestDeck.txt")
            };
            var gs = new GameState(players, decks);
            var dup = gs.Duplicate() as GameState;

            Assert.AreEqual(gs.Libraries, dup.Libraries);
            */
        }
    }
}
