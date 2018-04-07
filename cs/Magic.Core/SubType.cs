using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Magic.Core
{
    // {205.3}
    public abstract class SubType
    {
        public abstract IEnumerable<CardType> CardTypes { get; }
        public string Name { get; private set; }

        public SubType(string name)
        {
            this.Name = name;
        }

        private static bool _initialized = false;
        private static Dictionary<string, SubType> _subTypes = new Dictionary<string, SubType>();
        private static void Load()
        {
            if (_initialized)
            {
                return;
            }

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(SubType)))
                {
                    foreach(var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        if (field.FieldType.IsSubclassOf(typeof(SubType)))
                        {
                            var item = field.GetValue(null) as SubType;
                            _subTypes.Add(field.Name, item);
                        }
                    }
                }
            }
            
            _initialized = true;
        }

        public static SubType Parse(string s)
        {
            Load();
            return _subTypes[s];
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class NoneSubType : SubType
    {
        public override IEnumerable<CardType> CardTypes { get { return Enumerable.Empty<CardType>(); } }
        public static NoneSubType None = new NoneSubType("None");
        public NoneSubType(string name) : base(name) { }
    }

    // {205.3g}
    public class ArtifactSubType : SubType
    {
        public override IEnumerable<CardType> CardTypes { get { return CardType.Artifact.Yield(); } }
        public static ArtifactSubType Contraption = new ArtifactSubType("Contraption");
        public static ArtifactSubType Equipment = new ArtifactSubType("Equipment");
        public static ArtifactSubType Fortification = new ArtifactSubType("Fortification");
        public static ArtifactSubType Vehicle = new ArtifactSubType("Vehicle");
        public ArtifactSubType(string name) : base(name) { }
    }

    // {205.3h}
    public class EnchantmentSubType : SubType
    {
        public override IEnumerable<CardType> CardTypes { get { return CardType.Enchantment.Yield(); } }
        public static EnchantmentSubType Aura = new EnchantmentSubType("Aura");
        public static EnchantmentSubType Curse = new EnchantmentSubType("Curse");
        public static EnchantmentSubType Shrine = new EnchantmentSubType("Shrine");
        public EnchantmentSubType(string name) : base(name) { }
    }

    // {205.3i}
    public class LandSubType : SubType
    {
        public override IEnumerable<CardType> CardTypes { get { return CardType.Land.Yield(); } }

        public static LandSubType Desert = new LandSubType("Desert");
        public static LandSubType Forest = new LandSubType("Forest");
        public static LandSubType Gate = new LandSubType("Gate");
        public static LandSubType Island = new LandSubType("Island");
        public static LandSubType Lair = new LandSubType("Lair");
        public static LandSubType Locus = new LandSubType("Locus");
        public static LandSubType Mine = new LandSubType("Mine");
        public static LandSubType Mountain = new LandSubType("Mountain");
        public static LandSubType Plains = new LandSubType("Plains");
        public static LandSubType PowerPlant = new LandSubType("PowerPlant");
        public static LandSubType Swamp = new LandSubType("Swamp");
        public static LandSubType Tower = new LandSubType("Tower");
        public static LandSubType Urzas = new LandSubType("Urzas");
        public LandSubType(string name) : base(name) { }
    }

    // {205.3j}
    public class PlaneswalkerSubType : SubType
    {
        public override IEnumerable<CardType> CardTypes { get { return CardType.Planeswalker.Yield(); } }

        public static PlaneswalkerSubType Ajani = new PlaneswalkerSubType("Ajani");
        public static PlaneswalkerSubType Ashiok = new PlaneswalkerSubType("Ashiok");
        public static PlaneswalkerSubType Bolas = new PlaneswalkerSubType("Bolas");
        public static PlaneswalkerSubType Chandra = new PlaneswalkerSubType("Chandra");
        public static PlaneswalkerSubType Domri = new PlaneswalkerSubType("Domri");
        public static PlaneswalkerSubType Dovin = new PlaneswalkerSubType("Dovin");
        public static PlaneswalkerSubType Elspeth = new PlaneswalkerSubType("Elspeth");
        public static PlaneswalkerSubType Garruk = new PlaneswalkerSubType("Garruk");
        public static PlaneswalkerSubType Gideon = new PlaneswalkerSubType("Gideon");
        public static PlaneswalkerSubType Jace = new PlaneswalkerSubType("Jace");
        public static PlaneswalkerSubType Karn = new PlaneswalkerSubType("Karn");
        public static PlaneswalkerSubType Koth = new PlaneswalkerSubType("Koth");
        public static PlaneswalkerSubType Liliana = new PlaneswalkerSubType("Liliana");
        public static PlaneswalkerSubType Nissa = new PlaneswalkerSubType("Nissa");
        public static PlaneswalkerSubType Ral = new PlaneswalkerSubType("Ral");
        public static PlaneswalkerSubType Sarkhan = new PlaneswalkerSubType("Sarkhan");
        public static PlaneswalkerSubType Saheeli = new PlaneswalkerSubType("Saheeli");
        public static PlaneswalkerSubType Sorin = new PlaneswalkerSubType("Sorin");
        public static PlaneswalkerSubType Tamiyo = new PlaneswalkerSubType("Tamiyo");
        public static PlaneswalkerSubType Tezzeret = new PlaneswalkerSubType("Tezzeret");
        public static PlaneswalkerSubType Tibalt = new PlaneswalkerSubType("Tibalt");
        public static PlaneswalkerSubType Venser = new PlaneswalkerSubType("Venser");
        public static PlaneswalkerSubType Vraska = new PlaneswalkerSubType("Vraska");
        public static PlaneswalkerSubType Xenagos = new PlaneswalkerSubType("Xenagos");
        public PlaneswalkerSubType(string name) : base(name) { }
    }

    public class SpellSubType : SubType
    {
        private static CardType[] _cardTypes = new CardType[] { CardType.Instant, CardType.Sorcery };
        public override IEnumerable<CardType> CardTypes { get { return _cardTypes; } }

        public static SpellSubType Arcane = new SpellSubType("Arcane");
        public static SpellSubType Trap = new SpellSubType("Trap");
        public SpellSubType(string name) : base(name) { }
    }

    public class CreatureSubType : SubType
    {
        private static CardType[] _cardTypes = new CardType[] { CardType.Creature, CardType.Tribal };
        public override IEnumerable<CardType> CardTypes { get { return _cardTypes; } }

        public static CreatureSubType Advisor = new CreatureSubType("Advisor");
        public static CreatureSubType Aetherborn = new CreatureSubType("Aetherborn");
        public static CreatureSubType Ally = new CreatureSubType("Ally");
        public static CreatureSubType Angel = new CreatureSubType("Angel");
        public static CreatureSubType Anteater = new CreatureSubType("Anteater");
        public static CreatureSubType Antelope = new CreatureSubType("Antelope");
        public static CreatureSubType Ape = new CreatureSubType("Ape");
        public static CreatureSubType Archer = new CreatureSubType("Archer");
        public static CreatureSubType Archon = new CreatureSubType("Archon");
        public static CreatureSubType Artificer = new CreatureSubType("Artificer");
        public static CreatureSubType Assassin = new CreatureSubType("Assassin");
        public static CreatureSubType AssemblyWorker = new CreatureSubType("AssemblyWorker");
        public static CreatureSubType Atog = new CreatureSubType("Atog");
        public static CreatureSubType Aurochs = new CreatureSubType("Aurochs");
        public static CreatureSubType Avatar = new CreatureSubType("Avatar");
        public static CreatureSubType Badger = new CreatureSubType("Badger");
        public static CreatureSubType Barbarian = new CreatureSubType("Barbarian");
        public static CreatureSubType Basilisk = new CreatureSubType("Basilisk");
        public static CreatureSubType Bat = new CreatureSubType("Bat");
        public static CreatureSubType Bear = new CreatureSubType("Bear");
        public static CreatureSubType Beast = new CreatureSubType("Beast");
        public static CreatureSubType Beeble = new CreatureSubType("Beeble");
        public static CreatureSubType Berserker = new CreatureSubType("Berserker");
        public static CreatureSubType Bird = new CreatureSubType("Bird");
        public static CreatureSubType Blinkmoth = new CreatureSubType("Blinkmoth");
        public static CreatureSubType Boar = new CreatureSubType("Boar");
        public static CreatureSubType Bringer = new CreatureSubType("Bringer");
        public static CreatureSubType Brushwagg = new CreatureSubType("Brushwagg");
        public static CreatureSubType Camarid = new CreatureSubType("Camarid");
        public static CreatureSubType Camel = new CreatureSubType("Camel");
        public static CreatureSubType Caribou = new CreatureSubType("Caribou");
        public static CreatureSubType Carrier = new CreatureSubType("Carrier");
        public static CreatureSubType Cat = new CreatureSubType("Cat");
        public static CreatureSubType Centaur = new CreatureSubType("Centaur");
        public static CreatureSubType Cephalid = new CreatureSubType("Cephalid");
        public static CreatureSubType Chimera = new CreatureSubType("Chimera");
        public static CreatureSubType Citizen = new CreatureSubType("Citizen");
        public static CreatureSubType Cleric = new CreatureSubType("Cleric");
        public static CreatureSubType Cockatrice = new CreatureSubType("Cockatrice");
        public static CreatureSubType Construct = new CreatureSubType("Construct");
        public static CreatureSubType Coward = new CreatureSubType("Coward");
        public static CreatureSubType Crab = new CreatureSubType("Crab");
        public static CreatureSubType Crocodile = new CreatureSubType("Crocodile");
        public static CreatureSubType Cyclops = new CreatureSubType("Cyclops");
        public static CreatureSubType Dauthi = new CreatureSubType("Dauthi");
        public static CreatureSubType Demon = new CreatureSubType("Demon");
        public static CreatureSubType Deserter = new CreatureSubType("Deserter");
        public static CreatureSubType Devil = new CreatureSubType("Devil");
        public static CreatureSubType Djinn = new CreatureSubType("Djinn");
        public static CreatureSubType Dragon = new CreatureSubType("Dragon");
        public static CreatureSubType Drake = new CreatureSubType("Drake");
        public static CreatureSubType Dreadnought = new CreatureSubType("Dreadnought");
        public static CreatureSubType Drone = new CreatureSubType("Drone");
        public static CreatureSubType Druid = new CreatureSubType("Druid");
        public static CreatureSubType Dryad = new CreatureSubType("Dryad");
        public static CreatureSubType Dwarf = new CreatureSubType("Dwarf");
        public static CreatureSubType Efreet = new CreatureSubType("Efreet");
        public static CreatureSubType Elder = new CreatureSubType("Elder");
        public static CreatureSubType Eldrazi = new CreatureSubType("Eldrazi");
        public static CreatureSubType Elemental = new CreatureSubType("Elemental");
        public static CreatureSubType Elephant = new CreatureSubType("Elephant");
        public static CreatureSubType Elf = new CreatureSubType("Elf");
        public static CreatureSubType Elk = new CreatureSubType("Elk");
        public static CreatureSubType Eye = new CreatureSubType("Eye");
        public static CreatureSubType Faerie = new CreatureSubType("Faerie");
        public static CreatureSubType Ferret = new CreatureSubType("Ferret");
        public static CreatureSubType Fish = new CreatureSubType("Fish");
        public static CreatureSubType Flagbearer = new CreatureSubType("Flagbearer");
        public static CreatureSubType Fox = new CreatureSubType("Fox");
        public static CreatureSubType Frog = new CreatureSubType("Frog");
        public static CreatureSubType Fungus = new CreatureSubType("Fungus");
        public static CreatureSubType Gargoyle = new CreatureSubType("Gargoyle");
        public static CreatureSubType Germ = new CreatureSubType("Germ");
        public static CreatureSubType Giant = new CreatureSubType("Giant");
        public static CreatureSubType Gnome = new CreatureSubType("Gnome");
        public static CreatureSubType Goat = new CreatureSubType("Goat");
        public static CreatureSubType Goblin = new CreatureSubType("Goblin");
        public static CreatureSubType God = new CreatureSubType("God");
        public static CreatureSubType Golem = new CreatureSubType("Golem");
        public static CreatureSubType Gorgon = new CreatureSubType("Gorgon");
        public static CreatureSubType Graveborn = new CreatureSubType("Graveborn");
        public static CreatureSubType Gremlin = new CreatureSubType("Gremlin");
        public static CreatureSubType Griffin = new CreatureSubType("Griffin");
        public static CreatureSubType Hag = new CreatureSubType("Hag");
        public static CreatureSubType Harpy = new CreatureSubType("Harpy");
        public static CreatureSubType Hellion = new CreatureSubType("Hellion");
        public static CreatureSubType Hippo = new CreatureSubType("Hippo");
        public static CreatureSubType Hippogriff = new CreatureSubType("Hippogriff");
        public static CreatureSubType Homarid = new CreatureSubType("Homarid");
        public static CreatureSubType Homunculus = new CreatureSubType("Homunculus");
        public static CreatureSubType Horror = new CreatureSubType("Horror");
        public static CreatureSubType Horse = new CreatureSubType("Horse");
        public static CreatureSubType Hound = new CreatureSubType("Hound");
        public static CreatureSubType Human = new CreatureSubType("Human");
        public static CreatureSubType Hydra = new CreatureSubType("Hydra");
        public static CreatureSubType Hyena = new CreatureSubType("Hyena");
        public static CreatureSubType Illusion = new CreatureSubType("Illusion");
        public static CreatureSubType Imp = new CreatureSubType("Imp");
        public static CreatureSubType Incarnation = new CreatureSubType("Incarnation");
        public static CreatureSubType Insect = new CreatureSubType("Insect");
        public static CreatureSubType Jellyfish = new CreatureSubType("Jellyfish");
        public static CreatureSubType Juggernaut = new CreatureSubType("Juggernaut");
        public static CreatureSubType Kavu = new CreatureSubType("Kavu");
        public static CreatureSubType Kirin = new CreatureSubType("Kirin");
        public static CreatureSubType Kithkin = new CreatureSubType("Kithkin");
        public static CreatureSubType Knight = new CreatureSubType("Knight");
        public static CreatureSubType Kobold = new CreatureSubType("Kobold");
        public static CreatureSubType Kor = new CreatureSubType("Kor");
        public static CreatureSubType Kraken = new CreatureSubType("Kraken");
        public static CreatureSubType Lammasu = new CreatureSubType("Lammasu");
        public static CreatureSubType Leech = new CreatureSubType("Leech");
        public static CreatureSubType Leviathan = new CreatureSubType("Leviathan");
        public static CreatureSubType Lhurgoyf = new CreatureSubType("Lhurgoyf");
        public static CreatureSubType Licid = new CreatureSubType("Licid");
        public static CreatureSubType Lizard = new CreatureSubType("Lizard");
        public static CreatureSubType Manticore = new CreatureSubType("Manticore");
        public static CreatureSubType Masticore = new CreatureSubType("Masticore");
        public static CreatureSubType Mercenary = new CreatureSubType("Mercenary");
        public static CreatureSubType Merfolk = new CreatureSubType("Merfolk");
        public static CreatureSubType Metathran = new CreatureSubType("Metathran");
        public static CreatureSubType Minion = new CreatureSubType("Minion");
        public static CreatureSubType Minotaur = new CreatureSubType("Minotaur");
        public static CreatureSubType Monger = new CreatureSubType("Monger");
        public static CreatureSubType Mongoose = new CreatureSubType("Mongoose");
        public static CreatureSubType Monk = new CreatureSubType("Monk");
        public static CreatureSubType Monkey = new CreatureSubType("Monkey");
        public static CreatureSubType Moonfolk = new CreatureSubType("Moonfolk");
        public static CreatureSubType Mutant = new CreatureSubType("Mutant");
        public static CreatureSubType Myr = new CreatureSubType("Myr");
        public static CreatureSubType Mystic = new CreatureSubType("Mystic");
        public static CreatureSubType Nautilus = new CreatureSubType("Nautilus");
        public static CreatureSubType Nephilim = new CreatureSubType("Nephilim");
        public static CreatureSubType Nightmare = new CreatureSubType("Nightmare");
        public static CreatureSubType Nightstalker = new CreatureSubType("Nightstalker");
        public static CreatureSubType Ninja = new CreatureSubType("Ninja");
        public static CreatureSubType Noggle = new CreatureSubType("Noggle");
        public static CreatureSubType Nomad = new CreatureSubType("Nomad");
        public static CreatureSubType Nymph = new CreatureSubType("Nymph");
        public static CreatureSubType Octopus = new CreatureSubType("Octopus");
        public static CreatureSubType Ogre = new CreatureSubType("Ogre");
        public static CreatureSubType Ooze = new CreatureSubType("Ooze");
        public static CreatureSubType Orb = new CreatureSubType("Orb");
        public static CreatureSubType Orc = new CreatureSubType("Orc");
        public static CreatureSubType Orgg = new CreatureSubType("Orgg");
        public static CreatureSubType Ouphe = new CreatureSubType("Ouphe");
        public static CreatureSubType Ox = new CreatureSubType("Ox");
        public static CreatureSubType Oyster = new CreatureSubType("Oyster");
        public static CreatureSubType Pegasus = new CreatureSubType("Pegasus");
        public static CreatureSubType Pentavite = new CreatureSubType("Pentavite");
        public static CreatureSubType Pest = new CreatureSubType("Pest");
        public static CreatureSubType Phelddagrif = new CreatureSubType("Phelddagrif");
        public static CreatureSubType Phoenix = new CreatureSubType("Phoenix");
        public static CreatureSubType Pilot = new CreatureSubType("Pilot");
        public static CreatureSubType Pincher = new CreatureSubType("Pincher");
        public static CreatureSubType Pirate = new CreatureSubType("Pirate");
        public static CreatureSubType Plant = new CreatureSubType("Plant");
        public static CreatureSubType Praetor = new CreatureSubType("Praetor");
        public static CreatureSubType Prism = new CreatureSubType("Prism");
        public static CreatureSubType Rabbit = new CreatureSubType("Rabbit");
        public static CreatureSubType Rat = new CreatureSubType("Rat");
        public static CreatureSubType Rebel = new CreatureSubType("Rebel");
        public static CreatureSubType Reflection = new CreatureSubType("Reflection");
        public static CreatureSubType Rhino = new CreatureSubType("Rhino");
        public static CreatureSubType Rigger = new CreatureSubType("Rigger");
        public static CreatureSubType Rogue = new CreatureSubType("Rogue");
        public static CreatureSubType Sable = new CreatureSubType("Sable");
        public static CreatureSubType Salamander = new CreatureSubType("Salamander");
        public static CreatureSubType Samurai = new CreatureSubType("Samurai");
        public static CreatureSubType Sand = new CreatureSubType("Sand");
        public static CreatureSubType Saproling = new CreatureSubType("Saproling");
        public static CreatureSubType Satyr = new CreatureSubType("Satyr");
        public static CreatureSubType Scarecrow = new CreatureSubType("Scarecrow");
        public static CreatureSubType Scorpion = new CreatureSubType("Scorpion");
        public static CreatureSubType Scout = new CreatureSubType("Scout");
        public static CreatureSubType Serf = new CreatureSubType("Serf");
        public static CreatureSubType Serpent = new CreatureSubType("Serpent");
        public static CreatureSubType Shade = new CreatureSubType("Shade");
        public static CreatureSubType Shaman = new CreatureSubType("Shaman");
        public static CreatureSubType Shapeshifter = new CreatureSubType("Shapeshifter");
        public static CreatureSubType Sheep = new CreatureSubType("Sheep");
        public static CreatureSubType Siren = new CreatureSubType("Siren");
        public static CreatureSubType Skeleton = new CreatureSubType("Skeleton");
        public static CreatureSubType Slith = new CreatureSubType("Slith");
        public static CreatureSubType Sliver = new CreatureSubType("Sliver");
        public static CreatureSubType Slug = new CreatureSubType("Slug");
        public static CreatureSubType Snake = new CreatureSubType("Snake");
        public static CreatureSubType Soldier = new CreatureSubType("Soldier");
        public static CreatureSubType Soltari = new CreatureSubType("Soltari");
        public static CreatureSubType Spawn = new CreatureSubType("Spawn");
        public static CreatureSubType Specter = new CreatureSubType("Specter");
        public static CreatureSubType Spellshaper = new CreatureSubType("Spellshaper");
        public static CreatureSubType Sphinx = new CreatureSubType("Sphinx");
        public static CreatureSubType Spider = new CreatureSubType("Spider");
        public static CreatureSubType Spike = new CreatureSubType("Spike");
        public static CreatureSubType Spirit = new CreatureSubType("Spirit");
        public static CreatureSubType Splinter = new CreatureSubType("Splinter");
        public static CreatureSubType Sponge = new CreatureSubType("Sponge");
        public static CreatureSubType Squid = new CreatureSubType("Squid");
        public static CreatureSubType Squirrel = new CreatureSubType("Squirrel");
        public static CreatureSubType Starfish = new CreatureSubType("Starfish");
        public static CreatureSubType Surrakar = new CreatureSubType("Surrakar");
        public static CreatureSubType Survivor = new CreatureSubType("Survivor");
        public static CreatureSubType Tetravite = new CreatureSubType("Tetravite");
        public static CreatureSubType Thalakos = new CreatureSubType("Thalakos");
        public static CreatureSubType Thopter = new CreatureSubType("Thopter");
        public static CreatureSubType Thrull = new CreatureSubType("Thrull");
        public static CreatureSubType Treefolk = new CreatureSubType("Treefolk");
        public static CreatureSubType Triskelavite = new CreatureSubType("Triskelavite");
        public static CreatureSubType Troll = new CreatureSubType("Troll");
        public static CreatureSubType Turtle = new CreatureSubType("Turtle");
        public static CreatureSubType Unicorn = new CreatureSubType("Unicorn");
        public static CreatureSubType Vampire = new CreatureSubType("Vampire");
        public static CreatureSubType Vedalken = new CreatureSubType("Vedalken");
        public static CreatureSubType Viashino = new CreatureSubType("Viashino");
        public static CreatureSubType Volver = new CreatureSubType("Volver");
        public static CreatureSubType Wall = new CreatureSubType("Wall");
        public static CreatureSubType Warrior = new CreatureSubType("Warrior");
        public static CreatureSubType Weird = new CreatureSubType("Weird");
        public static CreatureSubType Werewolf = new CreatureSubType("Werewolf");
        public static CreatureSubType Whale = new CreatureSubType("Whale");
        public static CreatureSubType Wizard = new CreatureSubType("Wizard");
        public static CreatureSubType Wolf = new CreatureSubType("Wolf");
        public static CreatureSubType Wolverine = new CreatureSubType("Wolverine");
        public static CreatureSubType Wombat = new CreatureSubType("Wombat");
        public static CreatureSubType Worm = new CreatureSubType("Worm");
        public static CreatureSubType Wraith = new CreatureSubType("Wraith");
        public static CreatureSubType Wurm = new CreatureSubType("Wurm");
        public static CreatureSubType Yeti = new CreatureSubType("Yeti");
        public static CreatureSubType Zombie = new CreatureSubType("Zombie");
        public static CreatureSubType Zubera = new CreatureSubType("Zubera");
        public CreatureSubType(string name) : base(name) { }
    }
}
