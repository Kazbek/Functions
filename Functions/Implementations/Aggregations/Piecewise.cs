using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Comparators;
using Functions.Implementations.Intervals;
using Functions.Interfaces;

namespace Functions.Implementations.Aggregations
{
    public class Piecewise<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        private readonly IFunction<TSpace, TValue>[] _functions;

        public TValue Value(TSpace point)
        { 
            int position = Utils.Utils.InretvalBinarySearch(_functions, point);
            if(position == -1)
                throw new ArgumentOutOfRangeException();
            return _functions[position].Value(point);
        }

        public bool IsDefinedOn(TSpace point) => Utils.Utils.InretvalBinarySearch(_functions, point) != -1;

        public IFunction<TSpace, TValue> this[int index] => _functions[index]; //readonly
        public int IntervalsCount => _functions.Length;

        public Piecewise(List<IFunction<TSpace, TValue>> functions)
        {
            if (functions == null)
                throw new Exception("List of functions can`t be null.");

            int functionsCount = functions.Count;
            _functions = new IFunction<TSpace, TValue>[functionsCount];

            if (functionsCount == 0)
                return;

            functions.Sort(new FunctionIntervalStartsComparer<TSpace, TValue>());
            
            IFunction<TSpace, TValue> function = functions[0];
            IFunction<TSpace, TValue> lastAddedFunction = function;
            IInterval<TSpace> prevInterval = function.Interval;

            int lastRealIndex = 0;
            _functions[0] = function;

            for (int i = 1; i < functionsCount; i++)
            {
                function = functions[i];
                if (prevInterval.Intersect(function.Interval) && !lastAddedFunction.TryUnion(function))
                    throw new Exception("Not combinable functions have intersected intervals.");
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
            Array.Resize(ref _functions, ++lastRealIndex);
        }
    }
}
