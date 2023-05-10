namespace QuickStatistics.Net.Median_NS
{
    /// <summary>
    /// Median_Decimal can be used to calculate the median of an array or list.
    /// </summary>
    public static class Median_Decimal
    {
        /// <summary>
        /// gets the median value of an array
        /// </summary>
        /// <param name="numbers">the array with numbers where to get the median from</param>
        /// <param name="inputIsSorted">if the input array is already sorted, specify true to save performance</param>
        /// <returns>median</returns>
        /// <exception cref="ArgumentException"></exception>
        public static decimal GetMedian(decimal[] numbers, bool inputIsSorted = false)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("The input array must not be null or empty.");
            }

            decimal[] sortedNumbers = new decimal[numbers.Length];
            Array.Copy(numbers, sortedNumbers, numbers.Length);
            if (!inputIsSorted)
            {
                Array.Sort(sortedNumbers);
            }


            return Calculate(sortedNumbers);
        }
        /// <summary>
        /// gets the median value of a List
        /// </summary>
        /// <param name="numbers">the list with numbers where to get the median from</param>
        /// <param name="inputIsSorted">if the input list is already sorted, specify true to save performance</param>
        /// <returns>median</returns>
        /// <exception cref="ArgumentException"></exception>
        public static decimal GetMedian(List<decimal> numbers, bool inputIsSorted = false)
        {
            if (numbers == null || numbers.Count == 0)
            {
                throw new ArgumentException("The input list must not be null or empty.");
            }

            decimal[] sortedNumbers = numbers.ToArray();
            if (!inputIsSorted)
            {
                Array.Sort(sortedNumbers);
            }

            return Calculate(sortedNumbers);
        }
        /// <summary>
        /// gets the median value of a sorted(!) array
        /// </summary>
        /// <param name="sortedNumbers"></param>
        /// <returns></returns>
        private static decimal Calculate(decimal[] sortedNumbers)
        {
            int length = sortedNumbers.Length;

            if (length % 2 == 0)
            {
                return (sortedNumbers[length / 2 - 1] + sortedNumbers[length / 2]) / 2.0m;
            }
            else
            {
                return sortedNumbers[length / 2];
            }
        }
    }
}
