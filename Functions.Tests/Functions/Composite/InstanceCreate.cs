using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Functions.Composite
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        public void SingleIntForce()
        {
            List<IFunction<int, int>> constants = new List<IFunction<int, int>>();
            constants.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            IFunction<int, int> composite = new Composite<int, int>(constants);

            Assert.AreEqual(composite.Interval.Start.Position.CompareTo(1) == 0, true);
            Assert.AreEqual(composite.Interval.Start.Inclusive, true);

            Assert.AreEqual(composite.Interval.End.Position.CompareTo(2) == 0, true);
            Assert.AreEqual(composite.Interval.End.Inclusive, false);

            Assert.AreEqual(composite.Value(1), 1);
        }

        [TestMethod]
        public void ThreeIntForce()
        {
            List<IFunction<int, int>> constants = new List<IFunction<int, int>>();
            constants.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            constants.Add(new Constant<int, int>(new Interval<int>(2, true, 3, false), 2));
            constants.Add(new Constant<int, int>(new Interval<int>(3, true, 4, false), 3));
            IFunction<int, int> composite = new Composite<int, int>(constants);

            Assert.AreEqual(composite.Interval.Start.Position.CompareTo(1) == 0, true);
            Assert.AreEqual(composite.Interval.Start.Inclusive, true);

            Assert.AreEqual(composite.Interval.End.Position.CompareTo(4) == 0, true);
            Assert.AreEqual(composite.Interval.End.Inclusive, false);

            Assert.AreEqual(composite.Value(1), 1);
            Assert.AreEqual(composite.Value(2), 2);
            Assert.AreEqual(composite.Value(3), 3);
        }

        [TestMethod]
        public void ThreeUnorderedIntForce()
        {
            List<IFunction<int, int>> constants = new List<IFunction<int, int>>();
            constants.Add(new Constant<int, int>(new Interval<int>(2, true, 3, false), 2));
            constants.Add(new Constant<int, int>(new Interval<int>(3, true, 4, false), 3));
            constants.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            IFunction<int, int> composite = new Composite<int, int>(constants);

            Assert.AreEqual(composite.Interval.Start.Position.CompareTo(1) == 0, true);
            Assert.AreEqual(composite.Interval.Start.Inclusive, true);

            Assert.AreEqual(composite.Interval.End.Position.CompareTo(4) == 0, true);
            Assert.AreEqual(composite.Interval.End.Inclusive, false);

            Assert.AreEqual(composite.Value(1), 1);
            Assert.AreEqual(composite.Value(2), 2);
            Assert.AreEqual(composite.Value(3), 3);
        }
    }
}
