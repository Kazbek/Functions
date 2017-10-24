using System.Collections.Generic;
using Functions.Implementations.Functions;
using Functions.Implementations.Intervals;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Perfomance
{
    [TestClass]
    public class Composite
    {
        private readonly Composite<int, int> _composite;
        private int _count;
        public Composite()
        {
            _count = 10000;
            List<IFunction<int, int>> constants = new List<IFunction<int, int>>();

            for (int i = 0; i < _count; i++)
            {
                Interval<int> interval = new Interval<int>(i, true, i + 1, false);
                constants.Add(new Constant<int, int>(interval, i));
            }
            _composite = new Composite<int, int>(constants);
        }

        [TestMethod]
        public void ReadConstants()
        {
            int getValue;
            for (int i = 0; i < _count; i++)
            {
                getValue = _composite.Value(i);
            }
        }

        [TestMethod]
        public void CreateCompositeWithoutUnnions()
        {
            _count = 10000;
            List<IFunction<int, int>> constants = new List<IFunction<int, int>>();

            for (int i = 0; i < _count; i++)
            {
                Interval<int> interval = new Interval<int>(i, true, i + 1, false);
                constants.Add(new Constant<int, int>(interval, i));
            }
            Composite<int,int> composite = new Composite<int, int>(constants);
            int a = composite.Value(1);
        }

        [TestMethod]
        public void CreateCompositeWithHalfUions()
        {
            _count = 10000;
            List<IFunction<int, int>> constants = new List<IFunction<int, int>>();

            for (int i = 0; i < _count; i++)
            {
                Interval<int> interval = new Interval<int>(i, true, i + 1, false);
                constants.Add(new Constant<int, int>(interval, i/2));
            }
            Composite<int, int> composite = new Composite<int, int>(constants);
            int a = composite.Value(1);
        }
    }
}
