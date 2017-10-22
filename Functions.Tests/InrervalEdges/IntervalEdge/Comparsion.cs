using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Intervals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.InrervalEdges.IntervalEdge
{
    [TestClass]
    public class Comparsion
    {
        [TestMethod]
        [DataRow(3,true, 2, true)]
        [DataRow(3, true, 2, false)]
        [DataRow(3, false, 2, true)]
        [DataRow(3, false, 2, false)]
        [DataRow(-56, false, -332, false)]
        public void CompareToLess(int point1, bool inclusive1, int point2, bool inclusive2)
        {
            IntervalEdge<int> edge1 = new IntervalEdge<int>(point1, inclusive1);
            IntervalEdge<int> edge2 = new IntervalEdge<int>(point2, inclusive2);
            Assert.IsTrue(edge1.CompareTo(edge2) > 0);
            Assert.IsTrue(edge2.CompareTo(edge1) < 0);
        }
        [TestMethod]
        [DataRow(3, true, 3, true)]
        [DataRow(3, true, 3, false)]
        [DataRow(3, false, 3, true)]
        [DataRow(3, false, 3, false)]
        [DataRow(-231, false, -231, false)]
        public void CompareToEqual(int point1, bool inclusive1, int point2, bool inclusive2)
        {
            IntervalEdge<int> edge1 = new IntervalEdge<int>(point1, inclusive1);
            IntervalEdge<int> edge2 = new IntervalEdge<int>(point2, inclusive2);
            Assert.IsTrue(edge1.CompareTo(edge2) == 0);
            Assert.IsTrue(edge2.CompareTo(edge1) == 0);
        }
    }
}
