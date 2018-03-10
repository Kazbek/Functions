using System;
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
        public bool IsDefinedOn(TSpace point) => Interval.Contains(point);

        public bool TryUnion(IFunction<TSpace, TValue> function)
        {
            if (!(function is Constant<TSpace, TValue>) || !Interval.TryUnion(function.Interval))
                return false;
            if (!(_value is IEquatable<TValue> compare))
                return false;
            return compare.Equals(((Constant<TSpace, TValue>) function)._value as IEquatable<TValue>);
        }

        public bool TryUnion(IFunction<TSpace, TValue> function, out IFunction<TSpace, TValue> resultFunction)
        {
            if (!TryUnion(function))
            {
                resultFunction = null;
                return false;
            }
            resultFunction = UncheckedUnion(function);
            return true;
        }

        public IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function)
        {
            if (TryUnion(function))
                return UncheckedUnion(function);
            throw new Exception("It is not possible to combine these functions.");
        }

        private IFunction<TSpace, TValue> UncheckedUnion(IFunction<TSpace, TValue> function)
        {
            return new Constant<TSpace, TValue>(Interval.Union(function.Interval), _value);
        }

        public IFunction<TSpace, TValue> ShortenIntervalTo(IInterval<TSpace> interval)
        {
            if (Interval.Equals(interval))
                return this;
            if (Interval.Cover(interval))
                return new Constant<TSpace, TValue>(interval, _value);
            throw new ArgumentOutOfRangeException(nameof(interval));
        }

        public Constant(IInterval<TSpace> interval, TValue value)
        {
            Interval = interval;
            _value = value;
        }

        private Constant() { }
    }
}
