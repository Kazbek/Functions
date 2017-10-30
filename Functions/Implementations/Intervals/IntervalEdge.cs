using System;
using System.Collections.Generic;
using System.Text;
using Functions.Interfaces;

namespace Functions.Implementations.Intervals
{
    public class IntervalEdge<TSpace> : IIntervalEdge<TSpace> where TSpace : IComparable<TSpace>
    {
        public TSpace Position { get; }
        public bool Inclusive { get; }

        public IntervalEdge(TSpace position, bool inclusive)
        {
            if(position == null)
                throw new Exception("Position can`t be null.");
            Position = position;
            Inclusive = inclusive;
        }

        public int CompareTo(IIntervalEdge<TSpace> other) => Position.CompareTo(other.Position);
    }
}
