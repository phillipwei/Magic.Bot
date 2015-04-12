using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core.Test
{
    [TestClass]
    public class IOTest
    {
        public class TestObjectA
        {
            public string Letter { get; set; }
            public int Number { get; set; }
        }

        [TestMethod]
        public void LoadFromFileBasicCsv()
        {
            var list = IO.LoadFromFile<TestObjectA>(@"Data\LoadFromFileA.csv", ',', ';', "---");
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("abc", list[0].Letter);
            Assert.AreEqual(123, list[0].Number);
            Assert.AreEqual("xyz", list[1].Letter);
            Assert.AreEqual(456, list[1].Number);
        }

        public enum TestEnumA
        {
            Apple,
            Banana,
            Grape
        }

        public class TestObjectB
        {
            public int Id { get; set; }
            public TestEnumA Fruit { get; set; }
        }

        [TestMethod]
        public void LoadFromFileEnums()
        {
            var list = IO.LoadFromFile<TestObjectB>(@"Data\LoadFromFileB.csv", ',', ';', "---");
        }
    }
}
