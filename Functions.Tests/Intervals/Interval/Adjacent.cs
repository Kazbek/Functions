using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Functions.Implementations.Intervals;
using Functions.Interfaces;

namespace Functions.Tests.Intervals.Interval
{
    [TestClass]
    public class Adjacent
    {
        [TestMethod]
        [DataRow(1, false, 2, false, 2, true, 3, false, true)]
        [DataRow(3, false, 5, false, 2, true, 3, true, true)]
        [DataRow(0, false, 2, false, 2, false, 12, false, false)]
        [DataRow(1, false, 2, false, -7, true, 1, true, true)]
        [DataRow(1, false, 2, true, 2, true, 3, false, false)]
        [DataRow(1, false, 23, true, 2, true, 3, false, false)]
        [DataRow(1, false, 23, true, 2, true, 32, false, false)]
        public void IsAdjacent(int ps1, bool is1, int pe1, bool ie1, int ps2, bool is2, int pe2, bool ie2, bool isAdjacent)
        {
            IInterval<int> first = new Interval<int>(ps1, is1, pe1, ie1);
            IInterval<int> second = new Interval<int>(ps2, is2, pe2, ie2);
            Assert.AreEqual(isAdjacent, first.IsAdjacent(second));
            Assert.AreEqual(isAdjacent, second.IsAdjacent(first));
        }

        [TestMethod]
        [DataRow(1, false, 2, false, 2, true, 3, false, true)]
        [DataRow(3, false, 5, false, 2, true, 3, true, false)]
        [DataRow(0, false, 2, false, 2, false, 12, false, false)]
        [DataRow(1, false, 2, false, -7, true, 1, true, false)]
        [DataRow(1, false, 2, true, 2, true, 3, false, false)]
        [DataRow(1, false, 23, true, 2, true, 3, false, false)]
        [DataRow(1, false, 23, true, 2, true, 32, false, false)]
        [DataRow(1, true, 2, false, 2, true, 3, false, true)]
        public void IsAdjacentRightLeft(int ps1, bool is1, int pe1, bool ie1, int ps2, bool is2, int pe2, bool ie2, bool isAdjacent)
        {
            IInterval<int> first = new Interval<int>(ps1, is1, pe1, ie1);
            IInterval<int> second = new Interval<int>(ps2, is2, pe2, ie2);
            Assert.AreEqual(isAdjacent, first.IsAdjacentRight(second));
            Assert.AreEqual(isAdjacent, second.IsAdjacentLeft(first));
        }

        [TestMethod]
        [DataRow(2012, 1, true, 2012, 2, false,
                 2012, 2, true, 2012, 3, false, true)]
        [DataRow(2012, 1, true, 2012, 2, true,
                 2012, 2, true, 2012, 3, false, false)]
        [DataRow(2012, 1, true, 2012, 2, true,
                 2012, 2, false, 2012, 3, false, true)]
        [DataRow(2012, 3, true, 2012, 4, true,
                 2012, 2, false, 2012, 3, false, false)]
        [DataRow(1990, 3, true, 2012, 7, false,
                 2000, 5, true, 2006, 3, false, false)]
        [DataRow(1990, 3, true, 2012, 7, true,
                 2012, 7, false, 2015, 3, false, true)]
        public void IsAdjacentRightLeftDateTime(int year11, int month11, bool include11, int year12, int month12, bool include12, int year21, int month21, bool include21, int year22, int month22, bool include22, bool isAdjacent)
        {
            IInterval<DateTime> first = new Interval<DateTime>(new DateTime(year11, month11, 1), include11, new DateTime(year12, month12, 1), include12);
            IInterval<DateTime> second = new Interval<DateTime>(new DateTime(year21, month21, 1), include21, new DateTime(year22, month22, 1), include22);
            Assert.AreEqual(isAdjacent, first.IsAdjacentRight(second));
            Assert.AreEqual(isAdjacent, second.IsAdjacentLeft(first));
        }
    }
}
