using System;

namespace Functions.Interfaces
{
    public interface IIntervalEdge<TSpace> : IComparable<IIntervalEdge<TSpace>> where TSpace : IComparable<TSpace>
    {
        TSpace Position { get; }
        bool Inclusive { get; }
    }
}
