using System.Globalization;
using System.Text;

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// A method to calculate the moving average on sliding data using a fixed-size window.
    /// This implementation is particularly useful for infinite or unbounded data streams,
    /// as it maintains a constant memory footprint by only storing data points within the defined window size (`MaxDataLength`).
    /// </summary>
    /// <remarks>
    /// Memory requirement grows linearly with the window size (`MaxDataLength`), not with the total data inflow.
    /// For very large window sizes, a <see cref="SimpleExponentialAverage_Decimal"/> may be more suitable,
    /// as it uses constant memory and focuses on recent data points, albeit with a loss in precision.
    /// </remarks>
    public class SimpleMovingAverage_Decimal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMovingAverage_Decimal"/> class with a specified window size for the moving average.
        /// This window size defines how many of the most recent data points are used in the calculation.
        /// </summary>
        /// <remarks>
        /// Memory usage grows linearly with the window size (`MaxDataLength`), not the total amount of data processed.
        /// For scenarios requiring very large window sizes, consider using a <see cref="SimpleExponentialAverage_Decimal"/>,
        /// which maintains a constant memory footprint by focusing on recent data points, albeit with reduced precision.
        /// </remarks>
        /// <param name="length">The fixed window size for the moving average, determining how many recent data points are considered.</param>
        /// <param name="backupPath">the path to a backup file. if set, every datapoint will be stored. WARNING with large and fast data!</param>
        public SimpleMovingAverage_Decimal(int length, string backupPath = "")
        {
            MaxDataLength = length;
            Clear();
            if (!string.IsNullOrEmpty(backupPath))
            {
                BackupFile = new FileInfo(backupPath);
                RestoreBackup();
            }
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

        public FileInfo? BackupFile { get; private set; }
        private Queue<string> BackupLines = new Queue<string>();
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
            if (BackupFile != null)
            {
                AddBackupValue(input);
                StoreBackup();
            }
        }
        private void AddBackupValue(decimal input)
        {
            if (BackupFile == null) return;
            
            BackupLines.Enqueue(input.ToString(CultureInfo.InvariantCulture));

            if (BackupLines.Count > MaxDataLength)
            {
                BackupLines.Dequeue();
            }
        }

        private void StoreBackup()
        {
            if (BackupFile == null) return;

            BackupFile.Directory.Create();
            File.WriteAllLines(BackupFile.FullName, BackupLines);
        }

        private void RestoreBackup()
        {
            if (BackupFile?.Exists != true) return;

            string[] lines = File.ReadAllLines(BackupFile.FullName);
            foreach (string line in lines)
            {
                decimal value = decimal.Parse(line, CultureInfo.InvariantCulture);
                AverageQueue.Enqueue(value / MaxDataLength);
                if (AverageQueue.Count <= MaxDataLength)
                {
                    Value += (value - Value) / AverageQueue.Count;
                }
                else
                {
                    decimal change = value / MaxDataLength - AverageQueue.Dequeue();
                    Value += change;
                }
            }
        }
        /// <summary>
        /// Clears all data points and resets the moving average to zero.
        /// </summary>
        public void Clear()
        {
            AverageQueue.Clear();
            Value = 0;
            BackupLines.Clear();
        }

        /// <returns>The current SMA value as a string.</returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
