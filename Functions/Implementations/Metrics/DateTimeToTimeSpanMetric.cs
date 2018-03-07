using System;
using Functions.Interfaces;

namespace Functions.Implementations.Metrics
{
    public class DateTimeToTimeSpanMetric : IMetric<DateTime, TimeSpan>
    {
        public TimeSpan GetMetric(DateTime point1, DateTime point2) => point1 > point2 ? point1.Subtract(point2) : point2.Subtract(point1);
    }
}
