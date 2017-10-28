﻿using System;
using System.Collections.Generic;
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

        public IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function)
        {
            if(!TryUnion(function))
                throw new Exception("It is not possible to combine these functions.");
            Composite<TSpace, TValue> composite = function as Composite<TSpace, TValue>;
            Composite<TSpace, TValue> first, second;
            if (Interval.Start.CompareTo(function.Interval.Start) > 0)
            {
                first = (Composite<TSpace, TValue>) function;
                second = this;
            }
            else
            {
                first = this;
                second = (Composite<TSpace, TValue>)function;
            }
            int firstCount = first._functions.Length;
            int secondCount = second._functions.Length;
            IFunction<TSpace, TValue>[] newFunctions = new IFunction<TSpace, TValue>[firstCount + secondCount - (first._functions[firstCount - 1].TryUnion(second._functions[0])?1:0)];
            first._functions.CopyTo(newFunctions,0);
            if (first._functions[firstCount - 1].TryUnion(second._functions[0]))
            {
                second._functions.CopyTo(newFunctions, firstCount - 1);
                newFunctions[firstCount - 1] = first._functions[firstCount - 1].Union(second._functions[0]);
            }else
            {
                second._functions.CopyTo(newFunctions, firstCount);
            }
            return new Composite<TSpace, TValue>(ref newFunctions);
        }
        /// <summary>
        /// Создаёт сложную функцию, состоящую из нескольких других, объединенных на непрерывном интервале.
        /// </summary>
        /// <param name="functions">Функции, из которых будет состоять создаваемая. Должны быть упорядочены по интервалам, не иметь разрывов между собой, а также не перечекаться.</param>
        public Composite(List<IFunction<TSpace, TValue>> functions)
        {
            int functionsCount = functions?.Count ?? 0;
            if (functionsCount == 0)
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

        private Composite(ref IFunction<TSpace, TValue>[] array)
        {
            _functions = array;
            Interval = new Interval<TSpace>(array[0].Interval.Start, array[array.Length-1].Interval.End);
        }

        
    }
}
