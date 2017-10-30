using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Functions.Implementations.Intervals;

namespace Functions.Tests.Intervals.Interval
{
    [TestClass]
    public class Intersect
    {
        [DataRow(1, false, 2, false, 2, false, 3, false, false)]
        [DataRow(1, false, 2, false, 2, true, 3, false, false)]
        [DataRow(1, false, 2, true, 2, true, 3, false, true)]

        [DataRow(-4, false, 5, false, 7, false, 12, true, false)]
        [DataRow(-4, false, 3, false, 17, true, 312, true, false)]
        [DataRow(-4, false, 2, true, 73, true, 142, true, false)]

        [DataRow(2, false, 213, false, 7, false, 12, false, false)]
        [DataRow(1, false, 223, false, 9, false, 134, true, false)]
        [DataRow(5, false, 432, false, 32, true, 34, true, false)]

        [DataRow(2, true, 213, true, 7, false, 12, false, true)]
        [DataRow(1, false, 223, true, 9, false, 134, true, true)]
        [DataRow(5, false, 432, false, 32, true, 34, true, true)]

        [DataRow(1, false, 23, false, 5, false, 35, false, true)]
        [DataRow(1, false, 23, true, 5, false, 35, false, true)]
        [DataRow(1, true, 23, true, 5, false, 35, false, true)]

        [DataRow(1, false, 23, false, 5, true, 35, false, true)]
        [DataRow(1, false, 23, true, 5, true, 35, false, true)]
        [DataRow(1, true, 23, true, 5, true, 35, false, true)]

        [DataRow(1, false, 23, false, 5, true, 35, true, true)]
        [DataRow(1, false, 23, true, 5, true, 35, true, true)]
        [DataRow(1, true, 23, true, 5, true, 35, true, true)]
        public void IntIntersect(int p1, bool ip1, int p2, bool ip2, int p3, bool ip3, int p4, bool ip4, bool result)
        {
            Interval<int> first = new Interval<int>(p1, ip1, p2, ip2);
            Interval<int> second = new Interval<int>(p3, ip3, p4, ip4);

            Assert.AreEqual(first.Intersect(second), result);
            Assert.AreEqual(second.Intersect(first), result);
        }
    }
}
