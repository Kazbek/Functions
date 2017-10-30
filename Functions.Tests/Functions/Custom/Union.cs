using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Functions.Custom
{
    [TestClass]
    public class Union
    {
        [TestMethod]
        public void SuccessfullyUnited()
        {
            int Func(int t) => t + 1;
            Interval<int> interval1 = new Interval<int>(1, true, 10, true);
            Interval<int> interval2 = new Interval<int>(5, true, 15, true);

            Custom<int, int> custom1 = new Custom<int, int>(interval1, Func);
            Custom<int, int> custom2 = new Custom<int, int>(interval2, Func);

            Assert.IsTrue(custom1.TryUnion(custom2));
            Assert.IsTrue(custom2.TryUnion(custom1));

            IFunction<int, int> united1 = custom1.Union(custom2);
            IFunction<int, int> united2 = custom2.Union(custom1);

            Assert.AreEqual(united1.Interval.Start.Position, 1);
            Assert.AreEqual(united1.Interval.Start.Inclusive, true);
            Assert.AreEqual(united1.Interval.End.Position, 15);
            Assert.AreEqual(united1.Interval.End.Inclusive, true);
            Assert.AreEqual(united1.Value(5), 6);

            Assert.AreEqual(united2.Interval.Start.Position, 1);
            Assert.AreEqual(united2.Interval.Start.Inclusive, true);
            Assert.AreEqual(united2.Interval.End.Position, 15);
            Assert.AreEqual(united2.Interval.End.Inclusive, true);
            Assert.AreEqual(united2.Value(5), 6);
        }

        [TestMethod]
        public void UnsuccessfullyUnited()
        {
            int F1(int i) => i;
            int F2(int i) => i;
            Interval<int> interval1 = new Interval<int>(1, true, 10, true);
            Interval<int> interval2 = new Interval<int>(5, true, 15, true);

            Custom<int, int> custom1 = new Custom<int, int>(interval1, F1);
            Custom<int, int> custom2 = new Custom<int, int>(interval2, F2);

            Assert.IsFalse(custom1.TryUnion(custom2));
            Assert.IsFalse(custom2.TryUnion(custom1));
        }

        [TestMethod]
        public void UnsuccessfullyDirectUnited()
        {
            int F1(int i) => i;
            int F2(int i) => i;
            Interval<int> interval1 = new Interval<int>(1, true, 10, true);
            Interval<int> interval2 = new Interval<int>(5, true, 15, true);

            Custom<int, int> custom1 = new Custom<int, int>(interval1, i => i);
            Custom<int, int> custom2 = new Custom<int, int>(interval2, i => i);

            Assert.IsFalse(custom1.TryUnion(custom2));
            Assert.IsFalse(custom2.TryUnion(custom1));
        }
    }
}
