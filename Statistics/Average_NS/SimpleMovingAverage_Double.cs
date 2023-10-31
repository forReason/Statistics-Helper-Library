namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// Provides a simple and fast method for calculating the moving average on sliding data.
    /// This class is especially useful for large datasets or continuous data streams.
    /// </summary>
    /// <remarks>
    /// Memory requirement grows linearly with the data length. <br/>
    /// For very large lengths, an <see cref="SimpleExponentialAverage_Double"/> may
    /// be better suited, although less precise (focusses on recent datapoints)
    /// </remarks>
    public class SimpleMovingAverage_Double
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMovingAverage_Double"/> class with a specified length for the moving average window.
        /// </summary>
        /// <remarks>
        /// Memory requirement grows linearly with the data length. <br/>
        /// For very large lengths, an <see cref="SimpleExponentialAverage_Double"/> may
        /// be better suited, although less precise (focusses on recent datapoints)
        /// </remarks>
        /// <param name="length">The length of the moving average window.</param>
        public SimpleMovingAverage_Double(int length)
        {
            MaxDataLength = length;
            Clear();
        }

        /// <summary>
        /// holds all the values
        /// </summary>
        private Queue<double> AverageQueue = new Queue<double>();
        /// <summary>
        /// returns the oldest element of the averagequeue
        /// </summary>
        public double OldestElement { get { return AverageQueue.Peek(); } }
        /// <summary>
        /// returns the newest element of the averagequeue
        /// </summary>
        public double NewestElement { get { return AverageQueue.Last(); } }


        /// <summary>
        /// the current Simple Moving Average (SMA) value.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// the maximum length of the data for the moving average.
        /// </summary>
        public int MaxDataLength { get; private set; }

        /// <summary>
        /// Specifies the absolute rate of change by which the data set moved on average at each data point.
        /// </summary>
        public double AbsoluteRateOfChange { get { return (AverageQueue.Last() - AverageQueue.Peek()) / AverageQueue.Count; } }

        /// <summary>
        /// Calculates the trend based on the oldest data point, often used for investment scenarios.<br/>
        /// The trend is calculated using the oldest point as a reference.
        /// </summary>
        /// <remarks>
        /// The value is denoted as a percentage, e.g., -1.0 to X.0.
        /// </remarks>
        public double Trend
        {
            get
            {
                return AbsoluteRateOfChange / AverageQueue.Peek();
            }
        }

        /// <summary>
        /// Calculates the short-term trend (also known as Relative Strength or Momentum), often used for trading scenarios.<br/>
        /// The trend is calculated based on the AbsoluteRateOfChange and the most recent datapoint as a reference.
        /// </summary>
        /// <remarks>
        /// The value is denoted as a percentage, e.g., -1.0 to X.0.
        /// </remarks>
        public double Momentum
        {
            get
            {
                return AbsoluteRateOfChange / AverageQueue.Last();
            }
        }

        /// <summary>
        /// Calculates the deviation between the current data point and the Simple Moving Average (SMA).<br/>
        /// Deviation is the difference between the current value and the SMA, giving an idea of how much the current value varies from the average.
        /// </summary>
        /// <remarks>
        /// Positive deviation implies the current value is above the SMA, often interpreted as bullish movement in financial contexts.<br/>
        /// Negative deviation implies the current value is below the SMA, often interpreted as bearish movement in financial contexts.
        /// </remarks>
        public double DeviationFromSMA_Absolute { get { return AverageQueue.Last() - Value; } }

        /// <summary>
        /// Calculates the percentage-based deviation between the current data point and the Simple Moving Average (SMA).<br/>
        /// The percentage-based deviation shows how much the current value deviates from the SMA relative to the SMA itself.
        /// </summary>
        /// <remarks>
        /// Positive values indicate the current value is greater than the SMA, often interpreted as bullish.<br/>
        /// Negative values indicate the current value is less than the SMA, often interpreted as bearish.<br/>
        /// The value is denoted as a percentage, e.g., -1.0 to +X.0.
        /// </remarks>
        public double DeviationFromSMA_Percentage { get { return DeviationFromSMA_Absolute / Value; } }

        /// <summary>
        /// Gets the current number of data points in the moving average calculation.
        /// </summary>
        public int CurrentDataLength { get { return AverageQueue.Count; } }

        /// <summary>
        /// Adds a new data point to the moving average calculation.
        /// </summary>
        /// <param name="input">The new data point.</param>
        public void AddValue(double input)
        {
            double weightedInput = input / MaxDataLength;
            AverageQueue.Enqueue(weightedInput);
            if (AverageQueue.Count <= MaxDataLength)
            {
                Value += (input - Value) / AverageQueue.Count;
            }
            else
            {
                double change = weightedInput;
                change -= AverageQueue.Dequeue();
                Value += change;
            }
        }

        /// <summary>
        /// Clears all data points and resets the moving average to zero.
        /// </summary>
        public void Clear()
        {
            AverageQueue.Clear();
            Value = 0;
        }
        /// <returns>The current SMA value as a string.</returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}