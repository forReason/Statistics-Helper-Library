namespace QuickStatistics.Net.Variance_NS
{
    /// <summary>
    /// Implements the calculation of exponential standard deviation.
    /// </summary>
    public class ExponentialStandardDeviation
    {
        private double sum = 0.0;
        private double sumOfSquares = 0.0;
        private double count = 0;
        private DateTime lastDecayTimestamp;
        private readonly TimeSpan halfLife;
        private readonly object lockObj = new object();

        /// <summary>
        /// Path for the backup file.
        /// </summary>
        public string? BackupPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialStandardDeviation"/> class.
        /// </summary>
        /// <param name="approximateTimespan">The approximate timespan for calculating half-life.</param>
        /// <param name="backupPath">The path for the backup file.</param>
        public ExponentialStandardDeviation(TimeSpan approximateTimespan, string backupPath = null)
        {
            this.halfLife = CalculateHalfLife(approximateTimespan);
            this.BackupPath = backupPath;
            if (BackupPath != null)
            {
                RestoreBackup();
            }
        }

        /// <summary>
        /// Calculates the half-life of the decay based on the approximate timespan.
        /// </summary>
        /// <param name="approximateTimespan">The approximate timespan for the half-life.</param>
        /// <returns>Returns the calculated half-life as a <see cref="TimeSpan"/>.</returns>
        private static TimeSpan CalculateHalfLife(TimeSpan approximateTimespan)
        {
            double timespanDays = approximateTimespan.TotalDays;
            double halfLifeDays = timespanDays / Math.Log(2);
            return TimeSpan.FromDays(halfLifeDays);
        }

        /// <summary>
        /// Applies the decay factor to the sum, sum of squares, and count.
        /// <br/>
        /// This is based on the time elapsed since the last decay.
        /// </summary>
        /// <param name="currentTimestamp">The current timestamp for decay application.</param>
        private void ApplyDecay(DateTime currentTimestamp)
        {
            lock (lockObj)
            {
                if (lastDecayTimestamp != default)
                {
                    TimeSpan elapsed = currentTimestamp - lastDecayTimestamp;
                    double decayFactor = Math.Pow(0.5, elapsed.TotalSeconds / halfLife.TotalSeconds);
                    sum *= decayFactor;
                    sumOfSquares *= decayFactor;
                    count *= decayFactor;
                }
                lastDecayTimestamp = currentTimestamp;
            }
        }

        /// <summary>
        /// Adds a value to the statistical data and applies decay.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <param name="timestamp">Optional timestamp. If not provided, uses current time.</param>
        public void AddValue(double value, DateTime? timestamp = null)
        {
            if (timestamp == null) timestamp = DateTime.Now;
            lock (lockObj)
            {
                ApplyDecay(timestamp.Value);
                sum += value;
                sumOfSquares += value * value;
                count += 1.0;
                StoreBackup();
            }
        }

        /// <summary>
        /// Calculates the exponential standard deviation up to the current timestamp.
        /// </summary>
        /// <param name="currentTimestamp">Optional current timestamp. If not provided, uses current time.</param>
        /// <returns>Returns the calculated exponential standard deviation.</returns>
        public double CalculateExponentialStdDev(DateTime? currentTimestamp = null)
        {
            if (currentTimestamp == null) currentTimestamp = DateTime.Now;
            lock (lockObj)
            {
                ApplyDecay(currentTimestamp.Value);

                if (count < 2) return 0;

                double mean = sum / count;
                double variance = (sumOfSquares / count) - (mean * mean);

                return Math.Sqrt(variance);
            }
        }
        public double CalculateExponentialStdDevPercent(DateTime? currentTimestamp = null)
        {
            if (currentTimestamp == null) currentTimestamp = DateTime.Now;
            lock (lockObj)
            {
                ApplyDecay(currentTimestamp.Value);

                if (count < 2) return 0;

                double mean = sum / count;
                double variance = (sumOfSquares / count) - (mean * mean);

                // Convert standard deviation to percentage of the mean
                double stdDevPercentage = (Math.Sqrt(variance) / mean) * 100;

                return stdDevPercentage;
            }
        }


        /// <summary>
        /// Stores the current state to a backup file.
        /// </summary>
        private void StoreBackup()
        {
            if (BackupPath == null)
            { return; }

            try
            {
                using StreamWriter sw = new StreamWriter(BackupPath);
                sw.WriteLine($"{sum};{sumOfSquares};{count};{lastDecayTimestamp}");
            }
            catch (Exception)
            {
                // Log an error or issue a warning if writing to the file failed.
            }
        }


        /// <summary>
        /// Restores the state from a backup file.
        /// </summary>
        private void RestoreBackup()
        {
            if (BackupPath == null || !File.Exists(BackupPath))
            { return; }

            string line = File.ReadAllText(BackupPath);

            if (string.IsNullOrWhiteSpace(line))
            {
                // Log an error or issue a warning if backup file is empty.
                return;
            }

            string[] split = line.Split(';');

            if (split.Length < 4)
            {
                // Log an error or issue a warning if the backup file format is incorrect.
                return;
            }

            try
            {
                sum = double.Parse(split[0]);
                sumOfSquares = double.Parse(split[1]);
                count = double.Parse(split[2]);
                lastDecayTimestamp = DateTime.Parse(split[3]);
            }
            catch (Exception)
            {
                // Log an error or issue a warning if parsing failed.
            }
        }

    }
}
