using System;
using Functions.Implementations.Comparators;
using Functions.Interfaces;

namespace Functions.Implementations.Utils
{
    internal class OrderedAccessToArrays<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        private readonly IFunction<TSpace, TValue>[] _first, _second;
        private readonly int _firstCount;
        private readonly int _secondCount;
        private int _firstIndex = 0;
        private int _secondIndex = 0;
        private readonly FunctionIntervalComparer<TSpace, TValue> _functionIntervalComparer = new FunctionIntervalComparer<TSpace, TValue>();
        public OrderedAccessToArrays(IFunction<TSpace, TValue>[] first, IFunction<TSpace, TValue>[] second)
        {
            _firstCount = first.Length;
            _secondCount = second.Length;
            _first = first;
            _second = second;
        }

        public OrderedAccessElement<TSpace, TValue> GetNext()
        {
            if(_firstIndex >= _firstCount && _secondIndex >= _secondCount)
                throw new InvalidOperationException("All objects already returned.");
            if (_firstIndex >= _firstCount)
                return new OrderedAccessElement<TSpace, TValue>(false, _second[_secondIndex++]);
            if (_secondIndex >= _secondCount)
                return new OrderedAccessElement<TSpace, TValue>(true, _first[_firstIndex++]);

            if (_functionIntervalComparer.Compare(_first[_firstIndex], _second[_secondIndex]) < 0)
                return new OrderedAccessElement<TSpace, TValue>(true, _first[_firstIndex++]);
            return new OrderedAccessElement<TSpace, TValue>(false, _second[_secondIndex++]);
        }
    }

    internal struct OrderedAccessElement<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        internal readonly bool IsFirstArrayElement;
        internal readonly IFunction<TSpace, TValue> Function;

        internal OrderedAccessElement(bool isFirstArrayElement, IFunction<TSpace, TValue> function)
        {
            IsFirstArrayElement = isFirstArrayElement;
            Function = function;
        }
    }
}
