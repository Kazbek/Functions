using System;
using Functions.Interfaces;

namespace Functions.Implementations.Metrics
{
    public static class Metric
    {
        public static TimeSpan GetMetric(DateTime point1, DateTime point2) => point1 > point2 ? point1.Subtract(point2) : point2.Subtract(point1);
        public static TimeSpan GetMetric(IInterval<DateTime> interval) => interval.End.Position.Subtract(interval.Start.Position);

        public static int GetMetric(int point1, int point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static int GetMetric(IInterval<int> interval) => interval.End.Position - interval.Start.Position;

        public static uint GetMetric(uint point1, uint point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static uint GetMetric(IInterval<uint> interval) => interval.End.Position - interval.Start.Position;

        public static long GetMetric(long point1, long point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static long GetMetric(IInterval<long> interval) => interval.End.Position - interval.Start.Position;

        public static ulong GetMetric(ulong point1, ulong point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static ulong GetMetric(IInterval<ulong> interval) => interval.End.Position - interval.Start.Position;

        public static int GetMetric(short point1, short point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static int GetMetric(IInterval<short> interval) => interval.End.Position - interval.Start.Position;

        public static int GetMetric(ushort point1, ushort point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static int GetMetric(IInterval<ushort> interval) => interval.End.Position - interval.Start.Position;

        public static int GetMetric(byte point1, byte point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static int GetMetric(IInterval<byte> interval) => interval.End.Position - interval.Start.Position;

        public static int GetMetric(sbyte point1, sbyte point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static int GetMetric(IInterval<sbyte> interval) => interval.End.Position - interval.Start.Position;

        public static double GetMetric(double point1, double point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static double GetMetric(IInterval<double> interval) => interval.End.Position - interval.Start.Position;

        public static float GetMetric(float point1, float point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static float GetMetric(IInterval<float> interval) => interval.End.Position - interval.Start.Position;

        public static decimal GetMetric(decimal point1, decimal point2) => point1 > point2 ? point1 - point2 : point2 - point1;
        public static decimal GetMetric(IInterval<decimal> interval) => interval.End.Position - interval.Start.Position;
    }
}
