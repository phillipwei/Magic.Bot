using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core.GameActions;

namespace Magic.Core
{
    public abstract class GameAction
    {
        public abstract void Apply(GameState gs);
        public abstract void Reverse(GameState gs);
    }

    public class CoinFlip : Notification
    {
        public HeadsOrTails Outcome { get; private set; }

        public CoinFlip(HeadsOrTails outcome)
            : base(string.Format("Coin flipped = {0}", outcome))
        {
            this.Outcome = outcome;
        }
    }

    public class PlayerWinsFlip : CoinFlip
    {
        public Player Player { get; private set; }

        public PlayerWinsFlip(Player player, HeadsOrTails outcome)
            : base(outcome)
        {
            this.Player = player;
        }

        public override string ToString()
        {
            return string.Format("{0}; Player {1} wins", base.ToString(), this.Player);
        }
    }
    
    public class SetStartStateStatus : SetToAction<StartStateStatus>
    {
        public SetStartStateStatus(StartStateStatus to) : 
            base(to, (gs) => gs.StartState.Status, (gs,val) => gs.StartState.Status = val)
        { }
    }

    public class SetStartingPlayer : SetToAction<Player>
    {
        public SetStartingPlayer(Player to) :
            base(to, (gs) => gs.Starting, (gs, val) => gs.Starting = val) 
        { }
    }

    public class SetPriority : SetToAction<Player>
    {
        public SetPriority(Player to) :
            base(to, (gs) => gs.Priority, (gs, val) => gs.Priority = val) 
        { }
    }

    public class SetActive : SetToAction<Player>
    {
        public SetActive(Player to) :
            base(to, (gs) => gs.Active, (gs, val) => gs.Active = val)
        { }
    }
    
    public class SetPhase : SetToAction<Phase>
    {
        public SetPhase(Phase phase)
            : base(phase, gs => gs.Phase, (gs, val) => gs.Phase = val)
        { }
    }

    public class SetStep : SetToAction<Step>
    {
        public SetStep(Step step)
            : base(step, gs => gs.Step, (gs, val) => gs.Step = val)
        { }
    }

    public class SetBeginningOfStep : SetToAction<bool>
    {
        public SetBeginningOfStep(bool b)
            : base(b, gs => gs.BeginningOfStep, (gs, val) => gs.BeginningOfStep = val)
        { }
    }
}
