using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Functions.Implementations.Aggregations;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Aggregations.Piecewise
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        public void CreateInt()
        {
            List<IFunction<int,int>> list = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(1, true, 7, false), 1),
                new Constant<int, int>(new Interval<int>(2, true, 5, true), 1),
                new Constant<int, int>(new Interval<int>(3, true, 9, false), 1),
                new Constant<int, int>(new Interval<int>(14, true, 17, false), 2),
                new Constant<int, int>(new Interval<int>(34, true, 39, false), 1),
                new Constant<int, int>(new Interval<int>(-3, true, -2, false), 9),
                new Constant<int, int>(new Interval<int>(-1, true, 0, false), 2),
                new Constant<int, int>(new Interval<int>(-3, true, -2, true), 9),
                new Constant<int, int>(new Interval<int>(-3, true, -3, true), 9)
            };
            Piecewise<int,int> piecewise = new Piecewise<int, int>(list);
            Assert.AreEqual(piecewise.Value(-1),2);
            Assert.AreEqual(piecewise.Value(1), 1);
            Assert.AreEqual(piecewise.Value(2), 1);
            Assert.AreEqual(piecewise.Value(3), 1);
            Assert.AreEqual(piecewise.Value(8), 1);
            Assert.AreEqual(piecewise.Value(-3), 9);
            Assert.AreEqual(piecewise.Value(-2), 9);
            Assert.AreEqual(piecewise.Value(36), 1);

            Assert.AreEqual(piecewise.IntervalsCount, 5);
            Assert.AreEqual(piecewise[0].Interval.Start.Position, -3);
            Assert.AreEqual(piecewise[0].Interval.Start.Inclusive, true);
            Assert.AreEqual(piecewise[0].Interval.End.Position, -2);
            Assert.AreEqual(piecewise[0].Interval.End.Inclusive, true);

            Assert.IsTrue(piecewise.IsDefinedOn(1));

            Assert.IsFalse(piecewise.IsDefinedOn(9));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => piecewise.Value(9));
        }
    }
}
