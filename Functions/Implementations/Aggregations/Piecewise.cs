using System;
using System.Collections.Generic;
using System.Text;
using Functions.Implementations.Comparators;
using Functions.Implementations.Intervals;
using Functions.Implementations.Utils;
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

        public Piecewise<TSpace, TValue> Merge(Piecewise<TSpace, TValue> piecewise)
        {
            if(_functions.Length == 0)
                return new Piecewise<TSpace, TValue>((IFunction<TSpace, TValue>[])_functions.Clone());
            if(piecewise.IntervalsCount == 0)
                return new Piecewise<TSpace, TValue>((IFunction<TSpace, TValue>[])piecewise._functions.Clone());

            IFunction<TSpace, TValue>[] first, second, result;

            if (_functions[0].Interval.Start.CompareTo(piecewise[0].Interval.Start) > 0)
            {
                first = _functions;
                second = piecewise._functions;
            }
            else
            {
                first = piecewise._functions;
                second = _functions;
            }
            int firstCount = first.Length;
            int secondCount = second.Length;

            IFunction<TSpace, TValue>[] newFunctions;

            int compare = first[firstCount - 1].Interval.End.CompareTo(second[0].Interval.Start);
            if ( compare > 0 || compare == 0 && first[firstCount - 1].Interval.End.Inclusive != second[0].Interval.Start.Inclusive)
            {
                newFunctions = new IFunction<TSpace, TValue>[firstCount + secondCount - (first[firstCount - 1].TryUnion(second[0]) ? 1 : 0)];
                first.CopyTo(newFunctions, 0);
                if (first[firstCount - 1].TryUnion(second[0]))
                {
                    second.CopyTo(newFunctions, firstCount - 1);
                    newFunctions[firstCount - 1] = first[firstCount - 1].Union(second[0]);
                }
                else
                {
                    second.CopyTo(newFunctions, firstCount);
                }
                return new Piecewise<TSpace, TValue>(newFunctions);
            }
            int cycles = firstCount + secondCount;
            newFunctions = new IFunction<TSpace, TValue>[cycles * 2 - 1]; //trust me, I`m an engineer; example 1 + n, where second divide first, n -> inf make n*2-1 and its max.

            OrderedAccessToArrays<TSpace, TValue> arrays = new OrderedAccessToArrays<TSpace, TValue>(_functions, piecewise._functions); //This is important from which in which
            IFunction<TSpace, TValue> lastFunction = arrays.GetNext().Function;
            newFunctions[0] = lastFunction;
            int lastFunctionIndex = 0;
            
            for (int i = 1; i < cycles; i++)
            {
                (bool isFirstArrayElement, IFunction<TSpace, TValue> function) tuple = arrays.GetNext(); //If this and the previous begin with one point, then the interval of this will not be shorter than the previous one
                if (!lastFunction.Interval.Intersect(tuple.function.Interval))
                {
                    lastFunction = tuple.function;
                    newFunctions[++lastFunctionIndex] = lastFunction;
                    continue;
                }
                if (tuple.isFirstArrayElement)
                {
                    if(lastFunction.Interval.Cover(tuple.function.Interval))
                        continue;
                }
                else
                {
                    
                }
            }

            //Array.Resize(ref newFunctions, 0);
            return null;
        }

        private Piecewise(IFunction<TSpace, TValue>[] functions)
        {
            _functions = functions;
        }

        public Piecewise(List<IFunction<TSpace, TValue>> functions)
        {
            if (functions == null)
                throw new Exception("List of functions can`t be null.");

            int functionsCount = functions.Count;
            _functions = new IFunction<TSpace, TValue>[functionsCount];

            if (functionsCount == 0)
                return;

            functions.Sort(new FunctionIntervalComparer<TSpace, TValue>());
            
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
