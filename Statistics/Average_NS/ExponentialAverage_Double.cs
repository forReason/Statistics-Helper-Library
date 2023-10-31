
namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// Provides functionality for calculating Exponential Moving Averages (EMA) with time-based decay.
    /// </summary>
    public class ExponentialAverage_Double

    {
        private double currentEma = 0.0;
        private bool isInitialized = false;
        private TimeSpan halfLife;
        private DateTime lastTimestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialAverage_Double"/> class.
        /// </summary>
        /// <param name="approximateSmaTime">The approximate Simple Moving Average (SMA) time span.</param>
        public ExponentialAverage_Double(TimeSpan approximateSmaTime)
        {
            this.halfLife = CalculateHalfLife(approximateSmaTime);
        }

        /// <summary>
        /// Calculates the half-life based on the approximate SMA time span.
        /// </summary>
        /// <param name="approximateSmaTime">The approximate SMA time span.</param>
        /// <returns>Returns the calculated half-life.</returns>
        private static TimeSpan CalculateHalfLife(TimeSpan approximateSmaTime)
        {
            double smaDays = approximateSmaTime.TotalDays;
            double halfLifeDays = smaDays / Math.Log(2);
            return TimeSpan.FromDays(halfLifeDays);
        }

        /// <summary>
        /// Adds a new value to the EMA calculation.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <param name="timestamp">The timestamp for when the value was recorded.</param>
        public void AddValue(double value, DateTime timestamp)
        {
            if (!isInitialized)
            {
                currentEma = value;
                isInitialized = true;
            }
            else
            {
                TimeSpan elapsed = timestamp - lastTimestamp;
                double decayFactor = Math.Pow(0.5, elapsed.TotalSeconds / halfLife.TotalSeconds);

                currentEma = (value * (1 - decayFactor)) + (currentEma * decayFactor);
            }
            lastTimestamp = timestamp;
        }

        /// <summary>
        /// Gets the current Exponential Moving Average (EMA) value.
        /// </summary>
        /// <returns>Returns the current EMA.</returns>
        public double GetAverage()
        {
            return currentEma;
        }
    }
}
