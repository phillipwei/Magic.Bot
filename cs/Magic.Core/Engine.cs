using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.Core.Exceptions;
using Magic.Core.GameActions;

namespace Magic.Core
{
    public class Engine
    {
        public List<GameState> States = new List<GameState>();
        public List<GameAction> Actions = new List<GameAction>();
        public List<Agent> Agents = new List<Agent>();
        private Action<GameAction> _notify;
        private GameState _gs { get { return States.Last(); } }

        public Engine(List<Player> players, List<Deck> decks, List<Agent> agents, Action<GameAction> notify = null)
        {
            var libraries = players.Zip(decks, (p, d) => new Library(p, d.Main.Select(c => ObjectFactory.Instance.CreateCard(p, c)))).ToList();
            var sideboards = players.Zip(decks, (p, d) => new Sideboard(p, d.Sideboard.Select(c => ObjectFactory.Instance.CreateCard(p, c)))).ToList();
            this.States.Add(new GameState(players, libraries, sideboards));
            this.Agents = agents;
            this._notify = notify;
        }

        public bool GameOver()
        {
            return _gs.IsGameOver;
        }

        public void Play()
        {
            while (!_gs.IsGameOver)
            {
                Tick();
            }
        }

        public void Tick()
        {
            if (_gs.StartState.Status != StartStateStatus.HandsKept)
            {
                Step_Start();
            }
            else
            {
                if (_gs.BeginningOfStep)
                {
                    Step_BeginStep();
                }
                else if (_gs.Stack.Empty 
                    && Actions.Reverse<GameAction>().Take(_gs.Players.Count).All(ga => ga is PassPriority))
                {
                    Step_NextStep();
                }
                else
                {
                    var choices = new List<Choice>();
                    choices.AddRange(Choice_SpecialActions());
                    choices.AddRange(Choice_CastSpell());
                    choices.Add(Choice.PassPriority);
                    Console.WriteLine("Choices: [{0}]", string.Join(";", choices.Select(c => c.ToString())));
                    var choice = GetPlayerChoice(_gs.Priority, choices);

                    if(choice is PlayCardChoice)
                    {
                        var pcc = choice as PlayCardChoice;
                        if(pcc.Card.IsLand)
                        {
                            ApplyAction(new PlayLandAction(pcc.Player, pcc.Card));
                        }
                        else
                        {
                            // 601.Casting Spells
                            // 601.2a To propose the casting of a spell, a player first moves that card (or that copy 
                            // of a card) from where it is to the stack. It becomes the topmost object on the stack. 
                            // It has all the characteristics of the card (or the copy of a card) associated with it, 
                            // and that player becomes its controller. The spell remains on the stack until it’s 
                            // countered, it resolves, or an effect moves it elsewhere.


                            // 601.2b If the spell is modal, the player announces the mode choice (see rule 700.2). If 
                            // the player wishes to splice any cards onto the spell (see rule 702.46), he or she 
                            // reveals those cards in his or her hand. If the spell has alternative or additional costs 
                            // that will be paid as it’s being cast such as buyback or kicker costs (see rules 117.8 
                            // and 117.9), the player announces his or her intentions to pay any or all of those costs 
                            // (see rule 601.2f). A player can’t apply two alternative methods of casting or two 
                            // alternative costs to a single spell. If the spell has a variable cost that will be paid 
                            // as it’s being cast (such as an {X} in its mana cost; see rule 107.3), the player 
                            // announces the value of that variable. If a cost that will be paid as the spell is being 
                            // cast includes hybrid mana symbols, the player announces the nonhybrid equivalent cost he 
                            // or she intends to pay. If a cost that will be paid as the spell is being cast includes 
                            // Phyrexian mana symbols, the player announces whether he or she intends to pay 2 life or 
                            // the corresponding colored mana cost for each of those symbols. Previously made choices 
                            // (such as choosing to cast a spell with flashback from a graveyard or choosing to cast a 
                            // creature with morph face down) may restrict the player’s options when making these 
                            // choices.

                            // 601.2c The player announces his or her choice of an appropriate player, object, or zone 
                            // for each target the spell requires. A spell may require some targets only if an 
                            // alternative or additional cost (such as a buyback or kicker cost), or a particular mode, 
                            // was chosen for it; otherwise, the spell is cast as though it did not require those 
                            // targets. If the spell has a variable number of targets, the player announces how many 
                            // targets he or she will choose before he or she announces those targets. In some cases, 
                            // the number of targets will be defined by the spell’s text. Once the number of targets 
                            // the spell has is determined, that number doesn’t change, even if the information used to 
                            // determine the number of targets does. The same target can’t be chosen multiple times for 
                            // any one instance of the word “target” on the spell. However, if the spell uses the word 
                            // “target” in multiple places, the same object, player, or zone can be chosen once for 
                            // each instance of the word “target” (as long as it fits the targeting criteria). If any 
                            // effects say that an object or player must be chosen as a target, the player chooses 
                            // targets so that he or she obeys the maximum possible number of such effects without 
                            // violating any rules or effects that say that an object or player can’t be chosen as a 
                            // target. The chosen players, objects, and/or zones each become a target of that spell. 
                            // (Any abilities that trigger when those players, objects, and/or zones become the target 
                            // of a spell trigger at this point; they’ll wait to be put on the stack until the spell 
                            // has finished being cast.)
                            // Example: If a spell says “Tap two target creatures,” then the same creature can’t be chosen twice; the spell requires two different legal targets. A spell that says “Destroy target artifact and target land,” however, can target the same artifact land twice because it uses the word “target” in multiple places.
                            Choice target = null;
                            if (pcc.Card.TargetType == TargetType.CreatureOrPlayer)
                            {
                                var targetChoices = new List<Choice>() { Choice.AbortSpell };
                                targetChoices.AddRange(_gs.Players.Select(p => new TargetPlayerChoice(p)));
                                targetChoices.AddRange(_gs.Battlefield.Objects.Where(o => o.IsCreature).Select(o => new TargetCreatureChoice(o)));
                                target = GetPlayerChoice(_gs.Priority, targetChoices);
                                if (target == Choice.AbortSpell) return;
                            }
 
                            var castingChoices = new List<Choice>();
                            castingChoices.Add(Choice.AbortSpell);

                            // Mana Abilities
                            _gs.Battlefield
                                .Objects
                                .Where(o => o.Controller == pcc.Player && o.HasManaAbility && !o.Status.Tapped)
                                .ForEach(o => castingChoices.Add(new TapBasicLandChoice(o)));

                            // Pay Costs
                            // _gs.PlayerStates[_gs.IndexOf(pcc.Player)].ManaPool;

                            // pcc.Card.ManaCost
                            // GetPlayerChoice(_gs.Priority, targets);

                            ApplyAction(new PlayCardAction(pcc.Player, pcc.Card));
                        }
                    }
                    else if (choice == Choice.PassPriority)
                    {
                        ApplyAction(new PassPriority());
                    }
                }
            }
        }

