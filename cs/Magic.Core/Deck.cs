using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Magic.Core
{
    public class Deck
    {
        public string Name;
        public IEnumerable<CardDefinition> Main;
        public IEnumerable<CardDefinition> Sideboard;

        private static Regex nameRegex = new Regex(@"([a-z])([A-Z])");
        private static Regex deckRegex = new Regex(@"^(Main)?(?<main>((?!Sideboard).)*)(Sideboard)?(?<sideboard>.*)$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex cardRegex = new Regex(@"^((?<count>[0-9]+) )?(?<name>.+)$", RegexOptions.Multiline);

        public static Deck LoadFromFile(string path)
        {
            var name = nameRegex.Replace(Path.GetFileNameWithoutExtension(path), "$1 $2");

            var match = deckRegex.Match(IO.Sanitize(File.ReadAllText(path, Encoding.UTF8)));
            var main = ParseSection(match.Groups["main"].Value);
            var sideboard = match.Groups["sideboard"].Success ? ParseSection(match.Groups["sideboard"].Value) : null;

            return new Deck(name, main, sideboard);
        }

        public static IEnumerable<CardDefinition> ParseSection(string s)
        {
            var list = new List<CardDefinition>();
            foreach(Match match in cardRegex.Matches(s))
            {
                int count = match.Groups["count"].Success ? int.Parse(match.Groups["count"].Value) : 1;
                var name = match.Groups["name"].Value.Trim();
                if (name == string.Empty) continue;
                var cardDef = CardDefinition.Get(name);
                if (list.Contains(cardDef))
                {
                    throw new FormatException(string.Format("{0} listed on multiple lines.", cardDef));
                }
                list.AddRange(Enumerable.Repeat(cardDef, count));
            }
            return list;
        }

        public Deck(IEnumerable<CardDefinition> main)
            : this(string.Empty, main, null)
        {
        }

        public Deck(string name, IEnumerable<CardDefinition> main, IEnumerable<CardDefinition> sideBoard)
        {
            this.Name = name;
            this.Main = main;
            this.Sideboard = sideBoard != null ? sideBoard : Enumerable.Empty<CardDefinition>();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string ToDisplayString()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine(new string('=', Formatting.MaxLineLength));
            sb.AppendLine("Main");
            sb.AppendLine(new string('-', Formatting.MaxLineLength));
            sb.Append(Main.ToDisplayString());
            sb.AppendLine(new string('=', Formatting.MaxLineLength));
            
            if(this.Sideboard.Count() != 0)
            {
                sb.AppendLine("Side Board");
                sb.AppendLine(new string('-', Formatting.MaxLineLength));
                sb.Append(Sideboard.ToDisplayString());
                sb.AppendLine(new string('=', Formatting.MaxLineLength));
            }
            
            return sb.ToString();
        }
    }
}
