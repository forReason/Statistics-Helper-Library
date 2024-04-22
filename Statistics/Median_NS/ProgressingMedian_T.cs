

using System.Numerics;

namespace QuickStatistics.Net.Median_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// running median can be used to quickly gather the median of all values added (so far)
    /// note, you might run into memory constraints, so this method is not suitable for infinite data flow
    /// </summary>
    /// <remarks>has an internal conversion to double. For large numbers use <see cref="ProgressingMedian_Decimal"/></remarks>
    public class ProgressingMedian<T> where T : INumber<T>
    {
        private ProgressingMedian_Double Median = new ProgressingMedian_Double();

        /// <summary>
        ///  the amount of Values added so far
        /// </summary>
        public ulong ValueCount => Median.ValueCount;
        /// <summary>
        /// true when values have been added already
        /// </summary>
        public bool ContainsValues => Median.ContainsValues;

        /// <summary>
        /// retrieves the current Median Value.
        /// </summary>
        /// <remarks>Values need to be added before accessing</remarks>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when no Values are added yet</exception>
        public void AddValue(T value)
        {
            Median.AddValue(Convert.ToDouble(value));
        }

        /// <summary>
        /// retrieves the current Median Value.
        /// </summary>
        /// <remarks>Values need to be added before accessing</remarks>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when no Values are added yet</exception>
        public double GetMedian()
        {
            return Median.GetMedian();
        }

        /// <summary>
        /// retrieves the current Median Value.
        /// </summary>
        /// <remarks>Values need to be added before accessing</remarks>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when no Values are added yet</exception>
        public double Value => GetMedian();

        /// <summary>
        /// empties the median making it ready for reuse
        /// </summary>
        public void Clear()
        {
            Median.Clear();
        }
    }
#endif
}