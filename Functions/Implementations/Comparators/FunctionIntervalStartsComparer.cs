using System;
using System.Collections.Generic;
using System.Text;
using Functions.Interfaces;

namespace Functions.Implementations.Comparators
{
    public class FunctionIntervalStartsComparer<TSpace, TValue> : IComparer<IFunction<TSpace, TValue>> where TSpace : IComparable<TSpace>
    {
        public int Compare(IFunction<TSpace, TValue> x, IFunction<TSpace, TValue> y)
        {
            int compare = x.Interval.Start.CompareTo(y.Interval.Start);
            if (compare != 0)
                return compare;
            if (x.Interval.Start.Inclusive == y.Interval.Start.Inclusive)
                return 0;
            if (x.Interval.Start.Inclusive)
                return -1;
            return 1;
        }
    }
}
