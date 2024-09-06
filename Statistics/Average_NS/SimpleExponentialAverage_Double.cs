using System;
using System.Globalization;
using System.IO;

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// Provides a lightweight and fast method to compute a simple exponential moving average for double values.
    /// This class is particularly useful for time series data where quick calculations are needed, and the data size is large.
    /// </summary>
    /// <remarks>
    /// The exponential moving average is a type of weighted moving average that gives more weight to recent data points,
    /// making it more responsive to recent changes compared to a simple moving average.
    /// </remarks>
    public class SimpleExponentialAverage_Double
    {
        private uint _MaxDataLength;
        private uint _CorrectedDataLength;
        private double _DivergenceCorrection;
        private uint _CurrentDataLength { get; set; }
        public FileInfo? BackupFile { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleExponentialAverage_Double"/> class with specified settings.
        /// </summary>
        /// <param name="maxDataLength">The maximum number of data points to track for the moving average.</param>
        /// <param name="divergenceCorrection">A correction factor applied to reduce lag in the moving average calculation.</param>
        /// <param name="backupPath">The file path for storing backup data. If null or empty, backup is disabled.</param>
        public SimpleExponentialAverage_Double(
            uint maxDataLength,
            double divergenceCorrection = 0.29296875,
            string backupPath = "")
        {
            _DivergenceCorrection = divergenceCorrection;
            MaxDataLength = maxDataLength;
            Clear();

            if (!string.IsNullOrEmpty(backupPath))
            {
                BackupFile = new FileInfo(backupPath);
                RestoreBackup();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of data points the moving average should consider.
        /// Adjusting this value recalculates the corrected data length based on the divergence correction.
        /// </summary>
        public uint MaxDataLength
        {
            get => _MaxDataLength;
            set
            {
                _MaxDataLength = value;
                SetMax();
            }
        }

        /// <summary>
        /// Gets the actual number of data points currently tracked, adjusted by the divergence correction.
        /// </summary>
        public uint CurrentDataLength => (uint)(_CurrentDataLength / DivergenceCorrection);

        /// <summary>
        /// Gets the current value of the exponential moving average.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Gets or sets the divergence correction factor that adjusts the responsiveness of the moving average.
        /// </summary>
        /// <remarks>
        /// A lower value will make the moving average more responsive to changes, while a higher value will smooth out fluctuations more.
        /// </remarks>
        public double DivergenceCorrection
        {
            get => _DivergenceCorrection;
            set
            {
                _DivergenceCorrection = value;
                SetMax();
            }
        }

        /// <summary>
        /// Resets the moving average calculation, clearing all tracked data points and resetting the average value to zero.
        /// </summary>
        public void Clear()
        {
            _CurrentDataLength = 0;
            Value = 0;
        }

        /// <summary>
        /// Adds a new data point to the moving average calculation.
        /// </summary>
        /// <param name="input">The new data point to include in the moving average calculation.</param>
        public void AddValue(double input)
        {
            _CurrentDataLength++;
            Value += (input - Value) / _CurrentDataLength;
            _CurrentDataLength -= _CurrentDataLength / (_CorrectedDataLength + 1);

            if (BackupFile != null)
            {
                StoreBackup();
            }
        }

        /// <summary>
        /// Stores the current state of the moving average to a backup file.
        /// </summary>
        private void StoreBackup()
        {
            if (BackupFile == null) return;

            // Use a temporary file to ensure atomic write operations.
            string tempFilePath = BackupFile.FullName + ".tmp";

            try
            {
                // Write the state to a temporary file.
                using (var writer = new StreamWriter(tempFilePath))
                {
                    writer.WriteLine(Value.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(_CurrentDataLength.ToString(CultureInfo.InvariantCulture));
                }

                // Check if the original backup file exists.
                if (File.Exists(BackupFile.FullName))
                {
                    // Replace the old backup file with the new one atomically.
                    File.Replace(tempFilePath, BackupFile.FullName, null);
                }
                else
                {
                    // If the backup file doesn't exist, just move the temp file to the backup file location.
                    File.Move(tempFilePath, BackupFile.FullName);
                }
            }
            catch (Exception ex)
            {
                // Handle potential I/O exceptions.
                Console.WriteLine($"Error storing backup: {ex.Message}");
            }
        }

        /// <summary>
        /// Restores the moving average state from a backup file, if available.
        /// </summary>
        private void RestoreBackup()
        {
            if (BackupFile?.Exists != true) return;

            try
            {
                var lines = File.ReadAllLines(BackupFile.FullName);
                if (lines.Length >= 2 &&
                    double.TryParse(lines[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var value) &&
                    uint.TryParse(lines[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var currentLength))
                {
                    Value = value;
                    _CurrentDataLength = currentLength;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., corrupted files or missing data.
                Console.WriteLine($"Error restoring backup: {ex.Message}");
                Clear(); // Reset state if restore fails to avoid inconsistent state.
            }
        }

        /// <summary>
        /// Sets the corrected maximum data length based on the divergence correction factor.
        /// </summary>
        private void SetMax()
        {
            _CorrectedDataLength = (uint)(_MaxDataLength * _DivergenceCorrection);
            uint overflow = _CurrentDataLength - _CorrectedDataLength;
            overflow = Math.Max(overflow, 0);
            _CurrentDataLength -= overflow;
        }

        /// <summary>
        /// Returns the current value of the exponential moving average as a string.
        /// </summary>
        /// <returns>A string representation of the current moving average value.</returns>
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
