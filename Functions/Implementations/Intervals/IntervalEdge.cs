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
        public bool Equals(IIntervalEdge<TSpace> other) => Position.CompareTo(other.Position) == 0 && Inclusive == other.Inclusive;

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TSpace>.Default.GetHashCode(Position) * 397) ^ Inclusive.GetHashCode();
            }
        }
    }
}
