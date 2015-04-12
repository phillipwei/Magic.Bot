using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    // 109.3. An object's characteristics are name, mana cost, color, color indicator, card type, subtype, supertype, 
    // rules text, abilities, power, toughness, loyalty, hand modifier, and life modifier. Objects can have some or 
    // all of these characteristics. Any other information about an object isn't a characteristic. For example, 
    // characteristics don't include whether a permanent is tapped, a spell's target, an object's owner or controller, 
    // what an Aura enchants, and so on.
    interface ICharacteristics
    {
        string Name { get; }
        ManaCost ManaCost { get; }
        Color Color { get; }
        Color ColorIndicator { get; }
        List<CardType> CardTypes { get; }
        List<SubType> SubTypes { get; }
        SuperType SuperType { get; }
        string RulesText { get; }
        List<Ability> Abilities { get; }
        string Power { get; }
        string Toughness { get; }
        int Loyalty { get; }
    }
}
