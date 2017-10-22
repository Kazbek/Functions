using Functions.Implementations.Intervals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        public void Test1()
        {
            IntervalEdge<int> edge = new IntervalEdge<int>(3, false);
            Assert.AreEqual(edge.Position,3);
        }
    }
}
