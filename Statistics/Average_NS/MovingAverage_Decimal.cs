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
        /// Initializes a new instance of the <see cref="MovingAverage_Double"/> class.
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
        private DateTime CurrentTimeSpot { get; set; }
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
        /// The current Moving Average
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
        /// adds a single value point<br/>
        /// assumes the date time of the value point is NOW
        /// </summary>
        /// <remarks>
        /// handy for live fed data
        /// </remarks>
        /// <param name="value"></param>
        public void AddValue(decimal value)
        {
            AddValue(value, DateTime.Now);
        }
        /// <summary>
        /// adds a single data point based on the points date. </br>
        /// handy for historical data
        /// </summary>
        /// <remarks>
        /// Value points always need to be added in series
        /// </remarks>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        public void AddValue(decimal value, DateTime timeStamp)
        {
            /// check if new value needs to be added to queue
            if (CurrentTimeSpot + ValueResolution < timeStamp)
            { // new time spot needs to be created
                if (CurrentTimeSpot != default)
                {
                    /// the previous time spot needs to be added according to it's duration
                    if (CurrentTimeSpot + TotalTime < timeStamp)
                    { // There is a huge data gap. skip part to save processing time
                        CurrentTimeSpot = timeStamp - TotalTime;
                        Average.Clear();
                        BackupLines.Clear();
                    }
                    int missingSteps = (int)Math.Round((timeStamp - CurrentTimeSpot).TotalMinutes / ValueResolution.TotalMinutes);
                    for (int i = 0; i < missingSteps; i++)
                    { // fill up gap
                        Average.AddValue(CurrentTimeSpotVolumetricAverage);
                        AddBackupValue(CurrentTimeSpot, CurrentTimeSpotVolumetricAverage);
                        CurrentTimeSpot += ValueResolution;
                    }
                    StoreBackup();
                }
                // prepare for new timeSpot
                CurrentTimeSpot = LastTimeStamp;
                //LastTimeStamp = timeStamp;
                CurrentTimeSpotVolumetricAverage = 0;
            }
            // generate volumetric average
            TimeSpan microTickTime = timeStamp - LastTimeStamp;
            TimeSpan stepduration = timeStamp - CurrentTimeSpot - microTickTime;
            if (stepduration.TotalSeconds < 0 || microTickTime.TotalSeconds < 0)
            {
                throw new InvalidOperationException("you cannot add data points from the past! Did you forget to Clear()?");
            }

            decimal currentAssumption = (PreviousValue + value) / 2;
            if (microTickTime.TotalSeconds == 0.0) CurrentTimeSpotVolumetricAverage = (Value + value) / 2;
            else
            {
                CurrentTimeSpotVolumetricAverage = VolumetricAverage_Decimal.VolumeBasedAverage(
                    value1: CurrentTimeSpotVolumetricAverage, volume1: (decimal)stepduration.TotalSeconds,
                    value2: currentAssumption, volume2: (decimal)microTickTime.TotalSeconds);
            }
            PreviousValue = value;

            // add value to current time frame accumulation
            LastTimeStamp = timeStamp;
            TimeSpan currentSpotTimeSpan = LastTimeStamp - CurrentTimeSpot;
            
            // merge historic queue and previous time spot
            if (Average.CurrentDataLength > 0)
            {
                Value = VolumetricAverage_Decimal.VolumeBasedAverage(
                value1: Average.Value, volume1: (decimal)(Average.CurrentDataLength * ValueResolution).TotalMinutes,
                value2: CurrentTimeSpotVolumetricAverage, volume2: (decimal)currentSpotTimeSpan.TotalMinutes);
            } else
            {
                Value = CurrentTimeSpotVolumetricAverage;
            }
            
            if (Value != 0)
            {
                { }
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
        /// resets the average to 0 and clears all its internal values.
        /// </summary>
        /// <remarks>
        /// setting (time) persist
        /// </remarks>
        public void Clear()
        {
            CurrentTimeSpot = DateTime.MinValue;
            LastTimeStamp = DateTime.MinValue;
            //CurrentTimeSpotAverage.Clear();
            CurrentTimeSpotVolumetricAverage= 0;
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
