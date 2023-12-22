using System.Text;

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// calculates a time-normalized moving average for type decimal.<br/>
    /// time-Normalized means that it is intended for uses where the input values are not in a steady timely manner.
    /// </summary>
    /// <remarks>
    /// <para><b>OVERVIEW</b></para>
    /// The MovingAverage_Double class provides a robust and efficient solution to compute moving averages over a specified duration, even with missing or real-time data.
    /// 
    /// <para><b>CLASS FUNCTIONALITY</b></para>
    /// The MovingAverage_Double class is designed to handle both real-time and historical data. It can accept individual data points with timestamps for real-time calculations 
    /// and pairs of value-timestamp for historical data. The class also manages data gaps intelligently by filling gaps with the last known average, ensuring continuity in the moving average calculations.
    /// 
    /// <para><b>BACKUP AND RESTORE FEATURE</b></para>
    /// This class also includes a backup and restore functionality which aids in persistent computations and disaster recovery. The backup is triggered each time a new time block is filled, 
    /// and the restore operation occurs during the initialization if a valid backup file path is provided. If the backupPath parameter is either null or an empty string, this feature is disabled.
    /// 
    /// <para><b>IMPLICATIONS OF THE BACKUP FEATURE</b></para>
    /// Please consider potential storage and performance implications of the backup feature. The size of the backup file will grow with more frequent data point additions or a longer totalTime period. 
    /// Rapid data point additions could affect performance due to frequent file writing operations. Likewise, a large backup file might slow down the initialization due to the time required for reading the file. 
    /// It is recommended to manage the frequency of backup operations and the size of the backup file according to your application's requirements and performance characteristics.
    /// 
    /// <para><b>Thread Safety</b></para>
    /// This class is not thread-safe. If instances of this class are accessed from multiple threads,
    /// external synchronization must be implemented. For example, you can use lock statements when calling the methods to ensure thread safety.<br/>
    /// note that the input time should be linear (i.e. sorted by date)
    /// </remarks>
    public class MovingAverage_Decimal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovingAverage_Decimal"/> class.
        /// </summary>
        /// <param name="totalTime">Specifies the total duration which the moving average calculation should consider.</param>
        /// <param name="valueResolution">The duration each data point should represent. Must be smaller than totalTime.</param>
        /// <param name="backupPath">Path to store/restore backups. If left empty or null, the backup feature will be disabled.</param>
        /// <remarks>
        /// <para><b>OVERVIEW</b></para>
        /// The MovingAverage_Double class provides a robust and efficient solution to compute moving averages over a specified duration, even with missing or real-time data.
        /// 
        /// <para><b>CLASS FUNCTIONALITY</b></para>
        /// The MovingAverage_Double class is designed to handle both real-time and historical data. It can accept individual data points with timestamps for real-time calculations 
        /// and pairs of value-timestamp for historical data. The class also manages data gaps intelligently by filling gaps with the last known average, ensuring continuity in the moving average calculations.
        /// 
        /// <para><b>BACKUP AND RESTORE FEATURE</b></para>
        /// This class also includes a backup and restore functionality which aids in persistent computations and disaster recovery. The backup is triggered each time a new time block is filled, 
        /// and the restore operation occurs during the initialization if a valid backup file path is provided. If the backupPath parameter is either null or an empty string, this feature is disabled.
        /// 
        /// <para><b>IMPLICATIONS OF THE BACKUP FEATURE</b></para>
        /// Please consider potential storage and performance implications of the backup feature. The size of the backup file will grow with more frequent data point additions or a longer totalTime period. 
        /// Rapid data point additions could affect performance due to frequent file writing operations. Likewise, a large backup file might slow down the initialization due to the time required for reading the file. 
        /// It is recommended to manage the frequency of backup operations and the size of the backup file according to your application's requirements and performance characteristics.
        /// 
        /// <para><b>Thread Safety</b></para>
        /// This class is not thread-safe. If instances of this class are accessed from multiple threads,
        /// external synchronization must be implemented. For example, you can use lock statements when calling the methods to ensure thread safety.<br/>
        /// note that the input time should be linear (i.e. sorted by date)
        /// </remarks>
        public MovingAverage_Decimal(TimeSpan totalTime, TimeSpan valueResolution, string backupPath = "")
        {
            SetResolution(totalTime, valueResolution);
            Clear();
            if (backupPath != "")
            {
                FileInfo backupFile = new FileInfo(backupPath);
                RestoreBackup(backupFile);
                // NOTE: THIS MUST OCCUR AFTER RESTOREBACKUP
                // OTHERWISE INFINITE LOOP
                BackupFile = backupFile;
            }
        }
        // "Settings"
        /// <summary>
        /// Specifies the total duration of data that the moving average calculation should consider. 
        /// This forms the "window" of data the moving average will consider at any given time.
        /// </summary>
        /// <remarks>
        /// Changing this value will adjust the resolution of the moving average calculation, 
        /// leading to a different granularity of the moving average. Note that changing this value 
        /// will invoke a recalculation of the moving average to reflect the updated duration.
        /// </remarks>
        public TimeSpan TotalTime
        {
            get { return _TotalTime; }
            set { SetResolution(value, ValueResolution); }
        }
        private TimeSpan _TotalTime;

        /// <summary>
        /// Specifies the duration each data point should represent in the moving average calculation.
        /// </summary>
        /// <remarks>
        /// This determines how much time each individual data point in the calculation accounts for, 
        /// which can affect the precision and performance of the moving average calculation. Note 
        /// that changing this value will invoke a recalculation of the moving average to reflect 
        /// the updated resolution.
        /// </remarks>
        public TimeSpan ValueResolution
        {
            get { return _ValueResolution; }
            set { SetResolution(TotalTime, value); }
        }
        private TimeSpan _ValueResolution;

        /// <summary>
        /// Represents the total number of steps that fit into the "TotalTime" window. This value 
        /// is calculated automatically when either the TotalTime or ValueResolution properties 
        /// are updated.
        /// </summary>
        private int Steps { get; set; }

        /// <summary>
        /// Sets a new resolution for the moving average calculation by specifying a new total 
        /// time and value resolution.
        /// </summary>
        /// <param name="totalTime">The new total time that the moving average calculation should consider.</param>
        /// <param name="valueResolution">The new duration each data point should represent.</param>
        /// <remarks>
        /// This function updates the TotalTime, ValueResolution, and Steps properties and invokes 
        /// a recalculation of the moving average to reflect the updated resolution.
        /// </remarks>
        public void SetResolution(TimeSpan totalTime, TimeSpan valueResolution)
        {
            this._TotalTime = totalTime;
            this._ValueResolution = valueResolution;
            Steps = (int)Math.Round((this.TotalTime.TotalMinutes / this.ValueResolution.TotalMinutes));
            Average = new SimpleMovingAverage_Decimal(Steps);
        }

        // Working variables
        /// <summary>
        /// the current slot of the resolution step grid
        /// </summary>
        private DateTime CurrentTimeSpot { get; set; }
        /// <summary>
        /// the last added timespan
        /// </summary>
        private DateTime LastTimeStamp { get; set; }
        /// <summary>
        /// the path where the Backup (if desired) should be saved / loaded
        /// </summary>
        /// <remarks>
        /// if left blanc (null), disables backup feature
        /// </remarks>
        public FileInfo? BackupFile { get; set; }
        private decimal CurrentTimeSpotVolumetricAverage = 0;
        private decimal PreviousValue = 0;
        private SimpleMovingAverage_Decimal Average { get; set; }
        /// <summary>
        /// The current moving average value.
        /// </summary>
        public decimal Value { get; private set; }
        /// <summary>
        /// Specifies the absolute rate of change by which the data set moved on average at each data point.
        /// </summary>
        public decimal AbsoluteRateOfChange { get { return (Average.NewestElement - CurrentTimeSpotVolumetricAverage) / Average.CurrentDataLength + 1; } }

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
        public decimal DeviationFromMA_Absolute { get { return PreviousValue - Value; } }

        /// <summary>
        /// Calculates the percentage-based deviation between the current data point and the Simple Moving Average (SMA).<br/>
        /// The percentage-based deviation shows how much the current value deviates from the SMA relative to the SMA itself.
        /// </summary>
        /// <remarks>
        /// Positive values indicate the current value is greater than the SMA, often interpreted as bullish.<br/>
        /// Negative values indicate the current value is less than the SMA, often interpreted as bearish.<br/>
        /// The value is denoted as a percentage, e.g., -1.0 to +X.0.
        /// </remarks>
        public decimal DeviationFromMA_Percentage { get { return DeviationFromMA_Absolute / Value; } }
        /// <summary>
        /// Adds a single value point with the current timestamp. 
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <param name="interpolateMissingData">this flag can be used if you did not gater data at all in between this and the last point to interpolate</param>
        /// <remarks>
        /// This method is particularly useful for live fed data.
        /// </remarks>
        public void AddValue(decimal value, bool interpolateMissingData = false)
        {
            AddValue(value, DateTime.Now, interpolateMissingData);
        }
        /// <summary>
        /// Adds a single data point with a specific timestamp. 
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <param name="valueTimeStamp">The timestamp of the value.</param>
        /// <param name="interpolateMissingData">this flag can be used if you did not gater data at all in between this and the last point to interpolate</param>
        /// <remarks>
        /// This method is useful for handling historical data. 
        /// It's important to note that value points always need to be added in chronological order.
        /// </remarks>
        public void AddValue(decimal value, DateTime valueTimeStamp, bool interpolateMissingData = false)
        {
            DateTime valueTimeSlot = CalculateTimeSpotStart(valueTimeStamp);
            // algorythm to decide if a new timeSpot (epoch) is to be opened and handling Data Gaps 
            bool shouldCreateNewTimeSpot = CurrentTimeSpot < valueTimeSlot; // the valueTimeStamp is newer than the current epoch reach
            if (shouldCreateNewTimeSpot)
            {
                AddDataPoints(valueTimeSlot, interpolateMissingData, value); // handles Data gaps as well
                // prepare for new Time Spot
                CurrentTimeSpot = CalculateTimeSpotStart(valueTimeStamp); // Use valueTimeStamp
                LastTimeStamp = CurrentTimeSpot;
                CurrentTimeSpotVolumetricAverage = value;
                // Initialize with the first value only if no value has been added before
                if (LastTimeStamp == DateTime.MinValue)
                {
                    if (Average.CurrentDataLength == 0)  // Check if Average is empty
                    {
                        Average.AddValue(value);
                        PreviousValue = value;
                    }
                }
            }

            // Calculate the volumetric average for the new data point. This involves determining the
            // average value for the current time spot based on the incoming value and the time elapsed.
            // The volumetric average is a critical component in maintaining an accurate moving average,
            // especially when data points are not uniformly distributed over time.
            GenerateVolumetricAverage(value, valueTimeStamp);



            // Update the moving average with the new volumetric average. This step combines the current
            // time spot's volumetric average with the historical data to calculate the updated moving average.
            // It ensures that the moving average reflects the most recent data while considering the historical context.
            UpdateMovingAverage();
            { }
        }
        /// <summary>
        /// calculates the apropriate TimeSpot null aligned to prevent aliasing Issues
        /// </summary>
        /// <param name="valueTimeStamp"></param>
        /// <returns></returns>
        private DateTime CalculateTimeSpotStart(DateTime valueTimeStamp)
        {
            var timeSinceMinValue = valueTimeStamp - DateTime.MinValue;
            var totalIntervals = (long)(timeSinceMinValue.Ticks / ValueResolution.Ticks);
            return DateTime.MinValue + new TimeSpan(totalIntervals * ValueResolution.Ticks);
        }
        /// <summary>
        /// Handles data gaps between the current data point and the previous one.
        /// </summary>
        /// <param name="valueTimeSlot">The time slot of the current value being added.</param>
        /// <param name="interpolateMissingData">this flag can be used if you did not gater data at all in between this and the last point to interpolate</param>
        /// <remarks>
        /// This method addresses both large and small data gaps. For large gaps, exceeding the entire rolling window, 
        /// it clears historical data and restarts calculations. For smaller gaps, it fills in missing steps to maintain 
        /// continuity in the moving average calculation.
        /// </remarks>
        private void AddDataPoints(DateTime valueTimeSlot, bool interpolateMissingData = false, decimal? value = null)
        {
            decimal newValue = CurrentTimeSpotVolumetricAverage;
            if (value != null)
                newValue = value.Value;
            TimeSpan dataGap = (valueTimeSlot - CurrentTimeSpot);
            int missingSteps = (int)(dataGap / ValueResolution);
            if (dataGap > TotalTime)// the data gap is larger than the entire rolling time window
            {
                // clear the values and start freshly
                CurrentTimeSpot = CalculateTimeSpotStart(valueTimeSlot - TotalTime);
                Average.Clear();
                BackupLines.Clear();

                if (interpolateMissingData)
                {
                    // Interpolation logic for large data gap
                    decimal slope = (newValue - PreviousValue) / (missingSteps);  // Adjust the slope calculation
                    for (int i = Steps; i > 0; i--)
                    {
                        decimal interpolatedValue = newValue - slope * i;
                        Average.AddValue(interpolatedValue);
                        AddBackupValue(CurrentTimeSpot, interpolatedValue);
                    }
                }
            }
            else if (dataGap > ValueResolution) // we have at least 1 missing entry
            {
                for (int i = 0; i < missingSteps; i++)
                {
                    decimal valueToAdd = CurrentTimeSpotVolumetricAverage;
                    if (interpolateMissingData)
                    {
                        // Interpolation logic for each missing step
                        decimal interpolationStep = (valueToAdd - PreviousValue) / (missingSteps + 1);
                        valueToAdd = PreviousValue + interpolationStep * (i + 1);

                    }

                    Average.AddValue(valueToAdd);
                    AddBackupValue(CurrentTimeSpot, valueToAdd);
                    CurrentTimeSpot += ValueResolution;
                }
            }
            else // no missing entry, but new time spot
            {
                Average.AddValue(CurrentTimeSpotVolumetricAverage);
                AddBackupValue(CurrentTimeSpot, CurrentTimeSpotVolumetricAverage);
            }
            StoreBackup();
        }


        /// <summary>
        /// Calculates the volumetric average for a new data point.
        /// </summary>
        /// <param name="value">The new data value to be added.</param>
        /// <param name="valueTimeStamp">The timestamp associated with the new data value.</param>
        /// <remarks>
        /// This calculation involves determining the average value for the current time spot based on the incoming value and time elapsed.
        /// It's a critical component in maintaining an accurate moving average, especially for data points not uniformly distributed over time.
        /// </remarks>
        private void GenerateVolumetricAverage(decimal value, DateTime valueTimeStamp)
        {
            TimeSpan microTickTime = valueTimeStamp - LastTimeStamp;
            TimeSpan stepDuration = valueTimeStamp - CurrentTimeSpot - microTickTime;
            // validate Timestamps
            if (stepDuration.TotalSeconds < 0 || microTickTime.TotalSeconds < 0)
            {
                throw new InvalidOperationException("You cannot add data points from the past! Did you forget to Clear()?");
            }

            // calculate Moving Average
            CurrentTimeSpotVolumetricAverage = VolumetricAverage_Decimal.VolumeBasedAverage(CurrentTimeSpotVolumetricAverage, (decimal)stepDuration.TotalSeconds, value, (decimal)microTickTime.TotalSeconds);

            PreviousValue = value;
            LastTimeStamp = valueTimeStamp;
        }

        /// <summary>
        /// Updates the moving average with the newly calculated volumetric average.
        /// </summary>
        /// <remarks>
        /// This method combines the current time spot's volumetric average with historical data to compute the updated moving average.
        /// It ensures that the moving average reflects the most recent data while accounting for the historical context.
        /// </remarks>
        private void UpdateMovingAverage()
        {
            TimeSpan currentSpotTimeSpan = LastTimeStamp - CurrentTimeSpot;

            if (Average.CurrentDataLength > 0)
            {
                Value = VolumetricAverage_Decimal.VolumeBasedAverage(
                    value1: Average.Value, volume1: (decimal)(Average.CurrentDataLength * ValueResolution).TotalMinutes,
                    value2: CurrentTimeSpotVolumetricAverage, volume2: (decimal)currentSpotTimeSpan.TotalMinutes);
            }
            else
            {
                Value = CurrentTimeSpotVolumetricAverage;
            }
        }
        /// <summary>
        /// calculates the current Moving average, even when no new elements have been added
        /// </summary>
        /// <param name="requestedTime">defaults to DateTime.Now, use when reading historic Data</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public decimal GetCurrentMovingAverage(DateTime? requestedTime)
        {
            DateTime time = DateTime.Now;
            if (requestedTime != null)
                time = requestedTime.Value;

            if (requestedTime < LastTimeStamp)
                throw new InvalidOperationException("you cannot get the ma of the past");
            if (Average.CurrentDataLength > 0)
            {
                TimeSpan currentSpotTimeSpan = time - CurrentTimeSpot;
                return VolumetricAverage_Decimal.VolumeBasedAverage(
                    value1: Average.Value, volume1: (decimal)(Average.CurrentDataLength * ValueResolution).TotalMinutes,
                    value2: CurrentTimeSpotVolumetricAverage, volume2: (decimal)currentSpotTimeSpan.TotalMinutes);
            }
            else
            {
                return CurrentTimeSpotVolumetricAverage;
            }
        }

        private void AddBackupValue(DateTime time, decimal value)
        {
            if (BackupFile == null)
            {
                return;
            }
            BackupStringBuilder.Append(time);
            BackupStringBuilder.Append(';');
            BackupStringBuilder.Append(value);
            BackupLines.Enqueue(BackupStringBuilder.ToString());
            BackupStringBuilder.Clear();
            if (BackupLines.Count > Average.MaxDataLength)
            {
                BackupLines.Dequeue();
            }
        }
        private void StoreBackup()
        {
            if (BackupFile == null)
            { return; }
            BackupFile.Directory.Create();
            File.WriteAllLines(BackupFile.FullName, BackupLines);
        }
        StringBuilder BackupStringBuilder = new StringBuilder();
        Queue<string> BackupLines = new Queue<string>();
        private void RestoreBackup(FileInfo backupFile)
        {
            if (backupFile.Exists)
            {
                string[] lines = File.ReadAllLines(backupFile.FullName);
                foreach (string line in lines)
                {
                    string[] split = line.Split(';');
                    DateTime time = DateTime.Parse(split[0]);
                    decimal value = decimal.Parse(split[1]);
                    Average.AddValue(value);
                    BackupLines.Enqueue(line);
                }
            }
        }
        /// <summary>
        /// Resets the moving average to its initial state, clearing all internal values.
        /// </summary>
        /// <remarks>
        /// The time settings (total time and value resolution) persist after a clear operation.
        /// </remarks>
        public void Clear()
        {
            CurrentTimeSpot = DateTime.MinValue;
            LastTimeStamp = DateTime.MinValue;
            //CurrentTimeSpotAverage.Clear();
            CurrentTimeSpotVolumetricAverage = 0;
            PreviousValue = 0;
            Average.Clear();
            Value = 0;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }

    }
}
