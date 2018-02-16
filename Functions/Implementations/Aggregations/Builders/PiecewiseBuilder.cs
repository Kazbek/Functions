using System;
using System.Collections.Generic;
using Functions.Implementations.Comparators;
using Functions.Interfaces;

namespace Functions.Implementations.Aggregations.Builders
{
    public class PiecewiseBuilder<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        private readonly LinkedList<IFunction<TSpace, TValue>> _functions;

        public Piecewise<TSpace, TValue> ToPiecewise()
        {
            return new Piecewise<TSpace, TValue>(this);
        }

        internal IEnumerable<IFunction<TSpace, TValue>> GetFunctions() => _functions;

        public void Append(IFunction<TSpace, TValue> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (_functions.Count == 0)
            {
                _functions.AddLast(function);
                return;
            }
            int compare = _functions.Last.Value.Interval.End.CompareTo(function.Interval.Start);
            if (compare > 0 || compare == 0 && !(_functions.Last.Value.Interval.End.Inclusive && function.Interval.Start.Inclusive))
            {
                _functions.AddLast(function);
                return;
            }
            throw new ArgumentException("The interval of the function should go after the ones specified earlier.");

        }

        public void Prepend(IFunction<TSpace, TValue> function)
        {

            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (_functions.Count == 0)
            {
                _functions.AddFirst(function);
                return;
            }
            int compare = _functions.First.Value.Interval.Start.CompareTo(function.Interval.End);
            if (compare > 0 || compare == 0 && !(_functions.Last.Value.Interval.Start.Inclusive && function.Interval.End.Inclusive))
            {
                _functions.AddFirst(function);
                return;
            }
            throw new ArgumentException("The interval of the function should go before the ones specified earlier.");
        }

        public PiecewiseBuilder(List<IFunction<TSpace, TValue>> functions = null)
        {
            _functions = new LinkedList<IFunction<TSpace, TValue>>();
            if (functions == null || functions.Count == 0)
                return;

            functions.Sort(new FunctionIntervalComparer<TSpace, TValue>());
            int count = functions.Count;
            _functions.AddFirst(functions[0]);
            for (int i = 1; i < count; i++)
            {
                IFunction<TSpace, TValue> function = functions[i];
                if (_functions.Last.Value.TryUnion(function, out var unitedFunction))
                {
                    _functions.Last.Value = unitedFunction;
                }
                else if (_functions.Last.Value.Interval.Intersect(function.Interval))
                {
                    throw new Exception("Not combinable functions have intersected intervals.");
                }
                else
                {
                    _functions.AddLast(function);
                }
            }

        }
    }
}