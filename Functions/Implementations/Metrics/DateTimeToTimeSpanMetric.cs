using System;
using Functions.Interfaces;

namespace Functions.Implementations.Metrics
{
    public class DateTimeToTimeSpanMetric : IIntervalMetric<DateTime, TimeSpan>
    {
        public TimeSpan GetMetric(DateTime point1, DateTime point2) => point1 > point2 ? point1.Subtract(point2) : point2.Subtract(point1);

        public TimeSpan GetMetric(IInterval<DateTime> interval) => interval.End.Position.Subtract(interval.Start.Position);
    }
}
