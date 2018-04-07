#r @"..\Magic.Core\bin\Debug\Magic.Core.dll"

// #load @"fsharpfile.fs"

open Magic.Core

let data = @"P:\Code\git\Magic.Bot\cs\Magic.Core\Data"

// data
// |> 
// ahh, okay, thanks, yup, that's helpful.
// got it.
// ok, thanks!! night.
CardDefinition.Load(data + "\Sets")

let cards = Deck.LoadFromFile(data + "\Decks\Draft.txt").Main
// this is an Ienumerable<card>, yeah

let list = new System.Collections.Generic.List<CardDefinition>();
list.AddRange(cards)

let rs = new ReversibleShuffle(Seq.length list)

rs.Apply(list)

// great.

// so conceptually, how should one handle the back-and-forth
// of the types ... do you fully convert to F# and stay that way
// or ... just at the interface btw c# and f#?

// ienumerable

// got it

// I see.
// Not necessary to have a list ... i was just trying to
// reused as much old code -- i wrote this a year ago and 
// had a quick question i wanted to get the asnwer to

// and figured i'd learn some fsharp along the way
// ahh, i see. ok.

// you mean IEnumerable ... -- but not 


// good point
// yeah i'm just getting my grammar up to speed