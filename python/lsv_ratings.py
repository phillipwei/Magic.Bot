import re
import sys
import urllib
from bs4 import BeautifulSoup

def setUTF8():
    reload(sys)
    sys.setdefaultencoding("utf-8")

setUTF8()

CARD_DIR = "cards"
f = open(CARD_DIR + "\Magic2014CoreSet.tsv", 'r')
cards = []
for l in f:
    splits = l.split('\t')
    cards.append(splits[0])

urls = [
    'http://www.channelfireball.com/articles/magic-2014-set-review-artifacts-and-lands/',
    'http://www.channelfireball.com/home/magic-2014-set-review-green/',
    'http://www.channelfireball.com/articles/magic-2014-set-review-black/',
    'http://www.channelfireball.com/articles/magic-2014-set-review-red/',
    'http://www.channelfireball.com/articles/magic-2014-set-review-blue/',
    'http://www.channelfireball.com/articles/magic-2014-set-review-white/'
    #'http://www.channelfireball.com/home/gatecrash-set-review-bluesimic/',
    #'http://www.channelfireball.com/home/gatecrash-set-review-red/',
    #'http://www.channelfireball.com/home/gatecrash-set-review-greengruul/',
    #'http://www.channelfireball.com/home/gatecrash-set-review-black/',
    #'http://www.channelfireball.com/home/gatecrash-set-review-whiteorzhov/',
    #'http://www.channelfireball.com/home/gatecrash-set-review-boros/',
    #'http://www.channelfireball.com/home/gatecrash-set-review-dimir/'
    ]

valid_tags = ['strong', 'h2']

for url in urls:
    html = urllib.urlopen(url).read()
    soup = BeautifulSoup(html)
    #soup = BeautifulSoup(open('index.html', 'r').read())
    for tag in soup.find_all():
        if tag.name in valid_tags and tag.contents:
            title = unicode(tag.contents[0]).replace(u"\u2019","'")
            if title in cards:
                for sibling in (tag.next_siblings if tag.next_sibling else tag.parent.next_siblings):
                    if sibling.text and sibling.text.startswith('Limited:'):
                        print title + '\t' + sibling.text.split(' ')[1]
                        break