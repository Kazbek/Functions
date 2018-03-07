using System;
using Functions.Implementations.Metrics;
using Functions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Functions.Tests.Metrics
{
    [TestClass]
    public class DateTimeToMonths
    {
        [TestMethod]
        [DataRow(2012, 1, 1, 2012, 2, 1, 1)]
        [DataRow(2012, 1, 1, 2012, 1, 1, 0)]
        [DataRow(2012, 1, 1, 2012, 1, 31, 0)]
        [DataRow(2012, 1, 1, 2013, 2, 1, 13)]
        [DataRow(2011, 1, 1, 2012, 1, 1, 12)]
        [DataRow(2012, 2, 1, 2012, 1, 1, 1)]
        [DataRow(2012, 4, 1, 2012, 2, 1, 2)]
        public void DateTimeToMonthMetric(int year1, int month1, int day1, int year2, int month2, int day2, int result)
        {
            DateTime point1 = new DateTime(year1, month1, day1);
            DateTime point2 = new DateTime(year2, month2, day2);
            IMetric<DateTime, int> metric = new DateTimeToMonth();
            int months = metric.GetMetric(point1, point2);
            Assert.AreEqual(result, months);

        }
    }
}
