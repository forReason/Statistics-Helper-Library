using System.Numerics;

namespace QuickStatistics.Net.Median_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Provides methods to calculate the median of a sequence of numbers.
    /// </summary>
    public static class Median
    {
        /// <summary>
        /// Gets the median value of a sequence of numbers as a double.
        /// </summary>
        /// <typeparam name="T">The number type implementing INumber.</typeparam>
        /// <param name="numbers">The sequence of numbers.</param>
        /// <param name="inputIsSorted">Specifies if the input sequence is already sorted.</param>
        /// <returns>The median value as double.</returns>
        /// <exception cref="ArgumentException">Thrown if the input sequence is null or empty.</exception>
        public static double GetMedian<T>(IEnumerable<T> numbers, bool inputIsSorted = false) where T : INumber<T>
        {
            if (numbers is null || !numbers.Any())
                throw new ArgumentException("The input sequence must not be null or empty.");

            var sortedNumbers = inputIsSorted ? numbers.ToList() : numbers.OrderBy(x => x).ToList();
            return CalculateMedian(sortedNumbers);
        }

        /// <summary>
        /// Calculates the median from a sorted list of numbers and returns it as double.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedNumbers"></param>
        /// <returns></returns>
        private static double CalculateMedian<T>(IList<T> sortedNumbers) where T : INumber<T>
        {
            int count = sortedNumbers.Count;
            if (count % 2 == 0)
            {
                return (Convert.ToDouble(sortedNumbers[count / 2 - 1]) + Convert.ToDouble(sortedNumbers[count / 2])) / 2.0;
            }
            else
            {
                return Convert.ToDouble(sortedNumbers[count / 2]);
            }
        }
    }
#endif
}
