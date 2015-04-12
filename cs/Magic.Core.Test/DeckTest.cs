using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Magic.Core.Test
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void LoadAndDisplayDecksTest()
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
