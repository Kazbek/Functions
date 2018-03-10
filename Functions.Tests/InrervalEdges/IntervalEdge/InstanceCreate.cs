using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Intervals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.InrervalEdges.IntervalEdge
{
    [TestClass]
    public class InstanceCreate
    {
        [TestMethod]
        [DataRow(-233,false)]
        [DataRow(-1, true)]
        [DataRow(0,true)]
        [DataRow(1,false)]
        [DataRow(int.MaxValue,true)]
        [DataRow(int.MinValue,false)]
        public void IntFromDataRow(int value, bool inclusive)
        {
            IntervalEdge<int> edge = new IntervalEdge<int>(value, inclusive);
            Assert.AreEqual(edge.Position, value);
            Assert.AreEqual(edge.Inclusive, inclusive);
        }

        [TestMethod]
        public void ExceptionArgumentNull()
        {
            NullableInt nullPointer = null;
            Assert.ThrowsException<ArgumentNullException>(() => new IntervalEdge<NullableInt>(nullPointer, false));
            Assert.ThrowsException<ArgumentNullException>(() => new IntervalEdge<NullableInt>(nullPointer, true));
        }

        private class NullableInt : IComparable<NullableInt>
        {
            private int? _point;

            public NullableInt(int? point)
            {
                _point = point;
            }

            public int CompareTo(NullableInt other)
            {
                throw new NotImplementedException();
            }
        }

    }
}
