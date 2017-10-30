using System;
using Functions.Interfaces;

namespace Functions.Implementations.Functions
{
    public class Custom<TSpace, TValue> : IFunction<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        private readonly Func<TSpace, TValue> _func;
        public TValue Value(TSpace point)
        {
            if (Interval.Contains(point))
                return _func.Invoke(point);
            throw new ArgumentOutOfRangeException();
        }

        public IInterval<TSpace> Interval { get; }
        public bool IsDefinedOn(TSpace point) => Interval.Contains(point);

        public bool TryUnion(IFunction<TSpace, TValue> function)
        {
            if (!(_func is IEquatable<Func<TSpace, TValue>>) || !Interval.TryUnion(function.Interval))
                return false;
            IEquatable<Func<TSpace, TValue>> compare = _func as IEquatable<Func<TSpace, TValue>>;
            return compare.Equals(((Custom<TSpace, TValue>)function)._func as IEquatable<Func<TSpace, TValue>>);
        }

        public IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function)
        {
            if (TryUnion(function))
                return new Custom<TSpace, TValue>(Interval.Union(function.Interval), _func);
            throw new Exception("It is not possible to combine these functions.");
        }

        public IFunction<TSpace, TValue> ShortenIntervalTo(IInterval<TSpace> interval)
        {
            if (Interval.Equals(interval))
                return this;
            if (Interval.Cover(interval))
                return new Custom<TSpace, TValue>(interval, _func);
            throw new ArgumentOutOfRangeException(nameof(interval));
        }

        public Custom(IInterval<TSpace> inteval, Func<TSpace, TValue> func)
        {
            if (inteval == null || func == null)
                throw new ArgumentNullException(inteval == null ? nameof(inteval) : nameof(func));
            _func = func;
            Interval = inteval;
        }
    }
}
