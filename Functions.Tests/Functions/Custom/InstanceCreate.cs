using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;

namespace Functions.Tests.Functions.Custom
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        public void Incrementer()
        {
            int Func(int i) => i + 1;
            Interval<int> interval = new Interval<int>(0, true, 100, true);
            Custom<int, int> custom = new Custom<int, int>(interval, Func);

            Assert.AreEqual(custom.Interval.Start.Position, 0);
            Assert.AreEqual(custom.Interval.Start.Inclusive, true);
            Assert.AreEqual(custom.Interval.End.Position, 100);
            Assert.AreEqual(custom.Interval.End.Inclusive, true);

            for (int i = 0; i < 101; i++)
                Assert.AreEqual(custom.Value(i), i + 1);
        }
    }
}
