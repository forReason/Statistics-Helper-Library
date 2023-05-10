using System.Numerics;

namespace QuickStatistics.Net.Median_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// moving median (or: Rolling Median) calculates the median on a rolling Time window and thus is suitable for indefinite data inflow
    /// </summary>
    /// <remarks>uses an internal conversion to double. For larger numbers use <see cref="MovingMedian_Double"/></remarks>
    public class MovingMedian<T> where T : INumber<T>
    {
        MovingMedian_Double Median_Double { get; set; }

        public MovingMedian(int windowSize)
        {
            Median_Double = new MovingMedian_Double(windowSize);
        }

        public void AddValue(T value)
        {
            Median_Double.AddValue(Convert.ToDouble(value));
        }

        public double GetMedian()
        {
            return Median_Double.GetMedian();
        }

    }
#endif
}
