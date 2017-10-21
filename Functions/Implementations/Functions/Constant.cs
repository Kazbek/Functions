using System;
using System.Collections.Generic;
using System.Text;
using Functions.Interfaces;

namespace Functions.Implementations.Functions
{
    public class Constant<TSpace, TValue> : IFunction<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        private readonly TValue _value;
        public TValue Value(TSpace point)
        {
            if (Interval.Contains(point))
                return _value;
            throw new ArgumentOutOfRangeException();
        }

        public IInterval<TSpace> Interval { get; }
        public bool TryUnion(IFunction<TSpace, TValue> function)
        {
            throw new NotImplementedException();
        }

        public IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function)
        {
            throw new NotImplementedException();
        }

        public Constant(IInterval<TSpace> interval, TValue value)
        {
            Interval = interval;
            _value = value;
        }
    }
}
