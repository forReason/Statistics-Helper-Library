
namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// Provides functionality for calculating Exponential Moving Averages (EMA) with time-based decay.
    /// </summary>
    public class ExponentialAverage_Decimal
    {
        private decimal currentEma = 0.0m;
        private bool isInitialized = false;
        private TimeSpan halfLife;
        private DateTime lastTimestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialAverage_Decimal"/> class.
        /// </summary>
        /// <param name="approximateSmaTime">The approximate Simple Moving Average (SMA) time span.</param>
        public ExponentialAverage_Decimal(TimeSpan approximateSmaTime)
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
        public void AddValue(decimal value, DateTime? timestamp = null)
        {
            if (timestamp == null) timestamp = DateTime.Now;
            if (!isInitialized)
            {
                currentEma = value;
                isInitialized = true;
            }
            else
            {
                TimeSpan elapsed = timestamp.Value - lastTimestamp;
                decimal decayFactor = (decimal)Math.Pow((double)0.5, (double)elapsed.TotalSeconds / (double)halfLife.TotalSeconds);
                currentEma = (value * (1 - decayFactor)) + (currentEma * decayFactor);
            }
            lastTimestamp = timestamp.Value;
        }

        /// <summary>
        /// Gets the current Exponential Moving Average (EMA) value.
        /// </summary>
        /// <returns>Returns the current EMA.</returns>
        public decimal GetAverage()
        {
            return currentEma;
        }
    }
}
