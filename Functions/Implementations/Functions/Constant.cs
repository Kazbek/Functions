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
            if (!(function is Constant<TSpace, TValue>) || !Interval.TryUnion(function.Interval))
                return false;
            IEquatable<TValue> compare = _value as IEquatable<TValue>;
            if (compare == null)
                return false;
            return compare.Equals(((Constant<TSpace, TValue>) function)._value as IEquatable<TValue>);
        }

        public IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function)
        {
            if(TryUnion(function))
                return new Constant<TSpace, TValue>(Interval.Union(function.Interval), _value);
            throw new Exception("It is not possible to combine these functions.");
        }

        public Constant(IInterval<TSpace> interval, TValue value)
        {
            Interval = interval;
            _value = value;
        }
    }
}
