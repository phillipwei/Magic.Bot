using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Magic.Core
{
    public class ManaCost
    {
        public static readonly ManaCost None = new ManaCost(new List<ManaCostFragment>());
        public List<ManaCostFragment> Fragments;
        public int Converted { get { return Fragments.OfType<NumericManaCostFragment>().Sum(f => f.Amount); } }

        public ManaCost(List<ManaCostFragment> fragments)
        {
            this.Fragments = fragments;
        }

        private static Regex _parseRegex = new Regex(@"\{((?<variable>X)|(?<generic>[0-9]+)|(?<colored>[WUBRG])" + 
            @"|(?<hybrid>(?<color_a>[WUBRG])/(?<color_b>[WUBRG]))" +
            @"|(?<mono>2/(?<color>[WUBRG]))" +
            @"|(?<phyrexian>(?<color>[WUBRG])/P)" +
            @"|(?<snow>S)" +
            @")\}");

        public static ManaCost Parse(string s)
        {
            if(s == string.Empty)
            {
                return ManaCost.None;
            }

            var fragments = new List<ManaCostFragment>();

            foreach(Match match in _parseRegex.Matches(s))
            {
                if (match.Groups["variable"].Success)
                {
                    fragments.Add(new VariableManaCostFragment());
                }
                else if (match.Groups["generic"].Success)
                {
                    var amount = int.Parse(match.Groups["generic"].Value);
                    fragments.Add(new GenericManaCostFragment(amount));
                }
                else if (match.Groups["colored"].Success)
                {
                    var color = match.Groups["colored"].Value.ToColor();
                    var existingFragment = fragments.OfType<ColoredManaCostFragment>().FirstOrDefault(f => f.Color == color);
                    if (existingFragment != null)
                    {
                        existingFragment.Amount++;
                    }
                    else
                    {
                        fragments.Add(new ColoredManaCostFragment(color, 1));
                    }
                }
                else if (match.Groups["hybrid"].Success)
                {
                    var colorA = match.Groups["color_a"].Value.ToColor();
                    var colorB = match.Groups["color_b"].Value.ToColor();
                    var existingFragment = fragments.OfType<HybridManaCostFragment>().FirstOrDefault(f => f.ColorA == colorA && f.ColorB == colorB);
                    if (existingFragment != null)
                    {
                        existingFragment.Amount++;
                    }
                    else
                    {
                        fragments.Add(new HybridManaCostFragment(colorA, colorB, 1));
                    }
                }
                else if (match.Groups["mono"].Success)
                {
                    var color = match.Groups["color"].Value.ToColor();
                    var existingFragment = fragments.OfType<MonoColoredManaCostFragment>().FirstOrDefault(f => f.Color == color);
                    if (existingFragment != null)
                    {
                        existingFragment.Amount++;
                    }
                    else
                    {
                        fragments.Add(new MonoColoredManaCostFragment(color, 1));
                    }
                }
                else if (match.Groups["phyrexian"].Success)
                {
                    var color = match.Groups["color"].Value.ToColor();
                    var existingFragment = fragments.OfType<PhyrexianColoredManaCostFragment>().FirstOrDefault(f => f.Color == color);
                    if (existingFragment != null)
                    {
                        existingFragment.Amount++;
                    }
                    else
                    {
                        fragments.Add(new PhyrexianColoredManaCostFragment(color, 1));
                    }
                }
                else if (match.Groups["snow"].Success)
                {
                    var existingFragment = fragments.OfType<SnowColoredManaCostFragment>().FirstOrDefault();
                    if (existingFragment != null)
                    {
                        existingFragment.Amount++;
                    }
                    else
                    {
                        fragments.Add(new SnowColoredManaCostFragment(1));
                    }
                }
            }
            
            if (fragments.Count == 0)
            {
                throw new FormatException(string.Format("Cannot parse '{0}' as a ManaCost", s));
            }

            return new ManaCost(fragments);
        }

        public override string ToString()
        {
            return string.Concat(Fragments.Select(f => f.ToString()));
        }
    }
}
