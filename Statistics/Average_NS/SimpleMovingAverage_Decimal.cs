namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// a very simple and fast method to get the moving average on sliding data.
    /// This function is particularly useful for large datasets or infinite data inflow
    /// </summary>
    /// <remarks>
    /// Memory requirement grows linearly with the data length. <br/>
    /// For very large lengths, an <see cref="SimpleExponentialAverage_Decimal"/> may
    /// be better suited, although less precise (focusses on recent datapoints)
    /// </remarks>
    public class SimpleMovingAverage_Decimal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMovingAverage_Decimal"/> class with a specified length for the moving average window.
        /// </summary>
        /// <remarks>
        /// Memory requirement grows linearly with the data length. <br/>
        /// For very large lengths, an <see cref="SimpleExponentialAverage_Decimal"/> may
        /// be better suited, although less precise (focusses on recent datapoints)
        /// </remarks>
        /// <param name="length">The length of the moving average window.</param>
        public SimpleMovingAverage_Decimal(int length)
        {
            MaxDataLength = length;
            Clear();
        }
        /// <summary>
        /// holds all the values
        /// </summary>
        private Queue<decimal> AverageQueue = new Queue<decimal>();
        /// <summary>
        /// returns the oldest element of the averagequeue
        /// </summary>
        public decimal OldestElement { get { return AverageQueue.Peek(); } }
        /// <summary>
        /// returns the newest element of the averagequeue
        /// </summary>
        public decimal NewestElement { get { return AverageQueue.Last(); } }
        /// <summary>
        /// the current Simple Moving Average (SMA) value.
        /// </summary>
        public decimal Value { get; private set; }
        /// <summary>
        /// the maximum length of the data for the moving average.
        /// </summary>
        public int MaxDataLength { get; private set; }
        /// <summary>
        /// Specifies the absolute rate of change by which the data set moved on average at each data point.
        /// </summary>
        public decimal AbsoluteRateOfChange { get { return (AverageQueue.Last() - AverageQueue.Peek()) / AverageQueue.Count; } }

        /// <summary>
        /// Calculates the trend based on the moving average in order to obtain a percentage
        /// </summary>
        /// <remarks>
        /// The value is denoted as a percentage, e.g., -1.0 to X.0.
        /// </remarks>
        public decimal Trend
        {
            get
            {
                return AbsoluteRateOfChange / Value;
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
        public decimal DeviationFromSMA_Absolute { get { return AverageQueue.Last() - Value; } }

        /// <summary>
        /// Calculates the percentage-based deviation between the current data point and the Simple Moving Average (SMA).<br/>
        /// The percentage-based deviation shows how much the current value deviates from the SMA relative to the SMA itself.
        /// </summary>
        /// <remarks>
        /// Positive values indicate the current value is greater than the SMA, often interpreted as bullish.<br/>
        /// Negative values indicate the current value is less than the SMA, often interpreted as bearish.<br/>
        /// The value is denoted as a percentage, e.g., -1.0 to +X.0.
        /// </remarks>
        public decimal DeviationFromSMA_Percentage { get { return DeviationFromSMA_Absolute / Value; } }

        /// <summary>
        /// Gets the current number of data points in the moving average calculation.
        /// </summary>
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        /// <summary>
        /// Adds a new data point to the moving average calculation.
        /// </summary>
        /// <param name="input">The new data point.</param>
        public void AddValue(decimal input)
        {
            decimal weightedInput = input / MaxDataLength;
            AverageQueue.Enqueue(weightedInput);
            if (AverageQueue.Count <= MaxDataLength)
            {// queue is not full yet
                Value += (input - Value) / AverageQueue.Count;
            }
            else
            { // queue is full. now in sliding window
                decimal change = weightedInput;
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
