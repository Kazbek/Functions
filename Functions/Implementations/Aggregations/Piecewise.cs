using System;
using System.Collections.Generic;
using System.Linq;
using Functions.Implementations.Aggregations.Builders;
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

            IFunction<TSpace, TValue>[] first, second;

            if (_functions[0].Interval.Start.CompareTo(piecewise[0].Interval.Start) > 0)
            {
                first = piecewise._functions;
                second = _functions;
            }
            else
            {
                first = _functions;
                second = piecewise._functions;
            }
            int firstCount = first.Length;
            int secondCount = second.Length;

            IFunction<TSpace, TValue>[] newFunctions;

            int compare = first[firstCount - 1].Interval.End.CompareTo(second[0].Interval.Start);
            if ( compare < 0 || compare == 0 && first[firstCount - 1].Interval.End.Inclusive != second[0].Interval.Start.Inclusive)
            {
                newFunctions = new IFunction<TSpace, TValue>[firstCount + secondCount - (first[firstCount - 1].TryUnion(second[0]) ? 1 : 0)];
                first.CopyTo(newFunctions, 0);
                if (first[firstCount - 1].TryUnion(second[0], out var unitedFunction))
                {
                    second.CopyTo(newFunctions, firstCount - 1);
                    newFunctions[firstCount - 1] = unitedFunction;
                }
                else
                {
                    second.CopyTo(newFunctions, firstCount);
                }
                return new Piecewise<TSpace, TValue>(newFunctions);
            }
            int cycles = firstCount + secondCount;
            newFunctions = new IFunction<TSpace, TValue>[cycles * 2 - 1]; //trust me, I`m an engineer; example 1 + n where second divide first, n -> inf make n*2-1 and its max.

            OrderedAccessToArrays<TSpace, TValue> arrays = new OrderedAccessToArrays<TSpace, TValue>(_functions, piecewise._functions); //This is important from which in which
            IFunction<TSpace, TValue> lastFunction = arrays.GetNext().Function;
            newFunctions[0] = lastFunction;
            int lastFunctionIndex = 0;
            
            for (int i = 1; i < cycles; i++)
            {
                OrderedAccessElement<TSpace, TValue> orderedAccessElement = arrays.GetNext(); //If this and the previous begin with one point, then the interval of this will not be shorter than the previous one
                if (!lastFunction.Interval.Intersect(orderedAccessElement.Function.Interval))
                {
                    lastFunction = orderedAccessElement.Function;
                    newFunctions[++lastFunctionIndex] = lastFunction;
                    continue;
                }
                if (orderedAccessElement.IsFirstArrayElement && !lastFunction.Interval.Cover(orderedAccessElement.Function.Interval))
                {
                    lastFunction = orderedAccessElement.Function.ShortenIntervalTo(
                        new Interval<TSpace>(lastFunction.Interval.End.Position, !lastFunction.Interval.End.Inclusive,
                            orderedAccessElement.Function.Interval.End.Position, orderedAccessElement.Function.Interval.End.Inclusive));
                    newFunctions[++lastFunctionIndex] = lastFunction;
                    continue;
                }
                if (!orderedAccessElement.IsFirstArrayElement && orderedAccessElement.Function.Interval.Cover(lastFunction.Interval))
                {
                    lastFunction = orderedAccessElement.Function;
                    newFunctions[lastFunctionIndex] = lastFunction;
                    continue;
                }
                if (!orderedAccessElement.IsFirstArrayElement && lastFunction.Interval.Cover(orderedAccessElement.Function.Interval) && !(lastFunction.Interval.End.Position.CompareTo(orderedAccessElement.Function.Interval.End.Position) == 0 && lastFunction.Interval.End.Inclusive == orderedAccessElement.Function.Interval.End.Inclusive))
                {
                    //when we make 3 intervals
                    newFunctions[lastFunctionIndex] = lastFunction.ShortenIntervalTo(
                        new Interval<TSpace>(lastFunction.Interval.Start.Position, lastFunction.Interval.Start.Inclusive,
                        orderedAccessElement.Function.Interval.Start.Position, !orderedAccessElement.Function.Interval.Start.Inclusive));

                    newFunctions[++lastFunctionIndex] = orderedAccessElement.Function;

                    lastFunction = lastFunction.ShortenIntervalTo(new Interval<TSpace>(
                        orderedAccessElement.Function.Interval.End.Position, !orderedAccessElement.Function.Interval.End.Inclusive,
                        lastFunction.Interval.End.Position, lastFunction.Interval.End.Inclusive));
                    newFunctions[++lastFunctionIndex] = lastFunction;
                    continue;
                }
                if (!orderedAccessElement.IsFirstArrayElement && ( lastFunction.Interval.Intersect(orderedAccessElement.Function.Interval) || lastFunction.Interval.End.Position.CompareTo(orderedAccessElement.Function.Interval.End.Position) == 0 && lastFunction.Interval.End.Inclusive == orderedAccessElement.Function.Interval.End.Inclusive))
                {
                    newFunctions[lastFunctionIndex] = lastFunction.ShortenIntervalTo(
                        new Interval<TSpace>(lastFunction.Interval.Start.Position, lastFunction.Interval.Start.Inclusive,
                        orderedAccessElement.Function.Interval.Start.Position, !orderedAccessElement.Function.Interval.Start.Inclusive));
                    lastFunction = orderedAccessElement.Function;
                    newFunctions[++lastFunctionIndex] = lastFunction;
                }
            }

            Array.Resize(ref newFunctions, ++lastFunctionIndex);
            return new Piecewise<TSpace, TValue>(newFunctions);
        }

        private Piecewise(IFunction<TSpace, TValue>[] functions)
        {
            _functions = functions;
        }

        public Piecewise(PiecewiseBuilder<TSpace, TValue> piecewiseBuilder)
        {
            _functions = piecewiseBuilder.GetFunctions().ToArray();
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
            Array.Resize(ref _functions, ++lastRealIndex);
        }

        private Piecewise() { }
    }
}
