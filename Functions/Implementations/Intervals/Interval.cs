using System;
using Functions.Interfaces;

namespace Functions.Implementations.Intervals
{
    public class Interval<TSpace> : IInterval<TSpace> where TSpace : IComparable<TSpace>
    {
        public IntervalEdge<TSpace> Start { get; }
        public IntervalEdge<TSpace> End { get; }
        public bool Contains(TSpace point)
        {
            return point.CompareTo(Start.Position) > 0 && point.CompareTo(End.Position) < 0 || Start.Inclusive && point.CompareTo(Start.Position) == 0 || End.Inclusive && point.CompareTo(End.Position) == 0;
        }

        public bool TryUnion(IInterval<TSpace> interval)
        {
            return Start.CompareTo(interval.End) > 0 || interval.Start.CompareTo(End) < 0 || (Start.Inclusive || interval.End.Inclusive) && Start.CompareTo(interval.End) == 0 || (End.Inclusive || interval.Start.Inclusive) && interval.Start.CompareTo(End) == 0;
        }

        public IInterval<TSpace> Union(IInterval<TSpace> interval)
        {
            throw new NotImplementedException();
        }

        public Interval(TSpace start, bool inclusiveStart, TSpace end, bool inclusiveEnd)
        {
            if(start == null || end == null || start.CompareTo(end) > 0 || start.CompareTo(end) == 0 && inclusiveStart && inclusiveEnd)
                throw new Exception("Invalid arguments. They can`t be null and end must be not less than start.");
            Start = new IntervalEdge<TSpace>(start, inclusiveStart);
            End = new IntervalEdge<TSpace>(end, inclusiveEnd);
        }
        public Interval(IntervalEdge<TSpace> start, IntervalEdge<TSpace> end)
        {
            if (start.CompareTo(end) > 0 || start.CompareTo(end) == 0 && start.Inclusive && end.Inclusive)
                throw new Exception("End must be not less than start.");
            Start = start;
            End = end;
        }
    }
}
