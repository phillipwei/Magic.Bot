using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public class GameState : IDuplicatable
    {
        public StartState StartState;
        public List<Player> Players;
        public List<PlayerState> PlayerStates;
        
        public List<Library> Libraries;
        public List<Hand> Hands;
        public List<Graveyard> Graveyards;
        public List<Sideboard> Sideboards; 
        public Battlefield Battlefield;
        public Stack Stack;
        public Exile Exile;
        public Player Starting;
        public Player Active;
        public Player Priority;
        public int TurnNumber;
        public Phase Phase;
        public Step Step;
        public bool IsGameOver;
        public bool BeginningOfStep;

        public GameState(List<Player> players, List<Library> libraries, List<Sideboard> sideboards)
        {
            this.StartState = new StartState();
            this.Players = new List<Player>();
            this.PlayerStates = new List<PlayerState>();
            this.Hands = new List<Hand>();
            this.Libraries = new List<Library>();
            this.Graveyards = new List<Graveyard>();
            this.Battlefield = new Battlefield();
            this.Sideboards = new List<Sideboard>();
            this.Stack = new Stack();
            this.Exile = new Exile();
            this.TurnNumber = 0;
            this.Phase = Phase.Beginning;
            this.Step = Step.Untap;
            this.BeginningOfStep = true;
            players.ZipDo(libraries, sideboards, (p, l, s) =>
            {
                this.Players.Add(p);
                this.PlayerStates.Add(new PlayerState());
                this.Libraries.Add(l);
                this.Hands.Add(new Hand(p));
                this.Graveyards.Add(new Graveyard(p));
                this.Sideboards.Add(s);
            });
        }

        public GameState(GameState o)
        {
            this.StartState = o.StartState.Duplicate() as StartState;
            this.Players = o.Players;
            this.PlayerStates = o.PlayerStates.Duplicate();
            this.Libraries = o.Libraries.Duplicate();
            this.Hands = o.Hands.Duplicate();
            this.Graveyards = o.Graveyards.Duplicate();
            this.Battlefield = o.Battlefield.Duplicate() as Battlefield;
            this.Stack = o.Stack.Duplicate() as Stack;
            this.Exile = o.Exile.Duplicate() as Exile;
            this.Starting = o.Starting;
            this.Active = o.Active;
            this.Priority = o.Priority;
            this.TurnNumber = o.TurnNumber;
            this.Phase = o.Phase;
            this.Step = o.Step;
            this.IsGameOver = o.IsGameOver;
            this.BeginningOfStep = o.BeginningOfStep;
        }

        public object Duplicate()
        {
            return new GameState(this);
        }

        public GameState Apply(GameAction a)
        {
            var gs = Duplicate() as GameState;
            a.Apply(gs);
            return gs;
        }

        public IEnumerable<Player> PlayersFrom(Player p)
        {
            var startAt = Players.IndexOf(p);
            for(var i = 0; i < Players.Count; i++)
            {
                yield return Players[(startAt + i) % Players.Count];
            }
        }

        public Player Opponent(Player p)
        {
            return Players.IndexOf(p) == 0 ? Players[1] : Players[0];
        }

        public PhaseStep PhaseStep { get { return new PhaseStep(this.Phase, this.Step); } }
    }
}
