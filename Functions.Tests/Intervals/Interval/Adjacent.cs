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
            Assert.AreEqual(first.IsAdjacent(second), isAdjacent);
            Assert.AreEqual(second.IsAdjacent(first), isAdjacent);
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
            Assert.AreEqual(first.IsAdjacentRight(second), isAdjacent);
            Assert.AreEqual(second.IsAdjacentLeft(first), isAdjacent);
        }
    }
}
