using System;

namespace Functions.Implementations.Metrics
{
    public static class Metric
    {
        public static TimeSpan GetMetric(DateTime point1, DateTime point2) => point1 > point2 ? point1.Subtract(point2) : point2.Subtract(point1);
        public static int GetMetric(int point1, int point2) => point1 > point2 ? point1 - point2 : point2 - point1;
    }
}
