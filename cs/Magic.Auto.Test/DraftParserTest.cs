using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autobot;
using Magic.Auto;
using NUnit.Framework;

namespace Magic.Auto.Test
{
    [TestFixture]
    public static class DraftParserTest
    {
        private static readonly string DraftImageDirectory = "TestImages";
        private static readonly string[] DraftImageExtensions = new string[] { ".png", ".bmp" };

        [Test]
        public static void ReadDirectory()
        {
            DraftParser parser = new DraftParser();
            foreach (string file in Directory.GetFiles(DraftImageDirectory))
            {
                if (DraftImageExtensions.Contains(new FileInfo(file).Extension))
                {
                    DraftParser.Result result = parser.Read(FastAccessImage.FromPath(file));
                    Console.WriteLine(file + " = " + result);
                }
            }
        }
    }
}
