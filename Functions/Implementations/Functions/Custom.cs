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
            if (!(function is Custom<TSpace, TValue>) || !Interval.TryUnion(function.Interval))
                return false;
            Custom<TSpace, TValue> compare = (Custom<TSpace, TValue>) function;
            return _func.Equals(compare._func);
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

        /// <summary>
        /// Unite functions. Function object must be used from precompiled expression. If you use expression directly in constructor then equality method return false.
        /// </summary>
        /// <param name="function">Function for calculating TValue by a given TSpace</param>
        /// <returns></returns>
        public IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function)
        {
            if (TryUnion(function))
                return UncheckedUnion(function);
            throw new Exception("It is not possible to combine these functions.");
        }

        private IFunction<TSpace, TValue> UncheckedUnion(IFunction<TSpace, TValue> function)
        {
            return new Custom<TSpace, TValue>(Interval.Union(function.Interval), _func);
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
