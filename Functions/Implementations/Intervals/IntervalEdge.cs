using System;
using System.Collections.Generic;
using System.Text;

namespace Functions.Implementations.Intervals
{
    public class IntervalEdge<TSpace> : IComparable<IntervalEdge<TSpace>> where TSpace : IComparable<TSpace>
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

        public int CompareTo(IntervalEdge<TSpace> other)
        {
            int compare = Position.CompareTo(other.Position);
            if (compare != 0)
                return compare;
            if (Inclusive && !other.Inclusive)
                return 1;
            if (other.Inclusive && !Inclusive)
                return -1;
            return compare;
        }
    }
}
