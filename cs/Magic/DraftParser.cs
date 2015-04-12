using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autobot;
using Magic.Core;

namespace Magic.Auto
{
    public class DraftParser
    {
        private ImageParser.GlyphLookup cardNameLookup;
        private List<Rectangle> cardNameRectangles;
        private FastAccessImage cardImageLoading;
        private List<Rectangle> cardImageRectangles;
        private ImageParser.GlyphLookup countLookup;
        private Rectangle countRectangle;
        private List<ImageParser.GlyphLookup> timeRemainingLookups;
        private Rectangle timeRemainingRectangle;
        private FastAccessImage pickAccepted;
        private Rectangle pickAcceptedRectangle;
        private FastAccessImage submittingPick;
        private Rectangle submittingPickRectangle;

        public DraftParser()
        {
            this.Initialize();
        }

        public static string GuessSetName(IEnumerable<string> cardPrefixes)
        {
            Dictionary<string, int> setCounts = new Dictionary<string, int>();
            foreach (string cardPrefix in cardPrefixes)
            {
                foreach (OldCard card in OldCard.All)
                {
                    if (card.Name.StartsWith(cardPrefix))
                    {
                        if (!setCounts.ContainsKey(card.SetName))
                        {
                            setCounts[card.SetName] = 0;
                        }

                        setCounts[card.SetName]++;
                    }
                }
            }

            int max = setCounts.Values.Max();
            return setCounts.Where(pair => max == pair.Value).Select(pair => pair.Key).First();
        }

        public Result Read(FastAccessImage image, string setName = null)
        {
            Result result = new Result();
            foreach (var lookup in this.timeRemainingLookups)
            {
                string timeRemainingString = ImageParser.Read(image, lookup, this.timeRemainingRectangle);
                result.TimeRemaining = this.ParseTimeRemainingString(timeRemainingString);
                if (result.TimeRemaining != TimeSpan.Zero)
                {
                    break;
                }
            }

            string countString = ImageParser.Read(image, this.countLookup, this.countRectangle);
            result.Count = this.ParseCountString(countString);
            result.CardsByPosition = new OldCard[15];
            
            if (result.Count != 0 ||
                !image.Matches(this.pickAccepted, this.pickAcceptedRectangle) ||
                !image.Matches(this.submittingPick, this.submittingPickRectangle) ||
                this.cardImageRectangles.TrueForAll(r => !image.CloselyMatches(this.cardImageLoading, r, 10, 0.75)))
            {
                List<string> cardNamePrefixes = ImageParser.Read(image, this.cardNameLookup, this.cardNameRectangles);
                if (setName == null && cardNamePrefixes.Exists(s => s != string.Empty))
                {
                    setName = GuessSetName(cardNamePrefixes.Where(s => s != string.Empty));
                }

                result.CardsByPosition = cardNamePrefixes.ConvertAll(s => s == string.Empty ? null : OldCard.FindOne(setName, s + ".*")).ToArray();
            }

            return result;
        }

        private TimeSpan ParseTimeRemainingString(string s)
        {
            if (s == string.Empty || s == "--:--")
            {
                return TimeSpan.Zero;
            }
            else
            {
                string[] splits = s.Split(new char[] { ':' });
                return TimeSpan.FromSeconds((Convert.ToInt32(splits[0]) * 60) + Convert.ToInt32(splits[1]));
            }
        }

        private int ParseCountString(string s)
        {
            if (s == string.Empty)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(s);
            }
        }

        private void Initialize()
        {
            this.cardNameLookup = new ImageParser.GlyphLookup(
                "Data\\draft_card_font.png",
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'-,Æ".Select(c => c.ToString()).ToList(),
                4);
            this.cardNameRectangles = new List<Rectangle>()
            {
                new Rectangle(163, 512, 75, 10),
                new Rectangle(269, 512, 75, 10),
                new Rectangle(375, 512, 75, 10),
                new Rectangle(479, 512, 75, 10),
                new Rectangle(585, 512, 75, 10),
                new Rectangle(691, 512, 75, 10),
                new Rectangle(797, 512, 75, 10),
                new Rectangle(902, 512, 75, 10),
                new Rectangle(163, 635, 75, 10),
                new Rectangle(269, 635, 75, 10),
                new Rectangle(375, 635, 75, 10),
                new Rectangle(479, 635, 75, 10),
                new Rectangle(585, 635, 75, 10),
                new Rectangle(691, 635, 75, 10),
                new Rectangle(797, 635, 75, 10)
            };
            this.cardImageLoading = FastAccessImage.FromPath("Data\\draft_card_image_loading.png");
            this.cardImageRectangles = this.cardNameRectangles.ConvertAll(
                r => new Rectangle(r.X, r.Y + 11, this.cardImageLoading.Width, this.cardImageLoading.Height));
            this.countLookup = new ImageParser.GlyphLookup(
                "Data\\draft_count_font.png",
                "0123456789".Select(c => c.ToString()).ToList(),
                4);
            this.countRectangle = new Rectangle(21, 728, 26, 9);
            this.timeRemainingLookups = new List<ImageParser.GlyphLookup>()
            {
                new ImageParser.GlyphLookup(
                    "Data\\draft_clock_font.png", 
                    "0123456789:-".Select(c => c.ToString()).ToList(), 
                    4),
                new ImageParser.GlyphLookup(
                    "Data\\draft_clock_yellow_font.png", 
                    "0123456789:-".Select(c => c.ToString()).ToList(), 
                    4),
                new ImageParser.GlyphLookup(
                    "Data\\draft_clock_red_font.png", 
                    "0123456789:-".Select(c => c.ToString()).ToList(), 
                    4)
            };
            this.timeRemainingRectangle = new Rectangle(59, 492, 36, 8);
            this.pickAccepted = FastAccessImage.FromPath("Data\\pick_accepted.png");
            this.pickAcceptedRectangle = new Rectangle(190, 512, this.pickAccepted.Width, this.pickAccepted.Height);
            this.submittingPick = FastAccessImage.FromPath("Data\\submitting_pick.png");
            this.submittingPickRectangle = new Rectangle(190, 512, this.submittingPick.Width, this.submittingPick.Height);
        }

        public class Result
        {
            public TimeSpan TimeRemaining { get; set; }

            public int Count { get; set; }

            public OldCard[] CardsByPosition { get; set; }

            public bool HasCards
            {
                get
                {
                    return this.CardsByPosition.Count(c => c != null) != 0;
                }
            }

            public IEnumerable<OldCard> Cards
            {
                get
                {
                    return this.CardsByPosition.Where(c => c != null);
                }
            }

            public override string ToString()
            {
                return string.Format("T({0}) #{1}: {2}", this.TimeRemaining, this.Count, string.Join(",", this.Cards));
            }
        }
    }
}
