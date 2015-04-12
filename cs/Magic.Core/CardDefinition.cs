using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Magic.Core
{
    public class CardDefinition
    {
        public string Name { get; set; }
        public ManaCost ManaCost { get; set; }
        public SuperType SuperType { get; set; }
        public List<CardType> Types { get; set; }
        public List<SubType> SubTypes { get; set; }
        public List<SetAndRarity> SetsAndRarity { get; set; }
        public string Text { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public int Loyalty { get; set; }
        public CardDefinition SplitSide { get; set; }
        public bool Normal { get; set; }
        public CardDefinition FlipSide { get; set; }
        public CardDefinition TransformSide { get; set; }

        private static bool _initialized = false;
        private static Dictionary<string, CardDefinition> _cardsByName = new Dictionary<string,CardDefinition>();
        private static Dictionary<string, CardDefinition> _cardsBySet = new Dictionary<string,CardDefinition>();

        public static void Load(string setPath = @"Data\Sets", char seperator = '\t', char listSeperator = ',', string emptyToken = "-", bool reload = false)
        {
            if(_initialized && !reload)
            {
                return;
            }

            foreach (string path in Directory.EnumerateFiles(setPath, "*.tsv"))
            {
                var setName = new FileInfo(path).Name.Split(new char[] { '.' })[0];
                var set = typeof(Set).GetField(setName, BindingFlags.Public | BindingFlags.Static).GetValue(null);
                foreach (var def in IO.LoadFromFile<CardDefinition>(path, seperator, listSeperator, emptyToken))
                {
                    _cardsByName[def.Name.ToLower()] = def;
                }
            }

            _initialized = true;
        }

        public static CardDefinition Get(string name)
        {
            Load();
            CardDefinition val;
            if (!_cardsByName.TryGetValue(name.ToLower(), out val))
            {
                throw new KeyNotFoundException(string.Format("No card named '{0}'.", name));
            }
            return val;
        }

        public static IEnumerable<CardDefinition> GetAll()
        {
            Load();
            return _cardsByName.Values;
        }

        public override string ToString()
        {
            return Name;
        }

        public string DisplayString()
        {
            var sb = new StringBuilder();
            var widthA = Formatting.MaxLineLength / 2;
            var widthB = Formatting.MaxLineLength - widthA;

            // First Line
            sb.AppendLine(new string('=', Formatting.MaxLineLength));
            sb.AppendLine(string.Format("{0,-" + widthA + "}{1," + widthB + "}", Name, ManaCost));
            
            // Second Line
            sb.AppendLine(new string('-', Formatting.MaxLineLength));
            if (SuperType != SuperType.None) sb.Append(SuperType + " ");
            sb.Append(string.Join(" ", Types.Select(t => t.ToString())));
            if (SubTypes.Count != 0) sb.Append(" - " + string.Join(" ", SubTypes.Select(t => t.ToString())));
            sb.AppendLine();

            // Rules
            if (Text != string.Empty)
            {
                sb.AppendLine(new string('-', Formatting.MaxLineLength));
                foreach(var line in Text.Split(new char[] { '|' }))
                {
                    var words = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    sb.Append(words[0]);
                    var charCount = words[0].Length;
                    foreach(var word in words.Skip(1))
                    {
                        if (charCount + word.Length >= Formatting.MaxLineLength)
                        {
                            sb.AppendLine();
                            sb.Append(word);
                            charCount = word.Length;
                        }
                        else
                        {
                            sb.Append(' ');
                            sb.Append(word);
                            charCount += word.Length + 1;
                        }
                    }
                    sb.AppendLine();
                }
            }

            // Power / Toughness
            if (Power != string.Empty || Toughness != string.Empty)
            {
                sb.AppendLine(new string('-', Formatting.MaxLineLength));
                sb.AppendLine(string.Format("{0}/{1}", Power, Toughness));
            }

            // End
            sb.AppendLine(new string('=', Formatting.MaxLineLength));

            return sb.ToString();
        }
    }
}
