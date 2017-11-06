using System;
using System.Collections.Generic;
using Functions.Implementations.Comparators;
using Functions.Implementations.Intervals;
using Functions.Interfaces;

namespace Functions.Implementations.Functions
    {
    /// <summary>
    /// Представляет собой сложную функцию, состоящую из нескольких других, идущих друг за другом без промежутков в интервалах.
    /// </summary>
    /// <typeparam name="TSpace"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Composite<TSpace, TValue> : IFunction<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        private readonly IFunction<TSpace, TValue>[] _functions;
        public TValue Value(TSpace point)
        {
            if(!Interval.Contains(point))
                throw new ArgumentOutOfRangeException();
            return _functions[Utils.Utils.InretvalBinarySearch(_functions, point)].Value(point);
        }

        public IInterval<TSpace> Interval { get; }
        public bool IsDefinedOn(TSpace point) => Interval.Contains(point);

        public bool TryUnion(IFunction<TSpace, TValue> function)
        {
            return function is Composite<TSpace, TValue> && Interval.IsAdjacent(function.Interval);
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
            if(!TryUnion(function))
                throw new Exception("It is not possible to combine these functions.");
            return UncheckedUnion(function);
        }

        private IFunction<TSpace, TValue> UncheckedUnion(IFunction<TSpace, TValue> function)
        {
            Composite<TSpace, TValue> composite = function as Composite<TSpace, TValue>;
            Composite<TSpace, TValue> first, second;
            if (Interval.Start.CompareTo(function.Interval.Start) > 0)
            {
                first = (Composite<TSpace, TValue>)function;
                second = this;
            }
            else
            {
                first = this;
                second = (Composite<TSpace, TValue>)function;
            }
            int firstCount = first._functions.Length;
            int secondCount = second._functions.Length;
            IFunction<TSpace, TValue>[] newFunctions = new IFunction<TSpace, TValue>[firstCount + secondCount - (first._functions[firstCount - 1].TryUnion(second._functions[0]) ? 1 : 0)];
            first._functions.CopyTo(newFunctions, 0);
            if (first._functions[firstCount - 1].TryUnion(second._functions[0]))
            {
                second._functions.CopyTo(newFunctions, firstCount - 1);
                newFunctions[firstCount - 1] = first._functions[firstCount - 1].Union(second._functions[0]);
            }
            else
            {
                second._functions.CopyTo(newFunctions, firstCount);
            }
            return new Composite<TSpace, TValue>(newFunctions);
        }

        public IFunction<TSpace, TValue> ShortenIntervalTo(IInterval<TSpace> interval)
        {
            if (Interval.Equals(interval))
                return this;
            if (!Interval.Cover(interval))
                throw new ArgumentOutOfRangeException(nameof(interval));
            int firstIndex, lastIndex;
            int count = _functions.Length;

            if (_functions[0].Interval.Start.CompareTo(interval.Start) == 0)
            {
                firstIndex = 0;
            }
            else
            {
                firstIndex = Utils.Utils.InretvalBinarySearch(_functions, interval.Start.Position);
                if (!interval.Start.Inclusive && _functions[firstIndex].Interval.End.CompareTo(interval.Start) == 0)
                {
                    firstIndex++;
                    
                }
            }

            if (_functions[count-1].Interval.End.CompareTo(interval.End) == 0)
            {
                lastIndex = count-1;
            }
            else
            {
                lastIndex = Utils.Utils.InretvalBinarySearch(_functions, interval.End.Position);
                if (!interval.End.Inclusive && _functions[firstIndex].Interval.Start.CompareTo(interval.End) == 0)
                {
                    lastIndex--;
                }
            }
            count = lastIndex - firstIndex + 1;
            IFunction<TSpace, TValue>[] newFunctions = new IFunction<TSpace, TValue>[count];
            newFunctions[0] = _functions[firstIndex].ShortenIntervalTo(new Interval<TSpace>(interval.Start,_functions[firstIndex].Interval.End));
            if(count > 1)
                newFunctions[count-1] = _functions[lastIndex].ShortenIntervalTo(new Interval<TSpace>(_functions[lastIndex].Interval.Start, interval.End));
            if(count > 2)
                Array.Copy(_functions, firstIndex, newFunctions, 1, count-2);

            return new Composite<TSpace, TValue>(newFunctions);
        }

        /// <summary>
        /// Создаёт сложную функцию, состоящую из нескольких других, объединенных на непрерывном интервале.
        /// </summary>
        public Composite(List<IFunction<TSpace, TValue>> functions)
        {
            int functionsCount = functions?.Count ?? 0;
            if (functionsCount == 0)
                throw new Exception("Can not create from an empty list.");
            functions.Sort(new FunctionIntervalComparer<TSpace, TValue>());
            _functions = new IFunction<TSpace, TValue>[functionsCount];
            
            IFunction<TSpace, TValue> function = functions[0];
            IFunction<TSpace, TValue> lastAddedFunction = function;
            IInterval<TSpace> prevInterval = function.Interval;

            int lastRealIndex = 0;
            _functions[0] = function;
            
            for (int i = 1; i < functionsCount; i++)
            {
                function = functions[i];
                if (!prevInterval.IsAdjacentRight(function.Interval))
                    throw new Exception("Intervals do not go one by one.");
                prevInterval = function.Interval;
                if (lastAddedFunction.TryUnion(function, out var unitedFunction))
                {
                    lastAddedFunction = unitedFunction;
                }
                else
                {
                    lastAddedFunction = function;
                    ++lastRealIndex;
                }
                _functions[lastRealIndex] = lastAddedFunction;
            }
            Interval = new Interval<TSpace>(_functions[0].Interval.Start, _functions[lastRealIndex].Interval.End);
            Array.Resize(ref _functions, ++lastRealIndex);
        }

        private Composite(IFunction<TSpace, TValue>[] array)
        {
            _functions = array;
            Interval = new Interval<TSpace>(array[0].Interval.Start, array[array.Length-1].Interval.End);
        }
    }
}
