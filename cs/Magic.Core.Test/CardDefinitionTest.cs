using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Magic.Core.Test
{
    [TestClass]
    public class CardDefinitionTest
    {
        public class TimeTracker : IDisposable
        {
            Action<TimeSpan> _onComplete;
            Stopwatch _stopwatch;

            public TimeTracker(string name)
                : this(t => Console.WriteLine("{0} took {1} to complete.", name, t))
            {
            }

            public TimeTracker(Action<TimeSpan> onComplete)
            {
                this._onComplete = onComplete;
                this._stopwatch = new Stopwatch();
                this._stopwatch.Start();
            }

            public void Dispose()
            {
                this._onComplete(_stopwatch.Elapsed);
                this._stopwatch.Stop();
            }
        }

        [TestMethod]
        public void TimeLoadTest()
        {
            var maxLoadTime = TimeSpan.FromSeconds(1);
            using (new TimeTracker(t => {
                Console.WriteLine("CardDefinition.Load() took {0} to complete.", t);
                Assert.IsTrue(t < maxLoadTime);
            }))
            {
                CardDefinition.Load();
            }
        }

        [TestMethod]
        public void DisplayAllTest()
        {
            CardDefinition.Load();
            var sb = new StringBuilder();
            foreach (var c in CardDefinition.GetAll())
            {
                sb.AppendLine(c.DisplayString());
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
