using System;
using System.Collections.Generic;
using System.Text;
using Functions.Interfaces;

namespace Functions.Implementations.Utils
{
    internal static class Utils
    {
        internal static int InretvalBinarySearch<TSpace, TValue>(IFunction<TSpace, TValue>[] array, TSpace point) where TSpace : IComparable<TSpace>
        {
            int low = 0; // 0 is always going to be the first element
            int high = array.Length - 1; // Find highest element
            int middle = (low + high + 1) / 2; // Find middle element
            int location = -1; // Return value -1 if not found

            do
            {
                IFunction<TSpace, TValue> middleElement = array[middle];

                int compare = point.CompareTo(middleElement.Interval.Start.Position);
                if (compare < 0 || compare == 0 && !middleElement.Interval.Start.Inclusive)
                {
                    high = middle - 1;
                    middle = (low + high + 1) / 2;
                    continue;
                }

                compare = point.CompareTo(middleElement.Interval.End.Position);
                if (compare > 0 || compare == 0 && !middleElement.Interval.End.Inclusive)
                {
                    low = middle + 1;
                    middle = (low + high + 1) / 2;
                    continue;
                }

                if (middleElement.Interval.Contains(point))
                {
                    location = middle;
                    break;
                }
            } while (low <= high);

            return location;
        }
    }
}
