import urllib
import urllib.request
import re
import string
import sys
import os
from bs4 import BeautifulSoup

# configs
OUTPUT_DIR  = "cards"
OUTPUT_SUFFIX = '.tsv'
SETS_TO_SKIP  = ['Unhinged', 'Unglued']

# cleans up text
def sanitize(span):
  # replace dashs to make parsing later easier
  sanitizedText = str(span).replace('—','-') # aka \u2014
  # strip line breaks
  sanitizedText = sanitizedText.replace('\r','')
  sanitizedText = sanitizedText.replace('\n','')
  # get rid of extra white spaces
  sanitizedText = re.sub('\s\s*',' ',sanitizedText)
  # casting cost
  sanitizedText = re.sub('<[^>]*alt="White"[^>]*>','{W}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Blue"[^>]*>','{U}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Black"[^>]*>','{B}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Red"[^>]*>','{R}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Green"[^>]*>','{G}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Variable Colorless"[^>]*>', '{X}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="(\d+)"[^>]*>',r'{\1}',sanitizedText)
  # some of these don't exist; I've put them all in to just to be safe
  sanitizedText = re.sub('<[^>]*alt="White or Blue"[^>]*>',r'{W/U}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="White or Black"[^>]*>',r'{W/B}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="White or Red"[^>]*>',r'{W/R}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="White or Green"[^>]*>',r'{W/G}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Blue or White"[^>]*>','{U/W}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Blue or Black"[^>]*>',r'{U/B}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Blue or Red"[^>]*>',r'{U/R}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Blue or Green"[^>]*>',r'{U/G}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Black or White"[^>]*>',r'{B/W}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Black or Blue"[^>]*>',r'{B/U}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Black or Red"[^>]*>',r'{B/R}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Black or Green"[^>]*>',r'{B/G}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Red or White"[^>]*>',r'{R/W}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Red or Blue"[^>]*>',r'{R/U}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Red or Black"[^>]*>',r'{R/B}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Red or Green"[^>]*>',r'{R/G}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Green or White"[^>]*>',r'{G/W}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Green or Blue"[^>]*>',r'{G/U}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Green or Black"[^>]*>',r'{G/B}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Green or Red"[^>]*>',r'{G/R}',sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Two or White"[^>]*>',"{2/W}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Two or Blue"[^>]*>',"{2/U}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Two or Black"[^>]*>',"{2/B}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Two or Red"[^>]*>',"{2/R}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Two or Green"[^>]*>',"{2/G}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Phyrexian White"[^>]*>',"{W/P}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Phyrexian Blue"[^>]*>',"{U/P}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Phyrexian Black"[^>]*>',"{B/P}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Phyrexian Red"[^>]*>',"{R/P}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Phyrexian Green"[^>]*>',"{G/P}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Snow"[^>]*>',"{S}",sanitizedText)
  # abilities
  sanitizedText = re.sub('<[^>]*alt="Tap"[^>]*>',"{T}",sanitizedText)
  sanitizedText = re.sub('<[^>]*alt="Untap"[^>]*>',"{Q}",sanitizedText)
  # convert paragraphs accordingly
  sanitizedText = re.sub('</p>\s*<p>','|',sanitizedText)
  # remove any remaining html
  sanitizedText = re.sub('<[^>]*>','',sanitizedText)
  # turn double quotes into single
  sanitizedText = re.sub('"','\'',sanitizedText)
  return sanitizedText.strip()

def getSets():
  sets = []
  html = urllib.request.urlopen('http://gatherer.wizards.com/Pages/Default.aspx').read()
  soup = BeautifulSoup(html)
  for setTag in soup.find('select',id='ctl00_ctl00_MainContent_Content_SearchControls_setAddText').find_all('option'):
    if setTag.string is not None:
      sets.append(setTag.string)
  return sets
  
def getSetPage(set, index):
  html = urllib.request.urlopen('http://gatherer.wizards.com/Pages/Search/Default.aspx?set=[%22' + urllib.parse.quote_plus(set) + '%22]&page=' + str(index)).read()
  return html

def getMaxPage(html):
  soup = BeautifulSoup(html)
  maxPage = 0
  pagingTag = soup.find('div','pagingControls')
  if pagingTag is None:
    return 0
  for pageTag in pagingTag.find_all('a'):
    match = re.match('.*page=(\d+)', str(pageTag))
    if match is not None:
      pageNum = int(match.group(1))
      if pageNum > maxPage:
        maxPage = pageNum
  return maxPage

def toFileName(name):
  valid_chars = "_()%s%s" % (string.ascii_letters, string.digits)
  return ''.join(c for c in name if c in valid_chars)

def noneToString(string):
  return 'None' if string is None else string

def noneToEmpty(string):
  return '' if string is None else string
  
def parseType(type):
  match = re.match('(?P<super>Basic|Legendary|Snow|World)?(?P<types>[^-(]*)(\s*-\s*)?(?P<subtypes>[^(]+)?(\s*\((?P<power>[0-9*]+)/(?P<toughness>[0-9*]+)\))?',type)
  return noneToString(match.group('super')).strip(), \
    noneToString(match.group('types')).strip().replace(' ', ','), \
    noneToString(match.group('subtypes')).strip().replace(' ', ','), \
    noneToEmpty(match.group('power')).strip(), \
    noneToEmpty(match.group('toughness')).strip()

def parseRarity(title):
  match = re.match('[^(]*\((?P<rarity>[^)]*)\)',title)
  return match.group('rarity').strip()

def writeHeaderToFile(f):
  f.write('Name\tManaCost\tSuperType\tTypes\tSubTypes\tText\tPower\tToughness\n')
  
def parsePageToFile(html, f):
  soup = BeautifulSoup(html)
  for cardItem in soup.find_all('tr', 'cardItem'):
    cardinfo = cardItem.find('div', 'cardInfo')
    name = sanitize(cardinfo.find('span', 'cardTitle').find('a').contents[0])
    manaCost = sanitize(cardinfo.find('span', 'manaCost'))
    super,types,subtypes,power,toughness = parseType(sanitize(cardinfo.find('span', 'typeLine')))
    text = sanitize(cardinfo.find('div', 'rulesText'))
    rarity = parseRarity(cardItem.find('td', 'setVersions').find('img')['title'])
    f.write(name + '\t' + manaCost + '\t' + super + '\t' + types + '\t' + subtypes + '\t' + text + '\t' + power + '\t' + toughness + '\n')

def uniqueAdd(list, obj):
  if obj not in list:
    list.append(obj)

def getSetsToSkip():
  return SETS_TO_SKIP

def printCardTypeEnum(list):
  all = []
  for cardType in list:
    for token in cardType.split(' '):
      uniqueAdd(all, token)
  all.sort()
  for enumName in all:
    print(enumName + ',')

def writeListToFile(list,fileName):
  f = open(fileName, 'w')
  for i in list:
    f.write(i + '\n')

dirFiles = os.listdir(OUTPUT_DIR)

sets = getSets()

for set in sets:
  print('Processing ' + set + ' ... ')
  if set in getSetsToSkip():
    print('... skip set; skipping')
    continue
  fileName = toFileName(set) + OUTPUT_SUFFIX
  if fileName in dirFiles:
    print('... already exists; skipping')
    continue
  f = open(os.path.join(OUTPUT_DIR, fileName), 'w', encoding='utf-8')
  html = getSetPage(set, 0)
  maxPage = getMaxPage(html)
  print('... ' + fileName + ' : 0/' + str(maxPage))
  writeHeaderToFile(f)
  parsePageToFile(html, f)
  for pageNum in range(maxPage):
    html = getSetPage(set, pageNum + 1)
    print('... ' + fileName + ' : ' + str(pageNum + 1) + '/' + str(maxPage))
    parsePageToFile(html, f)