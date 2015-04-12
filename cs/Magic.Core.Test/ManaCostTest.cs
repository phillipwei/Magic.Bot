using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core.Test
{
    [TestClass]
    public class ManaCostTest
    {
        [TestMethod]
        public void OneVariableTest()
        {
            var mc = ManaCost.Parse("{X}");
            Assert.AreEqual(1, mc.Fragments.Count);
            Assert.AreEqual(typeof(VariableManaCostFragment), mc.Fragments[0].GetType());
        }

        [TestMethod]
        public void OneGenericTest()
        {
            for(int i=0; i<20; i++)
            {
                var mc = ManaCost.Parse("{" + i + "}");
                Assert.AreEqual(1, mc.Fragments.Count);
                Assert.AreEqual(typeof(GenericManaCostFragment), mc.Fragments[0].GetType());
                Assert.AreEqual(i, (mc.Fragments[0] as GenericManaCostFragment).Amount);
            }
        }

        [TestMethod]
        public void OneColoredTest()
        {
            foreach (var symbol in new string[] {"W", "U", "B", "G", "R"})
            {
                var mc = ManaCost.Parse("{" + symbol + "}");
                Assert.AreEqual(1, mc.Fragments.Count);
                Assert.AreEqual(typeof(ColoredManaCostFragment), mc.Fragments[0].GetType());
                Assert.AreEqual(1, (mc.Fragments[0] as ColoredManaCostFragment).Amount);
                Assert.AreEqual(symbol.ToColor(), (mc.Fragments[0] as ColoredManaCostFragment).Color);
            }
        }

        [TestMethod]
        public void RepeatingColoredTest()
        {
            foreach (var symbol in new string[] { "W", "U", "B", "G", "R" })
            {
                for (int i = 1; i < 5; i++)
                {
                    var mc = ManaCost.Parse(string.Concat(Enumerable.Repeat<string>("{" + symbol + "}", i)));
                    Assert.AreEqual(1, mc.Fragments.Count);
                    Assert.AreEqual(typeof(ColoredManaCostFragment), mc.Fragments[0].GetType());
                    Assert.AreEqual(i, (mc.Fragments[0] as ColoredManaCostFragment).Amount);
                    Assert.AreEqual(symbol.ToColor(), (mc.Fragments[0] as ColoredManaCostFragment).Color);
                }
            }
        }

        [TestMethod]
        public void IncinerateTest()
        {
            var mc = ManaCost.Parse("{1}{R}");
            Assert.AreEqual(2, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<ColoredManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<ColoredManaCostFragment>().First().Amount);
            Assert.AreEqual(Color.Red, mc.Fragments.OfType<ColoredManaCostFragment>().First().Color);
        }

        [TestMethod]
        public void TerminateTest()
        {
            var mc = ManaCost.Parse("{B}{R}");
            Assert.AreEqual(2, mc.Fragments.Count);
            Assert.AreEqual(2, mc.Fragments.OfType<ColoredManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.Black).First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.Red).First().Amount);
        }

        [TestMethod]
        public void ForceOfNatureTest()
        {
            var mc = ManaCost.Parse("{2}{G}{G}{G}{G}");
            Assert.AreEqual(2, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().Count());
            Assert.AreEqual(2, mc.Fragments.OfType<GenericManaCostFragment>().First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<ColoredManaCostFragment>().Count());
            Assert.AreEqual(4, mc.Fragments.OfType<ColoredManaCostFragment>().First().Amount);
            Assert.AreEqual(Color.Green, mc.Fragments.OfType<ColoredManaCostFragment>().First().Color);
        }

        [TestMethod]
        public void ProgenitusTest()
        {
            var mc = ManaCost.Parse("{W}{W}{U}{U}{B}{B}{R}{R}{G}{G}");
            Assert.AreEqual(5, mc.Fragments.Count);
            Assert.AreEqual(5, mc.Fragments.OfType<ColoredManaCostFragment>().Count());
            Assert.AreEqual(2, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.White).First().Amount);
            Assert.AreEqual(2, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.Blue).First().Amount);
            Assert.AreEqual(2, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.Black).First().Amount);
            Assert.AreEqual(2, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.Red).First().Amount);
            Assert.AreEqual(2, mc.Fragments.OfType<ColoredManaCostFragment>().Where(f => f.Color == Color.Green).First().Amount);
        }

        [TestMethod]
        public void BorosReckonerTest()
        {
            var mc = ManaCost.Parse("{R/W}{R/W}{R/W}");
            Assert.AreEqual(1, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<HybridManaCostFragment>().Count());
            Assert.AreEqual(3, mc.Fragments.OfType<HybridManaCostFragment>().First().Amount);
            Assert.AreEqual(Color.Red, mc.Fragments.OfType<HybridManaCostFragment>().First().ColorA);
            Assert.AreEqual(Color.White, mc.Fragments.OfType<HybridManaCostFragment>().First().ColorB);
        }

        [TestMethod]
        public void ReaperKingTest()
        {
            var mc = ManaCost.Parse("{2/W}{2/U}{2/B}{2/R}{2/G}");
            Assert.AreEqual(5, mc.Fragments.Count);
            Assert.AreEqual(5, mc.Fragments.OfType<MonoColoredManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<MonoColoredManaCostFragment>().Where(f => f.Color == Color.White).First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<MonoColoredManaCostFragment>().Where(f => f.Color == Color.Blue).First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<MonoColoredManaCostFragment>().Where(f => f.Color == Color.Black).First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<MonoColoredManaCostFragment>().Where(f => f.Color == Color.Red).First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<MonoColoredManaCostFragment>().Where(f => f.Color == Color.Green).First().Amount);
        }

        [TestMethod]
        public void SpectralProcessionTest()
        {
            var mc = ManaCost.Parse("{2/W}{2/W}{2/W}");
            Assert.AreEqual(1, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<MonoColoredManaCostFragment>().Count());
            Assert.AreEqual(3, mc.Fragments.OfType<MonoColoredManaCostFragment>().First().Amount);
            Assert.AreEqual(Color.White, mc.Fragments.OfType<MonoColoredManaCostFragment>().First().Color);
        }

        [TestMethod]
        public void BirthingPodTest()
        {
            var mc = ManaCost.Parse("{3}{G/P}");
            Assert.AreEqual(2, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().Count());
            Assert.AreEqual(3, mc.Fragments.OfType<GenericManaCostFragment>().First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<PhyrexianColoredManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<PhyrexianColoredManaCostFragment>().First().Amount);
            Assert.AreEqual(Color.Green, mc.Fragments.OfType<PhyrexianColoredManaCostFragment>().First().Color);
        }

        [TestMethod]
        public void DismemberTest()
        {
            var mc = ManaCost.Parse("{1}{B/P}{B/P}");
            Assert.AreEqual(2, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<PhyrexianColoredManaCostFragment>().Count());
            Assert.AreEqual(2, mc.Fragments.OfType<PhyrexianColoredManaCostFragment>().First().Amount);
            Assert.AreEqual(Color.Black, mc.Fragments.OfType<PhyrexianColoredManaCostFragment>().First().Color);
        }

        [TestMethod]
        public void ScryingSheetsTest()
        {
            var mc = ManaCost.Parse("{1}{S}");
            Assert.AreEqual(2, mc.Fragments.Count);
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<GenericManaCostFragment>().First().Amount);
            Assert.AreEqual(1, mc.Fragments.OfType<SnowColoredManaCostFragment>().Count());
            Assert.AreEqual(1, mc.Fragments.OfType<SnowColoredManaCostFragment>().First().Amount);
        }
    }
}
