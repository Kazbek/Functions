using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Intervals.Interval
{
    [TestClass]
    public class Union
    {
        [TestMethod]
        [DataRow(1, false, 2, false, 2, true, 3, false, true)]
        [DataRow(3, false, 5, false, 2, true, 3, true, true)]
        [DataRow(0, false, 2, false, 2, false, 12, false, false)]
        [DataRow(1, false, 2, false, -7, true, 1, true, true)]
        [DataRow(1, false, 2, true, 2, true, 3, false, true)]
        [DataRow(1, false, 23, true, 2, true, 3, false, true)]
        [DataRow(1, false, 23, true, 2, true, 32, false, true)]
        [DataRow(1, true, 2, false, 2, true, 3, false, true)]
        public void TwoIntTryUnion(int ps1, bool is1, int pe1, bool ie1, int ps2, bool is2, int pe2, bool ie2, bool tryUnion)
        {
            IInterval<int> first = new Interval<int>(ps1, is1, pe1, ie1);
            IInterval<int> second = new Interval<int>(ps2, is2, pe2, ie2);
            Assert.AreEqual(first.TryUnion(second), tryUnion);
            Assert.AreEqual(second.TryUnion(first), tryUnion);
        }

        [TestMethod]
        public void UnionSequence()
        {
            IInterval<int> first = new Interval<int>(1, true, 2, false);
            IInterval<int> second = new Interval<int>(2, true, 3, false);
            IInterval<int> third = new Interval<int>(3, true, 4, false);
            IInterval<int> interval = first;
            interval = interval.Union(second);
            interval = interval.Union(third);

            Assert.AreEqual(interval.Start.Position.CompareTo(1) == 0, true);
            Assert.AreEqual(interval.Start.Inclusive, true);
            Assert.AreEqual(interval.End.Position.CompareTo(4) == 0, true);
            Assert.AreEqual(interval.End.Inclusive, false);
        }

        [TestMethod]
        public void TryUnionSequence()
        {
            IInterval<int> first = new Interval<int>(1, true, 2, false);
            IInterval<int> second = new Interval<int>(2, true, 3, false);
            IInterval<int> third = new Interval<int>(3, true, 4, false);
            IInterval<int> interval = first;

            Assert.AreEqual(first.TryUnion(second), true);
            Assert.AreEqual(interval.TryUnion(second), true);
            interval = interval.Union(second);
            Assert.AreEqual(interval.TryUnion(third), true);
            interval = interval.Union(third);

            Assert.AreEqual(interval.Start.Position.CompareTo(1) == 0, true);
            Assert.AreEqual(interval.Start.Inclusive, true);
            Assert.AreEqual(interval.End.Position.CompareTo(4) == 0, true);
            Assert.AreEqual(interval.End.Inclusive, false);
        }
    }
}
