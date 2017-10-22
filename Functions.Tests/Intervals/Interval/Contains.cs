using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Functions.Implementations.Intervals;
using Functions.Interfaces;

namespace Functions.Tests.Intervals.Interval
{
    [TestClass]
    public class Contains
    {
        [TestMethod]
        [DataRow(-7, true, 23, false, 21, true)]
        [DataRow(-7, true, 23, false, 23, false)]
        [DataRow(-7, true, 23, false, -7, true)]
        [DataRow(-7, true, 23, true, -27, false)]
        [DataRow(23, true, 23, true, 23, true)]
        [DataRow(0, false, 23, false, 1, true)]
        [DataRow(0, false, 23, false, 0, false)]
        public void Contain(int start, bool inclusiveStart, int end, bool inclusiveEnd, int value, bool result)
        {
            IInterval<int> interval = new Interval<int>(start, inclusiveStart, end, inclusiveEnd);
            Assert.AreEqual(interval.Contains(value), result);
        }
    }
}
