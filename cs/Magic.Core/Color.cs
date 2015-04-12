using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    // {106.1.a}
    public enum Color
    {
        White,
        Blue,
        Black,
        Red,
        Green
    }

    public static class ColorExtensions
    {
        public static string ToSymbol(this Color color)
        {
            switch(color)
            {
                case Color.White: return "W";
                case Color.Blue: return "U";
                case Color.Black: return "B";
                case Color.Red: return "R";
                case Color.Green: return "G";
                default: throw new NotImplementedException();
            }
        }

        public static Color ToColor(this string s)
        {
            switch(s.ToUpper())
            {
                case "W": return Color.White;
                case "U": return Color.Blue;
                case "B": return Color.Black;
                case "R": return Color.Red;
                case "G": return Color.Green;
                default: throw new FormatException("ToColor()");
            }
        }
    }
}
