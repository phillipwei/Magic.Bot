using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Autobot;

namespace Magic.Auto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string draftImageDirectory = @"P:\Code\svn\Projects\magic\cs\Magic.App\bin\Debug\Screenshot\1";
            string[] draftImageExtensions = new string[] { ".png", ".bmp" };

            DraftParser parser = new DraftParser();
            foreach (string file in Directory.GetFiles(draftImageDirectory))
            {
                if (draftImageExtensions.Contains(new FileInfo(file).Extension))
                {
                    DraftParser.Result result = parser.Read(FastAccessImage.FromPath(file));
                    Console.WriteLine(file + " = " + result);
                }
            }
        }
    }
}
