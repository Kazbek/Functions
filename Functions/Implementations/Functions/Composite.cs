using System;
using System.Collections.Generic;
using System.Linq;
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
            return _functions[InretvalBinarySearch(_functions, point)].Value(point);
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
        /// <summary>
        /// Создаёт сложную функцию, состоящую из нескольких других, объединенных на непрерывном интервале.
        /// </summary>
        /// <param name="functions">Функции, из которых будет состоять создаваемая. Должны быть упорядочены по интервалам, не иметь разрывов между собой, а также не перечекаться.</param>
        public Composite(List<IFunction<TSpace, TValue>> functions)
        {
            int functionsCount = functions.Count;
            
            if(functions == null || functionsCount == 0)
                throw new Exception("Can not create from an empty list.");

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
                if (lastAddedFunction.TryUnion(function))
                {
                    lastAddedFunction = lastAddedFunction.Union(function);
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

        private static int InretvalBinarySearch(IFunction<TSpace, TValue>[] array, TSpace point)
        {
            int low = 0; // 0 is always going to be the first element
            int high = array.Length - 1; // Find highest element
            int middle = (low + high + 1) / 2; // Find middle element
            int location = -1; // Return value -1 if not found
            
            do
            {
                IFunction<TSpace, TValue> middleElement = array[middle];
                
                int compare = point.CompareTo(middleElement.Interval.Start.Position);
                if (compare < 0 || compare == 0 && !middleElement.Interval.Start.Inclusive)
                {
                    high = middle - 1;
                    middle = (low + high + 1) / 2;
                    continue;
                }

                compare = point.CompareTo(middleElement.Interval.End.Position);
                if (compare > 0 || compare == 0 && !middleElement.Interval.End.Inclusive)
                {
                    low = middle + 1;
                    middle = (low + high + 1) / 2;
                    continue;
                }

                if (middleElement.Interval.Contains(point))
                {
                    location = middle;
                    break;
                }
            } while (low <= high);

            return location;
        }
    }
}
