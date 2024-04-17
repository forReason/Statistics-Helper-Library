using System.Numerics;

namespace QuickStatistics.Net.Median_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Moving median (or: Rolling Median) calculates the median on a rolling Time window and thus is suitable for indefinite data inflow
    /// </summary>
    /// <remarks>uses an internal conversion to double. For larger numbers, use <see cref="MovingMedian_Double"/></remarks>
    public class MovingMedian<T> where T : INumber<T>
    {
        private MovingMedian_Double Median_Double { get; set; }
    
        public MovingMedian(int windowSize)
        {
            Median_Double = new MovingMedian_Double(windowSize);
        }
    
        /// <summary>
        /// Adds a new value to the rolling window and updates the median calculation.
        /// </summary>
        /// <param name="value">The value to be added to the rolling window.</param>
        public void AddValue(T value)
        {
            Median_Double.AddValue(Convert.ToDouble(value));
        }
    
        /// <summary>
        /// Retrieves the current median value of the elements within the rolling window.
        /// </summary>
        /// <returns>The median value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no values have been added yet.</exception>
        public double GetMedian()
        {
            return Median_Double.GetMedian();
        }
    
        /// <summary>
        /// Clears all values from the moving median, resetting its state.
        /// </summary>
        public void Clear()
        {
            Median_Double.Clear();
        }
    
        /// <summary>
        /// obtains the current value
        /// </summary>
        public bool ContainsValues => Median_Double.ContainsValues;
        /// <summary>
        /// Gets the current median value of the elements.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no values have been added yet.</exception>
        public double Value => Median_Double.GetMedian();
        /// <summary>
        /// returns the maximum value in the current sliding window
        /// </summary>
        public double Maximum => Median_Double.Maximum;
        /// <summary>
        /// returns the Minimum value in the current sliding Window
        /// </summary>
        public double Minimum => Median_Double.Minimum;
        /// <summary>
        /// gets the x percentile value in the current time window with a worst case cost of O(log(n/2))
        /// </summary>
        /// <remarks>This needs to iterate over the internal sorted dictionary.
        /// It`s hence not recommended to pull this metric for every single data entry in a tight loop</remarks>
        /// <param name="percentile"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public double GetPercentile(double percentile) => Median_Double.GetPercentile(percentile);

        /// <summary>
        /// generates a distribution map for the values in the current Heap
        /// </summary>
        /// <param name="steps">the resolution</param>
        /// <returns></returns>
        public SortedDictionary<double, int> GenerateDistribution(int steps = 10) =>
            Median_Double.GenerateDistribution(steps);
    }
#endif
}
