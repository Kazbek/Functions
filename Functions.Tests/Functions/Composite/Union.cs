using System.Collections.Generic;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Functions.Composite
{
    [TestClass]
    public class Union
    {
        [TestMethod]
        public void TryUnion_True()
        {
            List<IFunction<int, int>> constants1 = new List<IFunction<int, int>>();
            constants1.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            constants1.Add(new Constant<int, int>(new Interval<int>(2, true, 3, false), 2));
            constants1.Add(new Constant<int, int>(new Interval<int>(3, true, 4, false), 3));
            IFunction<int, int> composite1 = new Composite<int, int>(constants1);

            List<IFunction<int, int>> constants2 = new List<IFunction<int, int>>();
            constants2.Add(new Constant<int, int>(new Interval<int>(4, true, 5, false), 1));
            constants2.Add(new Constant<int, int>(new Interval<int>(5, true, 6, false), 2));
            constants2.Add(new Constant<int, int>(new Interval<int>(6, true, 7, false), 3));
            IFunction<int, int> composite2 = new Composite<int, int>(constants2);

            Assert.AreEqual(composite1.TryUnion(composite2), true);
            Assert.AreEqual(composite2.TryUnion(composite1), true);
        }

        [TestMethod]
        public void TryUnion_False()
        {
            List<IFunction<int, int>> constants1 = new List<IFunction<int, int>>();
            constants1.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            constants1.Add(new Constant<int, int>(new Interval<int>(2, true, 3, false), 2));
            constants1.Add(new Constant<int, int>(new Interval<int>(3, true, 4, true), 3));
            IFunction<int, int> composite1 = new Composite<int, int>(constants1);

            List<IFunction<int, int>> constants2 = new List<IFunction<int, int>>();
            constants2.Add(new Constant<int, int>(new Interval<int>(4, true, 5, false), 1));
            constants2.Add(new Constant<int, int>(new Interval<int>(5, true, 6, false), 2));
            constants2.Add(new Constant<int, int>(new Interval<int>(6, true, 7, false), 3));
            IFunction<int, int> composite2 = new Composite<int, int>(constants2);

            Assert.AreEqual(composite1.TryUnion(composite2), false);
            Assert.AreEqual(composite2.TryUnion(composite1), false);
        }

        [TestMethod]
        public void Union_WithElementsUnion()
        {
            List<IFunction<int, int>> constants1 = new List<IFunction<int, int>>();
            constants1.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            constants1.Add(new Constant<int, int>(new Interval<int>(2, true, 3, false), 2));
            constants1.Add(new Constant<int, int>(new Interval<int>(3, true, 4, false), 3));
            IFunction<int, int> composite1 = new Composite<int, int>(constants1);

            List<IFunction<int, int>> constants2 = new List<IFunction<int, int>>();
            constants2.Add(new Constant<int, int>(new Interval<int>(4, true, 5, false), 3));
            constants2.Add(new Constant<int, int>(new Interval<int>(5, true, 6, false), 2));
            constants2.Add(new Constant<int, int>(new Interval<int>(6, true, 7, false), 3));
            IFunction<int, int> composite2 = new Composite<int, int>(constants2);

            IFunction<int, int> united1 = composite1.Union(composite2);
            IFunction<int, int> united2 = composite2.Union(composite1);

            Assert.AreEqual(united1.Interval.Start.Position, 1);
            Assert.AreEqual(united1.Interval.Start.Inclusive, true);
            Assert.AreEqual(united1.Interval.End.Position, 7);
            Assert.AreEqual(united1.Interval.End.Inclusive, false);
            Assert.AreEqual(united1.Value(1), 1);
            Assert.AreEqual(united1.Value(2), 2);
            Assert.AreEqual(united1.Value(3), 3);
            Assert.AreEqual(united1.Value(4), 3);
            Assert.AreEqual(united1.Value(5), 2);
            Assert.AreEqual(united1.Value(6), 3);

            Assert.AreEqual(united2.Interval.Start.Position, 1);
            Assert.AreEqual(united2.Interval.Start.Inclusive, true);
            Assert.AreEqual(united2.Interval.End.Position, 7);
            Assert.AreEqual(united2.Interval.End.Inclusive, false);
            Assert.AreEqual(united2.Value(1), 1);
            Assert.AreEqual(united2.Value(2), 2);
            Assert.AreEqual(united2.Value(3), 3);
            Assert.AreEqual(united2.Value(4), 3);
            Assert.AreEqual(united2.Value(5), 2);
            Assert.AreEqual(united2.Value(6), 3);
        }

        [TestMethod]
        public void Union_WithoutElementsUnion()
        {
            List<IFunction<int, int>> constants1 = new List<IFunction<int, int>>();
            constants1.Add(new Constant<int, int>(new Interval<int>(1, true, 2, false), 1));
            constants1.Add(new Constant<int, int>(new Interval<int>(2, true, 3, false), 2));
            constants1.Add(new Constant<int, int>(new Interval<int>(3, true, 4, false), 3));
            IFunction<int, int> composite1 = new Composite<int, int>(constants1);

            List<IFunction<int, int>> constants2 = new List<IFunction<int, int>>();
            constants2.Add(new Constant<int, int>(new Interval<int>(4, true, 5, false), 4));
            constants2.Add(new Constant<int, int>(new Interval<int>(5, true, 6, false), 2));
            constants2.Add(new Constant<int, int>(new Interval<int>(6, true, 7, true), 3));
            IFunction<int, int> composite2 = new Composite<int, int>(constants2);

            IFunction<int, int> united1 = composite1.Union(composite2);
            IFunction<int, int> united2 = composite2.Union(composite1);

            Assert.AreEqual(united1.Interval.Start.Position, 1);
            Assert.AreEqual(united1.Interval.Start.Inclusive, true);
            Assert.AreEqual(united1.Interval.End.Position, 7);
            Assert.AreEqual(united1.Interval.End.Inclusive, true);
            Assert.AreEqual(united1.Value(1), 1);
            Assert.AreEqual(united1.Value(2), 2);
            Assert.AreEqual(united1.Value(3), 3);
            Assert.AreEqual(united1.Value(4), 4);
            Assert.AreEqual(united1.Value(5), 2);
            Assert.AreEqual(united1.Value(6), 3);
            Assert.AreEqual(united1.Value(7), 3);

            Assert.AreEqual(united2.Interval.Start.Position, 1);
            Assert.AreEqual(united2.Interval.Start.Inclusive, true);
            Assert.AreEqual(united2.Interval.End.Position, 7);
            Assert.AreEqual(united2.Interval.End.Inclusive, true);
            Assert.AreEqual(united2.Value(1), 1);
            Assert.AreEqual(united2.Value(2), 2);
            Assert.AreEqual(united2.Value(3), 3);
            Assert.AreEqual(united2.Value(4), 4);
            Assert.AreEqual(united2.Value(5), 2);
            Assert.AreEqual(united2.Value(6), 3);
            Assert.AreEqual(united2.Value(7), 3);
        }
    }
}
