

using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// running median can be used to quickly gather the median of all values added (so far)
    /// note, you might run into memory constraints, so this method is not suitable for infinite data flow
    /// </summary>
    /// <remarks>has an internal conversion to double. For large numbers use <see cref="RunningMedian_Decimal"/></remarks>
    public class RunningMedian<T> where T : INumber<T>
    {
        private RunningMedian_Double Median = new RunningMedian_Double();

        public void AddValue(T value)
        {
            Median.AddValue(Convert.ToDouble(value));
        }

        public double GetMedian()
        {
            return Median.GetMedian();
        }
    }
}