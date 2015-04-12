using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Magic.Core
{
    public class OldCard
    {
        public string Name { get; set; }
        public string SetName { get; set; }

        private const int ExpectedColumns = 8;
        private const string SetPath = "Data\\Sets";
        private static List<OldCard> cards = null;
        private static Dictionary<string, List<OldCard>> cardsBySet = null;

        public OldCard(string set, string name)
        {
            this.SetName = set;
            this.Name = name;
        }

        public static bool Initialized
        {
            get
            {
                return cards != null;
            }
        }

        public static IEnumerable<OldCard> All
        {
            get
            {
                OldCard.Initialize();
                return cards;
            }
        }

        public static List<OldCard> LoadFromTSV(string path)
        {
            List<OldCard> cards = new List<OldCard>();
            string setName = new FileInfo(path).Name.Split(new char[] { '.' })[0];
            foreach (string line in File.ReadLines(path))
            {
                string[] splits = line.Split(new char[] { '\t' });
                if (splits.Length == ExpectedColumns)
                {
                    throw new FormatException(string.Format(
                        "Read {0} lines; expected {1}", splits.Length, ExpectedColumns));
                }

                cards.Add(new OldCard(set: setName, name: splits[0]));
            }

            return cards;
        }

        public static OldCard FindOne(string set, string nameRegex)
        {
            return cardsBySet[set].Where(c => Regex.IsMatch(c.Name, nameRegex)).First();
        }

        public static void Initialize(string path = null, bool reinitialize = false)
        {
            if (OldCard.Initialized && !reinitialize)
            {
                return;
            }

            path = (path == null) ? OldCard.SetPath : path;
            OldCard.cards = new List<OldCard>();
            OldCard.cardsBySet = new Dictionary<string, List<OldCard>>();
            foreach (string setPath in Directory.EnumerateFiles(path))
            {
                List<OldCard> cards = LoadFromTSV(setPath);
                if (cards.Count == 0)
                {
                    throw new FormatException(string.Format(
                        "{0} was empty", setPath));
                }

                OldCard.cards.AddRange(cards);
                OldCard.cardsBySet[cards[0].SetName] = cards;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Name, this.SetName);
        }
    }
}