using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
    #if NET7_0_OR_GREATER
    public class Median <T> where T : INumber<T>
    {
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
