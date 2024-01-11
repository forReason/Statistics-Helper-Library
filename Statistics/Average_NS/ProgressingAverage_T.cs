using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Provides a thread-safe, simple, fast, and precise method to compute the average of a finite number of inputs.
    /// This class uses decimal for calculations which provides precision even for large numbers. <br/><br/>
    /// thread safe
    /// </summary>
    /// <remarks>
    /// This class is not suitable for an indefinite number of inputs.
    /// For that purpose, please refer to <see cref="SimpleMovingAverage"/>.<br/>
    /// The theoretical maximum amount of Data Points is int.MaxValue. <br/>
    /// However, the precision might become less the closer you reach count = int.MaxValue
    /// </remarks>
    public class ProgressingAverage<T> where T : INumber<T>
    {
        private ProgressingAverage_Decimal avg;

        /// <summary>
        /// Initializes a new instance of the ProgressingAverage class.
        /// </summary>
        public ProgressingAverage()
        {
            avg = new ProgressingAverage_Decimal();
        }

        /// <summary>
        /// Gets the current average value.
        /// </summary>
        public decimal ValuePrecise => avg.Value;
        /// <summary>
        /// Gets the current average value.
        /// </summary>
        public double Value => (double)avg.Value;

        /// <summary>
        /// Gets the current count of values that have been added.
        /// </summary>
        public decimal Count => avg.Count;

        /// <summary>
        /// Adds a new value to the average calculation.
        /// </summary>
        /// <param name="input">The value to add.</param>
        public void AddValue(T input)
        {
            avg.AddValue(Convert.ToDecimal(input));
        }

        /// <summary>
        /// Adds an array of new values to the average calculation.
        /// </summary>
        /// <param name="input">The array of values to add.</param>
        public void AddValue(T[] input)
        {
            foreach (var item in input)
            {
                avg.AddValue(Convert.ToDecimal(item));
            }
        }

        /// <summary>
        /// Adds a list of new values to the average calculation.
        /// </summary>
        /// <param name="input">The list of values to add.</param>
        public void AddValue(List<T> input)
        {
            foreach (var item in input)
            {
                avg.AddValue(Convert.ToDecimal(item));
            }
        }

        /// <summary>
        /// Clears the current average calculation.
        /// </summary>
        public void Clear()
        {
            avg.Clear();
        }

        /// <summary>
        /// Returns a string that represents the current average value.
        /// </summary>
        /// <returns>A string that represents the current average value.</returns>
        public override string ToString()
        {
            return avg.ToString();
        }
    }
#endif
}
