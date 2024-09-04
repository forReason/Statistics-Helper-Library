using System.Globalization;

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// Provides a simple and efficient method for calculating the moving average over a sliding data window.
    /// This class is especially useful for large datasets or continuous data streams, as it maintains a fixed memory footprint based on the specified window size.
    /// </summary>
    /// <remarks>
    /// Memory usage grows linearly with the window size (`MaxDataLength`), not the total amount of data processed.
    /// For scenarios requiring very large window sizes, a <see cref="SimpleExponentialAverage_Double"/> may be more appropriate,
    /// as it uses constant memory by focusing on recent data points, albeit with reduced precision.
    /// </remarks>
    public class SimpleMovingAverage_Double
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMovingAverage_Double"/> class with a specified window size for the moving average.
        /// This window size determines the number of recent data points included in the calculation.
        /// </summary>
        /// <remarks>
        /// Memory usage grows linearly with the window size (`MaxDataLength`), making it suitable for large datasets with a manageable window size.
        /// For very large window sizes, consider using a <see cref="SimpleExponentialAverage_Double"/> to maintain constant memory usage,
        /// though with less precision due to its focus on recent data points.
        /// </remarks>
        /// <param name="length">The window size for the moving average, defining how many recent data points are considered.</param>
        /// <param name="backupPath">the path to a backup file. if set, every datapoint will be stored. WARNING with large and fast data!</param>
        public SimpleMovingAverage_Double(int length, string backupPath = "")
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

        public FileInfo? BackupFile { get; private set; }
        private Queue<string> BackupLines = new Queue<string>();
        /// <summary>
        /// Calculates the trend based on the moving average in order to obtain a percentage
        /// </summary>
        /// <remarks>
        /// The value is denoted as a percentage, e.g., -1.0 to X.0.
        /// </remarks>
        public double Trend
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
            if (BackupFile != null)
            {
                AddBackupValue(input);
                StoreBackup();
            }
        }
        private void AddBackupValue(double input)
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
                double value = double.Parse(line, CultureInfo.InvariantCulture);
                AverageQueue.Enqueue(value / MaxDataLength);
                if (AverageQueue.Count <= MaxDataLength)
                {
                    Value += (value - Value) / AverageQueue.Count;
                }
                else
                {
                    double change = value / MaxDataLength - AverageQueue.Dequeue();
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