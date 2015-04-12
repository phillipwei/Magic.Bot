import random
from pprint import pprint as pp
from pprint import pformat as pf

CARD_DIR = "cards"
DOTS_LEN = 39
RATINGS_DIR = "lsv_ratings"

class Card:
    def __init__(self,name,type,subtype,rarity,cc,desc,power,toughness,is_foil):
        self.name = name
        self.type = type
        self.subtype = subtype
        self.rarity = rarity
        self.is_foil = is_foil
        self.cc = cc
        self.desc = desc
        self.power = power
        self.toughness = toughness
    
    def __repr__(self):
        return '{name}{foil_txt}'.format(
            name=self.name, 
            foil_txt=' (Foil)' if self.is_foil else '')
    
    def full_str(self):
        return '{name}; {cc}; {type} - {subtype}; {rarity}; {desc}; [{power}/{toughness}]'.format(
            name=self.name,
            cc=self.cc,
            type=self.type,
            subtype=self.type,
            rarity=self.rarity,
            desc=self.desc,
            power=self.power,
            toughness=self.toughness)

def create_pack(name):
    is_mythic = random.choice([True, False])
    is_foil   = random.choice([True, False])
    cards = list()
    cards.append(random.choice(mythics if is_mythic else rares))
    cards.extend(random.sample(uncommons, 3))
    cards.extend(random.sample(commons, 9))
    cards.append(random.choice(foils if is_foil else commons))
    cards.append(random.choice(lands))
    return Pack(name, cards)
        
class Pack:
    def __init__(self,name,cards):
        self.name = name
        self.cards = cards
        self.drafted = []
    
    def __repr__(self):
        s = '{dots} {name} {dots}\n'.format(name=self.name, dots='='*((DOTS_LEN-2-len(self.name))/2))
        s = s + self.cards_str()
        for i,c in enumerate(self.drafted):
            s = s + '{i}. {card}\n'.format(i=i, card=c)
        s = s + '='*(DOTS_LEN) + '\n'
        return s

    def draft(self,card):
        self.drafted.append(card)
        self.cards.remove(card)
    
    def cards_str(self):
        s = ''
        for i,c in enumerate(self.cards):
            s = s + '{a}. {card}\n'.format(a=chr(ord('a') + i), card=c)
        return s
        
        
class Player:
    def __init__(self,name):
        self.name = name
        self.pack = None
        self.drafted = []
        
    def __repr__(self):
        s = '{dots} {name} {dots}\n'.format(name=self.name, dots='='*((DOTS_LEN-2-len(self.name))/2))
        for i,c in enumerate(self.drafted):
            s = s + '{i}. {card}\n'.format(i=i, card=c)
        s = s + '='*(DOTS_LEN) + '\n'
        return s
    
    def draft(self,card):
        self.pack.draft(card)
        self.drafted.append(card)
    
    def draft_with_strategy(self,strategy):
        card = random.choice(self.pack.cards)
        self.draft(card)
    
class Draft:
    def __init__(self):
        self.packs = []
        self.players = []
        self.round = 0
        for i in range(8):
            player = Player('Player #{0}'.format(i))
            self.players.append(player)
        self.hero = self.players[0]
        self.opponents = self.players[1:]
        self.deal_packs()
        
    def show(self):
        print self.hero
        print self.hero.pack.name
        print self.hero.pack.cards_str()
    
    def opponents_draft(self):
        for o in self.opponents:
            o.draft_with_strategy(None)
    
    def deal_packs(self):
        for player in self.players:
            pack = create_pack('Pack #{0}'.format(len(self.packs)))
            player.pack = pack
            self.packs.append(pack)

    def pass_packs(self):
        old_packs = [player.pack for player in self.players]
        for i,player in enumerate(self.players):
            player.pack = old_packs[(i + 1) % len(self.players)]
    
    def process_packs(self):
        if self.players[0].pack.cards:
            self.pass_packs()
        elif len(self.packs) is 24:
            return
        else:
            self.deal_packs()
        self.round = self.round + 1
    
    def card_for_letter(self,letter):
        i = ord(letter) - ord('a')
        return self.hero.pack.cards[i]
        
    def process(self,input):
        try:
            if input == '':
                self.show()
            elif input.startswith('info'):
                print self.card_for_letter(input.split(' ')[1]).full_str()
            else:
                card = self.card_for_letter(input)
                self.hero.draft(card)
                self.opponents_draft()
                self.process_packs()
                print 'OK.'
                self.show()
        except:
            print 'Try again.'

def loop():
    d = Draft()
    d.show()
    while True:
        response = raw_input('> ')
        d.process(response) 
        
f = open(CARD_DIR + "\Theros.tsv", 'r')
commons, uncommons, rares, mythics, lands, foils = [], [], [], [], [], []
lookup = {'Common': commons, 'Uncommon': uncommons, 'Rare': rares, 'Mythic Rare': mythics}
for l in f:
    l = l.rstrip()
    splits = l.split('\t')
    splits.extend([None, None])
    if splits[4] != 'Land':
        lookup[splits[4]].append(Card(splits[0], splits[5], splits[6], splits[4], splits[2], splits[7], splits[8], splits[9], False))
        foils.append(Card(splits[0], splits[5], splits[6], splits[4], splits[2], splits[7], splits[8], splits[9], True))
for l in ['Mountain', 'Forest', 'Island', 'Swamp', 'Plains']:
    lands.append(Card(l, 'Land', None, 'Land', None, None, None, None, False))
    foils.append(Card(l, 'Land', None, 'Land', None, None, None, None, True))

'''
ratings = {}
f = open(RATINGS_DIR + "\Gatecrash.tsv", 'r')
for l in f:
    splits = l.split('\t')
    try:
        ratings[splits[0]] = float(splits[1])
    except:
        print 'Could not load: ' + l
        
for c in foils:
    if not ratings.has_key(c.name):
        print 'Missing ' + c.name
'''
loop()