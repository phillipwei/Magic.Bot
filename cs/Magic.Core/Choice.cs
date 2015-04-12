using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class Choice
    {
        public static Choice PlayFirst = new Choice("play first");
        public static Choice PlayLast = new Choice("play last");
        public static Choice Mulligan = new Choice("mulligan");
        public static Choice Keep = new Choice("keep as starting");
        public static Choice PassPriority = new Choice("pass priority");

        public string Description { get; private set; }
        public Choice(string description)
        {
            this.Description = description;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }

    public class MultiChoice
    {
        public List<Choice> Choices;

        public MultiChoice(params Choice[] choices)
            : this(choices.AsEnumerable())
        {
        }

        public MultiChoice(IEnumerable<Choice> choices)
        {
            this.Choices = choices.ToList();
        }

        public override bool Equals(object obj)
        {
            MultiChoice o = obj as MultiChoice;
            if (o == null) return false;
            return Enumerable.SequenceEqual(this.Choices, o.Choices);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return Choices.Sum(c => c.GetHashCode());
            }
        }
    }

    public class Choice<T> : Choice
    {
        public T Item;

        public Choice(T item)
            : base(string.Format("{1}", item))
        {
            this.Item = item;
        }

        public override bool Equals(object obj)
        {
            Choice<T> o = obj as Choice<T>;
            if (o == null) return false;
            return this.Item.Equals(o.Item);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }

    }

    public class PlayCardChoice : Choice
    {
        public Player Player;
        public Card Card;

        public PlayCardChoice(Player p, Card c)
            : base(string.Format("'{0}' plays '{1}'", p, c))
        {
            this.Player = p;
            this.Card = c;
        }

        public override bool Equals(object obj)
        {
            PlayCardChoice o = obj as PlayCardChoice;
            if (o == null) return false;
            return this.Player.Equals(o.Player) &&
                this.Card.Equals(o.Card);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Player.GetHashCode();
                hash = hash * 23 + this.Card.GetHashCode();
                return hash;
            }
        }
    }
}
