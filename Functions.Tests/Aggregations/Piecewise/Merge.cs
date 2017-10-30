using System.Collections.Generic;
using Functions.Implementations.Aggregations;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Aggregations.Piecewise
{
    [TestClass]
    public class Merge
    {
        [TestMethod]
        public void UnintersectedMerge()
        {
            List<IFunction<int, int>> list1 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(1, true, 7, false), 1),
                new Constant<int, int>(new Interval<int>(3, true, 9, false), 1),
                new Constant<int, int>(new Interval<int>(14, true, 17, false), 2),
                new Constant<int, int>(new Interval<int>(34, true, 39, false), 1),
            };

            List<IFunction<int, int>> list2 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(56, true, 349, false), 33),
                new Constant<int, int>(new Interval<int>(350, true, 900, false), 4)
            };

            Piecewise<int, int> piecewise1 = new Piecewise<int, int>(list1);
            Piecewise<int, int> piecewise2 = new Piecewise<int, int>(list2);

            Piecewise<int, int> merged1 = piecewise1.Merge(piecewise2);

            Assert.AreEqual(merged1.IntervalsCount, 5);
            Assert.AreEqual(merged1[4].Interval.Start.Position, 350);
            Assert.AreEqual(merged1.Value(400), 4);
            Assert.AreEqual(merged1[0].Interval.Start.Position, 1);
            Assert.AreEqual(merged1.Value(1), 1);
            Assert.IsFalse(merged1.IsDefinedOn(45));
            Assert.IsTrue(merged1.IsDefinedOn(60));

            Piecewise<int, int> merged2 = piecewise2.Merge(piecewise1);

            Assert.AreEqual(merged2.IntervalsCount, 5);
            Assert.AreEqual(merged2[4].Interval.Start.Position, 350);
            Assert.AreEqual(merged2.Value(400), 4);
            Assert.AreEqual(merged2[0].Interval.Start.Position, 1);
            Assert.AreEqual(merged2.Value(1), 1);
            Assert.IsFalse(merged2.IsDefinedOn(45));
            Assert.IsTrue(merged2.IsDefinedOn(60));
        }

        [TestMethod]
        public void InMiddleMerge()
        {
            List<IFunction<int, int>> list1 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(1, true, 2, false), 1),
                new Constant<int, int>(new Interval<int>(3, true, 4, false), 3),
            };

            List<IFunction<int, int>> list2 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(2, true, 3, false), 2)
            };

            Piecewise<int, int> piecewise1 = new Piecewise<int, int>(list1);
            Piecewise<int, int> piecewise2 = new Piecewise<int, int>(list2);

            Piecewise<int, int> merged1 = piecewise1.Merge(piecewise2);

            Assert.AreEqual(merged1.IntervalsCount, 3);
            Assert.AreEqual(merged1[2].Interval.Start.Position, 3);
            Assert.AreEqual(merged1.Value(2), 2);
            Assert.AreEqual(merged1[0].Interval.Start.Position, 1);
            Assert.AreEqual(merged1.Value(1), 1);
            Assert.AreEqual(merged1.Value(3), 3);
            Assert.IsFalse(merged1.IsDefinedOn(4));
            Assert.IsTrue(merged1.IsDefinedOn(2));

            Piecewise<int, int> merged2 = piecewise2.Merge(piecewise1);

            Assert.AreEqual(merged2.IntervalsCount, 3);
            Assert.AreEqual(merged2[2].Interval.Start.Position, 3);
            Assert.AreEqual(merged2.Value(2), 2);
            Assert.AreEqual(merged2[0].Interval.Start.Position, 1);
            Assert.AreEqual(merged2.Value(1), 1);
            Assert.AreEqual(merged2.Value(3), 3);
            Assert.IsFalse(merged2.IsDefinedOn(4));
            Assert.IsTrue(merged2.IsDefinedOn(2));
        }

        [TestMethod]
        public void SecondOnFirstMerge()
        {
            List<IFunction<int, int>> list1 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(1, true, 100, false), 1),
            };

            List<IFunction<int, int>> list2 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(2, true, 3, false), 2),
                new Constant<int, int>(new Interval<int>(6, true, 10, false), 2),
                new Constant<int, int>(new Interval<int>(45, true, 55, false), 2),
                new Constant<int, int>(new Interval<int>(75, false, 100, false), 2),
            };

            Piecewise<int, int> piecewise1 = new Piecewise<int, int>(list1);
            Piecewise<int, int> piecewise2 = new Piecewise<int, int>(list2);

            Piecewise<int, int> merged1 = piecewise1.Merge(piecewise2);

            Assert.AreEqual(merged1.IntervalsCount, 8);
            Assert.AreEqual(merged1.Value(1), 1);
            Assert.AreEqual(merged1.Value(3), 1);
            Assert.AreEqual(merged1.Value(15), 1);
            Assert.AreEqual(merged1.Value(75), 1);
            Assert.AreEqual(merged1.Value(2), 2);
            Assert.AreEqual(merged1.Value(7), 2);
            Assert.AreEqual(merged1.Value(50), 2);
            Assert.AreEqual(merged1.Value(99), 2);

            Assert.IsTrue(merged1[1].Interval.Equals(list2[0].Interval));
            Assert.IsTrue(merged1[3].Interval.Equals(list2[1].Interval));
            Assert.IsTrue(merged1[5].Interval.Equals(list2[2].Interval));
            Assert.IsTrue(merged1[7].Interval.Equals(list2[3].Interval));

            Piecewise<int, int> merged2 = piecewise2.Merge(piecewise1);

            Assert.AreEqual(merged2.IntervalsCount, 1);
            Assert.IsTrue(merged2[0].Interval.Equals(list1[0].Interval));
            Assert.AreEqual(merged2.Value(1), 1);
        }

        [TestMethod]
        public void IntersectMerge()
        {
            List<IFunction<int, int>> list1 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(1, true, 100, false), 1)
            };

            List<IFunction<int, int>> list2 = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(50, true, 300, false), 2)
            };

            Piecewise<int, int> piecewise1 = new Piecewise<int, int>(list1);
            Piecewise<int, int> piecewise2 = new Piecewise<int, int>(list2);

            Piecewise<int, int> merged1 = piecewise1.Merge(piecewise2);

            Assert.AreEqual(merged1.IntervalsCount, 2);
            Assert.AreEqual(merged1[0].Interval.Start.Position, 1);
            Assert.AreEqual(merged1[0].Interval.Start.Inclusive, true);
            Assert.AreEqual(merged1[0].Interval.End.Position, 50);
            Assert.AreEqual(merged1[0].Interval.End.Inclusive, false);
            Assert.AreEqual(merged1[1].Interval.Start.Position, 50);
            Assert.AreEqual(merged1[1].Interval.Start.Inclusive, true);
            Assert.AreEqual(merged1[1].Interval.End.Position, 300);
            Assert.AreEqual(merged1[1].Interval.End.Inclusive, false);
            Assert.AreEqual(merged1.Value(1),1);
            Assert.AreEqual(merged1.Value(50), 2);
            Assert.AreEqual(merged1.Value(100), 2);
            Assert.IsFalse(merged1.IsDefinedOn(0));
            Assert.IsFalse(merged1.IsDefinedOn(300));
            Assert.IsTrue(merged1.IsDefinedOn(1));
            Assert.IsTrue(merged1.IsDefinedOn(50));

            Piecewise<int, int> merged2 = piecewise2.Merge(piecewise1);

            Assert.AreEqual(merged2.IntervalsCount, 2);
            Assert.AreEqual(merged2[0].Interval.Start.Position, 1);
            Assert.AreEqual(merged2[0].Interval.Start.Inclusive, true);
            Assert.AreEqual(merged2[0].Interval.End.Position, 100);
            Assert.AreEqual(merged2[0].Interval.End.Inclusive, false);
            Assert.AreEqual(merged2[1].Interval.Start.Position, 100);
            Assert.AreEqual(merged2[1].Interval.Start.Inclusive, true);
            Assert.AreEqual(merged2[1].Interval.End.Position, 300);
            Assert.AreEqual(merged2[1].Interval.End.Inclusive, false);
            Assert.AreEqual(merged2.Value(1), 1);
            Assert.AreEqual(merged2.Value(50), 1);
            Assert.AreEqual(merged2.Value(100), 2);
            Assert.AreEqual(merged2.Value(299), 2);
            Assert.IsFalse(merged2.IsDefinedOn(0));
            Assert.IsFalse(merged2.IsDefinedOn(300));
            Assert.IsTrue(merged2.IsDefinedOn(1));
            Assert.IsTrue(merged2.IsDefinedOn(50));
        }
    }
}