        public void Undo()
        {
            this.States.RemoveAt(this.States.Count - 1);
            this.Actions.RemoveAt(this.Actions.Count - 1);
        }

        private GameState ThisTurnStartingState()
        {
            return States.Reverse<GameState>().TakeWhile(ga => ga.TurnNumber == _gs.TurnNumber).Last();
        }

        private IEnumerable<GameState> ThisTurnStates()
        {
            return States.Reverse<GameState>().TakeWhile(ga => ga.TurnNumber == _gs.TurnNumber).Reverse();
        }

        private Choice GetPlayerChoice(Player p, List<Choice> choices)
        {
            return GetPlayerChoice(p, choices.ToArray());
        }

        private Choice GetPlayerChoice(Player p, params Choice[] choices)
        {
            return this.Agents[_gs.Players.IndexOf(p)].Choose(_gs, choices);
        }

        private IEnumerable<Choice> GetPlayerChoices(Player p, IEnumerable<Choice> choices)
        {
            return GetPlayerChoices(p, choices.ToArray());
        }

        private IEnumerable<Choice> GetPlayerChoices(Player p, params Choice[] choices)
        {
            return this.Agents[_gs.Players.IndexOf(p)].ChooseMany(_gs, choices);
        }

        private void ApplyAction(GameAction a)
        {
            this.Actions.Add(a);
            this.States.Add(_gs.Apply(a));
            _notify(a);
        }

