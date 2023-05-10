using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// generic method in order to obtain the median from other types than double.
    /// </summary>
    /// <remarks>internaly uses a conversion to double at some point so may not be a good idea for large data types such a decimal<br/>
    /// better use <see cref="Median_Decimal"/> for this purpose</remarks>
    /// <typeparam name="T"></typeparam>
    public class Median <T> where T : INumber<T>
    {
        /// <summary>
        /// gets the median value of an array of numbers
        /// </summary>
        /// <param name="numbers">the numbers input</param>
        /// <param name="inputIsSorted">specify when the input numbers are sorted already to save processing time</param>
        /// <returns>median</returns>
        /// <exception cref="ArgumentException"></exception>
        public double GetMedian(T[] numbers, bool inputIsSorted = false)
        {
            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentException("The input array must not be null or empty.");
            }

            T[] sortedNumbers = new T[numbers.Length];
            Array.Copy(numbers, sortedNumbers, numbers.Length);
            if (!inputIsSorted)
            {
                Array.Sort(sortedNumbers);
            }
            

            return Calculate(sortedNumbers);
        }
        /// <summary>
        /// gets the median value of a list of numbers
        /// </summary>
        /// <param name="numbers">the numbers input</param>
        /// <param name="inputIsSorted">specify when the input numbers are sorted already to save processing time</param>
        /// <returns>median</returns>
        /// <exception cref="ArgumentException"></exception>
        public double GetMedian(List<T> numbers, bool inputIsSorted = false)
        {
            if (numbers == null || numbers.Count == 0)
            {
                throw new ArgumentException("The input list must not be null or empty.");
            }

            T[] sortedNumbers = numbers.ToArray();
            if (!inputIsSorted)
            {
                Array.Sort(sortedNumbers);
            }

            return Calculate(sortedNumbers);
        }
        /// <summary>
        /// gets the median value from a SORTED array of numbers
        /// </summary>
        /// <param name="sortedNumbers"></param>
        /// <returns></returns>
        private double Calculate(T[] sortedNumbers)
        {
            int length = sortedNumbers.Length;

            if (length % 2 == 0)
            {
                return (Convert.ToDouble(sortedNumbers[length / 2 - 1]) + Convert.ToDouble(sortedNumbers[length / 2])) / 2.0;
            }
            else
            {
                return Convert.ToDouble(sortedNumbers[length / 2]);
            }
        }
    }
    #endif
}
