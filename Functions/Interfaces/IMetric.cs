using System;

namespace Functions.Interfaces
{
    public interface IMetric<in TSpace, out TMetric>
    {
        TMetric GetMetric(TSpace point1, TSpace point2);
    }

    public interface IIntervalMetric<TSpace, out TMetric> : IMetric<TSpace, TMetric>
        where TSpace : IComparable<TSpace>
    {
        TMetric GetMetric(IInterval<TSpace> interval);
    }
}