        private void ApplyActions(string desc, params GameAction[] actions)
        {
            ApplyAction(new CompoundAction(desc, actions));
        }

        private void ApplyActions(string desc, IEnumerable<GameAction> actions)
        {
            ApplyActions(desc, actions.ToArray());
        }

        private void Step_Start()
        {
            switch (_gs.StartState.Status)
            {
                case StartStateStatus.Unstarted:
                    Step_Start_ShuffleAndDraw();
                    break;
                case StartStateStatus.Shuffled:
                    Step_Start_FlipToStart();
                    break;
                case StartStateStatus.StartingPlayerDecided:
                    Step_Start_SetLifeTotals();
                    break;
                case StartStateStatus.LifeTotalsSet:
                    Step_Start_InitialDraw();
                    break;
                case StartStateStatus.InitialDraw:
                    Step_Start_MulliganOrKeep();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        // 103.1. At the start of a game, each player shuffles his or her deck so that the cards are in a 
        // random order. Each player may then shuffle or cut his or her opponents' decks. The players' 
        // decks become their libraries.
        private void Step_Start_ShuffleAndDraw()
        {
            ApplyActions(
                "Shuffle and draw",
                _gs.Players.Select(p => new ShuffleLibrary(p))
                .Concat(new SetStartStateStatus(StartStateStatus.Shuffled).Yield<GameAction>())
            );
        }

        // 103.2. After the decks have been shuffled, the players determine which one of them will choose 
        // who takes the first turn. In the first game of a match (including a single-game match), the 
        // players may use any mutually agreeable method (flipping a coin, rolling dice, etc.) to do so. 
        // In a match of several games, the loser of the previous game chooses who takes the first turn. 
        // If the previous game was a draw, the player who made the choice in that game makes the choice 
        // in this game. The player chosen to take the first turn is the starting player.  The game's 
        // default turn order begins with the starting player and proceeds clockwise.
        private void Step_Start_FlipToStart()
        {
            var flip = RNG.Flip();
            var player = _gs.Players[(int)flip];
            var choice = GetPlayerChoice(player, Choice.PlayFirst, Choice.PlayLast);
            var startingPlayer = choice == Choice.PlayFirst ? player : _gs.Opponent(player);
            ApplyActions(
                "Flip to start",
                new PlayerWinsFlip(player, flip),
                new PlayerChoice(player, choice),
                new SetStartingPlayer(startingPlayer),
                new SetStartStateStatus(StartStateStatus.StartingPlayerDecided)
            );
        }

        // 103.3. Each player begins the game with a starting life total of 20. Some variant games have 
        // different starting life totals.
        private void Step_Start_SetLifeTotals()
        {
            ApplyActions(
                "Set life totals",
                _gs.Players.Select(p => new SetLifeTotal(p, 20))
                .Concat(new SetStartStateStatus(StartStateStatus.LifeTotalsSet).Yield<GameAction>())
            );
        }

        // 103.4. Each player draws a number of cards equal to his or her starting hand size, which is 
        // normally seven. (Some effects can modify a player's starting hand size.) A player who is 
        // dissatisfied with his or her initial hand may take a mulligan. First, the starting player 
        // declares whether or not he or she will take a mulligan. Then each other player in turn order 
        // does the same. Once each player has made a declaration, all players who decided to take 
        // mulligans do so at the same time. To take a mulligan, a player shuffles his or her hand back 
        // into his or her library, then draws a new hand of one fewer cards than he or she had before. 
        // If a player kept his or her hand of cards, those cards become the player's opening hand, and 
        // that player may not take any further mulligans. This process is then repeated until no player 
        // takes a mulligan. (Note that if a player's hand size reaches zero cards, that player must keep 
        // that hand.)
        private void Step_Start_InitialDraw()
        {
            ApplyActions(
                "Initial draw",
                _gs.Players.Select(p => DrawCard.X(p, 7))
                .Concat(new SetStartStateStatus(StartStateStatus.InitialDraw).Yield<GameAction>())
            );
        }

        private void Step_Start_MulliganOrKeep()
        {
            var actions = new List<GameAction>();
            foreach (var p in _gs.PlayersFrom(_gs.Starting).Where(p => !_gs.StartState.Keeps.Contains(p)))
            {
                var i = _gs.Players.IndexOf(p);
                var size = _gs.Hands[i].Size;
                if (size != 0)
                {
                    var choice = GetPlayerChoice(p, Choice.Keep, Choice.Mulligan);
                    if (choice == Choice.Mulligan)
                    {
                        actions.Add(new Mulligan(p, size - 1));
                    }
                    else
                    {
                        actions.Add(new KeepStartingHand(p));
                    }
                }
                else
                {
                    actions.Add(new KeepStartingHand(p));
                }
            }

            if (!actions.OfType<Mulligan>().Any())
            {
                actions.Add(new SetStartStateStatus(StartStateStatus.HandsKept));
                actions.Add(new SetActive(_gs.Starting));
                actions.Add(new SetPriority(_gs.Starting));
            }

            ApplyActions("Mulligan or keep", actions);
        }

        private void Step_BeginStep()
        {
            var actions = new List<GameAction>();

            if (_gs.Phase == Phase.Beginning && _gs.Step == Step.Untap)
            {
                actions.Add(new Untap(_gs.Active));
            }
            else if (_gs.Phase == Phase.Beginning && _gs.Step == Step.Draw)
            {
                actions.Add(new DrawCard(_gs.Active));
            }
            else if (_gs.Phase == Phase.Combat && _gs.Step == Step.DeclareAttackers)
            {
                var choices = GetPlayerChoices(_gs.Active, _gs.Battlefield.Objects.Where(p => CanAttack(p)).Select(p => new Choice<Permanent>(p)));
            }

            actions.Add(new SetBeginningOfStep(false));
            ApplyActions(string.Format("Beginning of {0} Phase, {1} Step", _gs.Phase, _gs.Step), actions);
        }

        private bool CanAttack(Permanent p)
        {
            return p.Controller == _gs.Active &&
                p.CardTypes.Contains(CardType.Creature) &&
                ThisTurnStartingState().Battlefield.Objects.Contains(p);
        }

        private void Step_NextStep()
        {
            List<PhaseStep> phaseSteps = new List<PhaseStep>()
            {
                new PhaseStep(Phase.Beginning, Step.Untap),
                new PhaseStep(Phase.Beginning, Step.Upkeep),
                new PhaseStep(Phase.Beginning, Step.Draw),
                new PhaseStep(Phase.PreCombatMain, Step.None),
                new PhaseStep(Phase.Combat, Step.BeginningofCombat),
                new PhaseStep(Phase.Combat, Step.DeclareAttackers),
                new PhaseStep(Phase.Combat, Step.DeclareBlockers),
                new PhaseStep(Phase.Combat, Step.CombatDamage),
                new PhaseStep(Phase.PostCombatMain, Step.None),
                new PhaseStep(Phase.Ending, Step.End),
                new PhaseStep(Phase.Ending, Step.Cleanup)
            };

            var index = phaseSteps.IndexOf(_gs.PhaseStep);
            
            if(index == -1)
            {
                throw new InvalidStateException();
            }
            var nextIndex = (index + 1) % phaseSteps.Count;
            var next = phaseSteps[nextIndex];
            ApplyActions("Next Step", new SetPhase(next.Phase), new SetStep(next.Step), new SetBeginningOfStep(true));

            if(nextIndex == 0)
            {
                ApplyAction(new IncrementTurnNumber());
            }
        }

        // 115.1. Special actions are actions a player may take when he or she has priority that don't 
        // use the stack. These are not to be confused with turn-based actions and state-based actions, 
        // which the game generates automatically.
        private IEnumerable<Choice> Choice_SpecialActions()
        {
            var choices = new List<Choice>();

            // 505.5b During either main phase, the active player may play one land card from his 
            // or her hand if the stack is empty, if the player has priority, and if he or she hasn't 
            // played a land this turn (unless an effect states the player may play additional lands). 
            // This action doesn't use the stack. Neither the land nor the action of playing the land 
            // is a spell or ability, so it can't be countered, and players can't respond to it with 
            // instants or activated abilities. (See rule 305, "Lands.")
            var nonInstant = (_gs.Phase == Phase.PreCombatMain || _gs.Phase == Phase.PostCombatMain)
                && _gs.Stack.Empty;

            var cardsPlayed = Actions.Reverse<GameAction>()
                .TakeWhile(ga => !(ga is IncrementTurnNumber))
                .OfType<PlayCardAction>()
                .ToList();

            var landsPlayed = cardsPlayed.None(pca => pca.Card.IsLand);

            var canPlayLand = nonInstant && landsPlayed;

            var activeIndex = _gs.Players.IndexOf(_gs.Active);
            if(canPlayLand)
            {
                _gs.Hands[activeIndex].Objects
                    .Where(c => c.CardTypes.Contains(CardType.Land))
                    .ForEach(c => choices.Add(new PlayCardChoice(_gs.Active, c)));
            }

            return choices;
        }

        private IEnumerable<Choice> Choice_CastSpell()
        {
            var choices = new List<Choice>();

            // 301.1. A player who has priority may cast an artifact card from his or her hand 
            // during a main phase of his or her turn when the stack is empty. Casting an artifact 
            // as a spell uses the stack. (See rule 601, "Casting Spells.")

            // 302.1. A player who has priority may cast a creature card from his or her hand 
            // during a main phase of his or her turn when the stack is empty. Casting a creature 
            // as a spell uses the stack. (See rule 601, "Casting Spells.")

            // 303.1. A player who has priority may cast an enchantment card from his or her hand 
            // during a main phase of his or her turn when the stack is empty. Casting an enchantment 
            // as a spell uses the stack. (See rule 601, "Casting Spells.")

            // 306.1. A player who has priority may cast a planeswalker card from his or her hand 
            // during a main phase of his or her turn when the stack is empty. Casting a planeswalker 
            // as a spell uses the stack. (See rule 601, "Casting Spells.")

            // 307.1. A player who has priority may cast a sorcery card from his or her hand 
            // during a main phase of his or her turn when the stack is empty. Casting a sorcery 
            // as a spell uses the stack. (See rule 601, "Casting Spells.")
            var canCastNonInstants = (_gs.Phase == Phase.PreCombatMain || _gs.Phase == Phase.PostCombatMain) 
                && _gs.Stack.Empty 
                && _gs.Priority == _gs.Active;

            var activeIndex = _gs.Players.IndexOf(_gs.Active);
            var nonInstantTypes = new CardType[] { CardType.Artifact, CardType.Creature, CardType.Enchantment, 
                CardType.Planeswalker, CardType.Sorcery };
            if (canCastNonInstants)
            {
                _gs.Hands[activeIndex]
                    .Objects
                    .Where(c => c.CardTypes.Any(ct => nonInstantTypes.Contains(ct)))
                    .ForEach(c => choices.Add(new PlayCardChoice(_gs.Active, c)));
            }

            // 304.1. A player who has priority may cast an instant card from his or her hand. 
            // Casting an instant as a spell uses the stack. (See rule 601, "Casting Spells.")
            _gs.Hands[activeIndex]
                .Objects
                .Where(c => c.CardTypes.Contains(CardType.Instant))
                .ForEach(c => choices.Add(new PlayCardChoice(_gs.Active, c)));

            return choices;
        }

    }
}
