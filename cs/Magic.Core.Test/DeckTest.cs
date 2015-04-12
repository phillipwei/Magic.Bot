using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Core;
using NUnit.Framework;
using System.IO;

namespace Magic.Core.Test
{
    [TestFixture]
    public class DeckTest
    {
        [Test]
        public static void LoadAndDisplayDecksTest()
        {
            string deckPath = @"Data\Decks";
            foreach (string path in Directory.EnumerateFiles(deckPath, "*.txt"))
            {
                var deck = Deck.LoadFromFile(path);
                Console.WriteLine(deck.ToDisplayString());
            }
        }
    }
}
