using System;
using System.Collections.Generic;
using System.Text;

namespace Functions.Interfaces
{
    public interface IFunction<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        TValue Value(TSpace point);
        IInterval<TSpace> Interval { get; }
        bool IsDefinedOn(TSpace point);
        bool TryUnion(IFunction<TSpace, TValue> function);
        IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function);
    }
}
