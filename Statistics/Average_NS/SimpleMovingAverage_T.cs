using System.Globalization;
using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// A generic, efficient method for calculating the moving average on sliding data using a specified numeric type.
    /// This class is particularly useful for large datasets or continuous data streams, maintaining a fixed-size window for calculations.
    /// </summary>
    /// <remarks>
    /// Internally converts data to double for calculations. For data types requiring higher precision, consider using <see cref="SimpleMovingAverage_Decimal"/>.
    /// Ensure that the window size is set appropriately to balance memory usage and performance.
    /// </remarks>
    public class SimpleMovingAverage<T> where T : INumber<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMovingAverage{T}"/> class with a specified window size and optional backup functionality.
        /// </summary>
        /// <param name="length">The window size for the moving average, defining how many recent data points are included in the calculation.</param>
        /// <param name="backupPath">Optional path for backup storage, enabling restoration of state in case of interruptions.</param>
        /// <param name="backupPath">the path to a backup file. if set, every datapoint will be stored. WARNING with large and fast data!</param>
        public SimpleMovingAverage(int length, string backupPath = "")
        {
            MaxDataLength = length;
            Clear();
            if (!string.IsNullOrEmpty(backupPath))
            {
                BackupFile = new FileInfo(backupPath);
                RestoreBackup();
            }
        }
        public FileInfo? BackupFile { get; private set; }
        private Queue<string> BackupLines = new Queue<string>();
        private Queue<double> AverageQueue = new Queue<double>();
        public double Value { get; private set; }
        public int MaxDataLength { get; private set; }
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        public void AddValue(T input)
        {
            double weightedInput = Convert.ToDouble(input) / MaxDataLength;
            AverageQueue.Enqueue(weightedInput);
            if (AverageQueue.Count <= MaxDataLength)
            {// queue is not full yet
                Value += (Convert.ToDouble(input) - Value) / AverageQueue.Count;
            }
            else
            { // queue is full. now in sliding window
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
        private void AddBackupValue(T input)
        {
            if (BackupFile == null) return;

            // Use invariant culture to convert the input to a string.
            BackupLines.Enqueue(input.ToString("G", CultureInfo.InvariantCulture));

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
                // Parse the value from the backup line using invariant culture.
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
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
#endif
}
