using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public abstract class ManaCostFragment
    {
    }

    public class VariableManaCostFragment : ManaCostFragment
    {
        public override string ToString()
        {
            return "{X}";
        }
    }

    public abstract class NumericManaCostFragment : ManaCostFragment
    {
        public int Amount { get; set; }
        
        public NumericManaCostFragment(int amount)
        {
            this.Amount = amount;
        }
    }

    public class GenericManaCostFragment : NumericManaCostFragment
    {
        public GenericManaCostFragment(int amount)
            : base(amount)
        {
        }

        public override string ToString()
        {
            return "{" + this.Amount + "}";
        }
    }

    public class ColoredManaCostFragment : NumericManaCostFragment
    {
        public Color Color;

        public ColoredManaCostFragment(Color color, int amount)
            : base(amount)
        {
            this.Color = color;
        }

        public override string ToString()
        {
            return string.Concat(Enumerable.Repeat("{" + this.Color.ToSymbol() + "}", this.Amount));
        }
    }

    public class HybridManaCostFragment : NumericManaCostFragment
    {
        public Color ColorA;
        public Color ColorB;

        public HybridManaCostFragment(Color colorA, Color colorB, int amount)
            : base(amount)
        {
            this.ColorA = colorA;
            this.ColorB = colorB;
        }

        public override string ToString()
        {
            return string.Concat(Enumerable.Repeat("{" + this.ColorA.ToSymbol() + "/" + this.ColorB.ToSymbol() + "}", this.Amount));
        }
    }

    public class MonoColoredManaCostFragment : NumericManaCostFragment
    {
        public Color Color;

        public MonoColoredManaCostFragment(Color color, int amount)
            : base(amount)
        {
            this.Color = color;
        }

        public override string ToString()
        {
            return string.Concat(Enumerable.Repeat("{2/" + this.Color.ToSymbol() + "}", this.Amount));
        }
    }

    public class PhyrexianColoredManaCostFragment : NumericManaCostFragment
    {
        public Color Color;

        public PhyrexianColoredManaCostFragment(Color color, int amount)
            : base(amount)
        {
            this.Color = color;
        }

        public override string ToString()
        {
            return string.Concat(Enumerable.Repeat("{" + this.Color.ToSymbol() + "/P}", this.Amount));
        }
    }

    public class SnowColoredManaCostFragment : NumericManaCostFragment
    {
        public SnowColoredManaCostFragment(int amount)
            : base(amount)
        {
        }

        public override string ToString()
        {
            return string.Concat(Enumerable.Repeat("{S}", this.Amount));
        }
    }
}
