using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core.Test
{
    [TestFixture]
    public class ReversibleShuffleTest
    {
        [Test]
        public void SmallListTest()
        {
            var list = Enumerable.Range(1, 10).ToList();
            var original = new List<int>(list);
            var rs = new ReversibleShuffle(list.Count);
            rs.Apply(list);
            Console.WriteLine(string.Join(",", list));
            rs.Undo(list);
            Console.WriteLine(string.Join(",", list));
            Assert.IsTrue(Enumerable.SequenceEqual(original, list));
        }

        [Test]
        public void BigListTest()
        {
            var list = Enumerable.Range(1, 1000).ToList();
            var original = new List<int>(list);
            var rs = new ReversibleShuffle(list.Count);
            rs.Apply(list);
            Console.WriteLine(string.Join(",", list));
            rs.Undo(list);
            Console.WriteLine(string.Join(",", list));
            Assert.IsTrue(Enumerable.SequenceEqual(original, list));
        }

    }
}
