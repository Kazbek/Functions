using System;
using System.Collections.Generic;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Intervals.Interval
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        [DataRow(1,false, 3, true)]
        [DataRow(7, true, 7, true)]
        [DataRow(-34, false, 45, false)]
        public void IntFromDataRowByValues(int position1, bool inclusive1, int position2, bool inclusive2)
        {
            Interval<int> interval = new Interval<int>(position1, inclusive1, position2, inclusive2);
            Assert.AreEqual(interval.Start.Position, position1);
            Assert.AreEqual(interval.Start.Inclusive, inclusive1);
            Assert.AreEqual(interval.End.Position, position2);
            Assert.AreEqual(interval.End.Inclusive, inclusive2);
        }

        [TestMethod]
        [DataRow(1, false, 3, true)]
        [DataRow(7, true, 7, true)]
        [DataRow(-34, false, 45, false)]
        public void IntFromDataRowByIntervals(int position1, bool inclusive1, int position2, bool inclusive2)
        {
            IIntervalEdge<int> start = new IntervalEdge<int>(position1, inclusive1);
            IIntervalEdge<int> end = new IntervalEdge<int>(position2, inclusive2);
            IInterval<int> interval = new Interval<int>(start, end);
            Assert.AreEqual(interval.Start.Position, position1);
            Assert.AreEqual(interval.Start.Inclusive, inclusive1);
            Assert.AreEqual(interval.End.Position, position2);
            Assert.AreEqual(interval.End.Inclusive, inclusive2);
        }
    }
}
