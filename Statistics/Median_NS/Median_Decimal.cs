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
        public static decimal GetMedian(IEnumerable<decimal> numbers, bool inputIsSorted = false)
        {
            if (numbers == null || !numbers.Any())
            {
                throw new ArgumentException("The input array must not be null or empty.");
            }
            IList<decimal> sourceArray = numbers as IList<decimal> ?? numbers.ToList();
            
            return inputIsSorted ? Calculate(sourceArray) : Calculate(sourceArray.OrderBy(x => x).ToList());
        }
        /// <summary>
        /// gets the median value of a sorted(!) array
        /// </summary>
        /// <param name="sortedNumbers"></param>
        /// <returns></returns>
        private static decimal Calculate(IList<decimal> sortedNumbers)
        {
            int length = sortedNumbers.Count;

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
