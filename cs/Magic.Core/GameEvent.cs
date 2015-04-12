using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    /*
    public abstract class GameEvent
    {
        public string Description { get; set; }

        public override string ToString()
        {
            return this.Description;
        }

    }

    public class Notification : GameEvent
    {
    }

    public class CoinFlipped : Notification
    {
        public HeadsOrTails Outcome { get; private set; }

        public CoinFlipped(HeadsOrTails outcome)
        {
            this.Description = string.Format("A coin was flipped and it was {0}.", outcome);
        }
    }

    public class PlayerSelected : Notification
    {
        public Player Player { get; private set; }

        public PlayerSelected(Player player, string description)
        {
            this.Description = string.Format("{0} {1}.", player, description);
        }
    }

    public abstract class AwaitPlayerDecision : GameEvent
    {
        public Player Player { get; protected set; }

        public AwaitPlayerDecision(Player player)
        {
            this.Player = player;
        }
    }

    public class AwaitPlayerChoice : AwaitPlayerDecision
    {
        public IEnumerable<Choice> Choices { get; protected set; }

        public AwaitPlayerChoice(Player player, IEnumerable<Choice> choices)
            : base(player)
        {
            this.Choices = choices;
            this.Description = string.Format("{0} -- {1}", player, string.Join(",", choices.Select(c => c.ToString())));
        }
    }
    */
}
