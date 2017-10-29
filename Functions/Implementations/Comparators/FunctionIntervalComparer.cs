using System;
using System.Collections.Generic;
using System.Text;
using Functions.Interfaces;

namespace Functions.Implementations.Comparators
{
    public class FunctionIntervalComparer<TSpace, TValue> : IComparer<IFunction<TSpace, TValue>> where TSpace : IComparable<TSpace>
    {
        public int Compare(IFunction<TSpace, TValue> x, IFunction<TSpace, TValue> y)
        {
            int compare = x.Interval.Start.CompareTo(y.Interval.Start);
            if (compare != 0)
                return compare;
            compare = x.Interval.Start.Inclusive.CompareTo(y.Interval.Start.Inclusive);
            if (compare != 0)
                return compare;

            compare = y.Interval.End.CompareTo(y.Interval.End);
            if (compare != 0)
                return compare;
            compare = x.Interval.End.Inclusive.CompareTo(y.Interval.End.Inclusive);
            return compare;
        }
    }
}
