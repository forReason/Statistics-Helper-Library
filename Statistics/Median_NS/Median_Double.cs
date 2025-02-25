﻿namespace QuickStatistics.Net.Median_NS
{
    /// <summary>
    /// Median_Double can be used to calculate the median of an array or list.
    /// </summary>
    public static class Median_Double
    {
        /// <summary>
        /// gets the median value of a sequence of numbers
        /// </summary>
        /// <param name="numbers">the sequence with numbers where to get the median from</param>
        /// <param name="inputIsSorted">if the input sequence is already sorted, specify true to save performance</param>
        /// <returns>median</returns>
        /// <exception cref="ArgumentException"></exception>
        public static double GetMedian(IEnumerable<double> numbers, bool inputIsSorted = false)
        {
            if (numbers is null || !numbers.Any())
            {
                throw new ArgumentException("The input sequence must not be null or empty.");
            }

            IList<double> sourceArray = numbers as IList<double> ?? numbers.ToList();

            return inputIsSorted ? Calculate(sourceArray) : Calculate(sourceArray.OrderBy(x => x).ToList());
        }

        /// <summary>
        /// gets the median value of a sorted(!) array
        /// </summary>
        /// <param name="sortedNumbers"></param>
        /// <returns></returns>
        private static double Calculate(IList<double> sortedNumbers)
        {
            int length = sortedNumbers.Count;

            if (length % 2 == 0)
            {
                return (sortedNumbers[length / 2 - 1] + sortedNumbers[length / 2]) / 2.0;
            }
            else
            {
                return sortedNumbers[length / 2];
            }
        }
    }
}
