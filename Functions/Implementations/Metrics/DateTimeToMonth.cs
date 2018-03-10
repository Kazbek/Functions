using System;
using Functions.Interfaces;

namespace Functions.Implementations.Metrics
{
    public class DateTimeToMonth : IIntervalMetric<DateTime, int>
    {
        public int GetMetric(DateTime point1, DateTime point2)
        {
            if (point1 > point2)
            {
                DateTime tmp = point1;
                point1 = point2;
                point2 = tmp;
            }
            return (point2.Year - point1.Year) * 12 + (point2.Month - point1.Month);
        }

        public int GetMetric(IInterval<DateTime> interval) => (interval.End.Position.Year - interval.Start.Position.Year) * 12 + (interval.End.Position.Month - interval.Start.Position.Month);
    }
}
