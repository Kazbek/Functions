using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Intervals;

namespace Functions.Interfaces
{
    public interface IInterval<TSpace> where TSpace : IComparable<TSpace>
    {
        IIntervalEdge<TSpace> Start { get; }
        IIntervalEdge<TSpace> End { get; }
        bool Contains(TSpace point);
        bool Intersect(IInterval<TSpace> interval);
        bool Cover(IInterval<TSpace> interval);
        bool IsAdjacent(IInterval<TSpace> interval);
        bool IsAdjacentRight(IInterval<TSpace> interval);
        bool IsAdjacentLeft(IInterval<TSpace> interval);
        bool TryUnion(IInterval<TSpace> interval);
        IInterval<TSpace> Union(IInterval<TSpace> interval);
    }
}
