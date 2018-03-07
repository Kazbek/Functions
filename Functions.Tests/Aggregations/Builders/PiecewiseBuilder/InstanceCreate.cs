using System.Collections.Generic;
using Functions.Implementations.Aggregations.Builders;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Functions.Implementations.Aggregations;

namespace Functions.Tests.Aggregations.Builders.PiecewiseBuilder
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        public void SimpleListIntConstructor()
        {
            List<IFunction<int, int>> functions = new List<IFunction<int, int>>
            {
                new Constant<int, int>(new Interval<int>(1,true, 10, true), 3)
            };

            PiecewiseBuilder<int,int> pb = new PiecewiseBuilder<int, int>(functions);
            Piecewise<int, int> p = pb.ToPiecewise();

            Assert.AreEqual(1, p.IntervalsCount);
            Assert.AreEqual(1, p[0].Interval.Start.Position);
            Assert.AreEqual(10, p[0].Interval.End.Position);
            Assert.AreEqual(true, p[0].Interval.Start.Inclusive);
            Assert.AreEqual(true, p[0].Interval.End.Inclusive);
            Assert.AreEqual(3, p[0].Value(5));
        }
    }
}
