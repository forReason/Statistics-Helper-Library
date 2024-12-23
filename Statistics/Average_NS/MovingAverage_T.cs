﻿using System.Numerics;
using System.Text;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// calculates a time-normalized moving average for generic number type.<br/>
    /// time-Normalized means that it is intended for uses where the input values are not in a steady timely manner.
    /// </summary>
    /// <remarks>Has an internal conversion to double so may not be suited for large number types such as decimal<br/>
    /// for decimal, please use <see cref="MovingAverage_Decimal"/>
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
    public class MovingAverage <T> where T : INumber<T>
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
        public MovingAverage(TimeSpan totalTime, TimeSpan valueResolution, string backupPath = "")
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
            Average = new SimpleMovingAverage_Double(Steps);
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
        private double CurrentTimeSpotVolumetricAverage = 0;
        private T PreviousValue = default;
        private SimpleMovingAverage_Double Average { get; set; }
        /// <summary>
        /// The current Moving Average
        /// </summary>
        public double Value { get; private set; }
        /// <summary>
        /// adds a single value point and assumes the date time of the value point is now
        /// </summary>
        /// <remarks>
        /// handy for live fed data
        /// </remarks>
        /// <param name="value"></param>
        public void AddValue(T value)
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
        public void AddValue(T value, DateTime timeStamp)
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

            double currentAssumption = Convert.ToDouble(PreviousValue + value) / 2.0;
            if (microTickTime.TotalSeconds == 0.0) CurrentTimeSpotVolumetricAverage = (Value + Convert.ToDouble(value)) / 2.0;
            else
            {
                CurrentTimeSpotVolumetricAverage = VolumetricAverage_Double.VolumeBasedAverage(CurrentTimeSpotVolumetricAverage, stepduration.TotalSeconds, currentAssumption, microTickTime.TotalSeconds);
            }
            PreviousValue = value;

            // add value to current time frame accumulation
            LastTimeStamp = timeStamp;
            TimeSpan currentSpotTimeSpan = LastTimeStamp - CurrentTimeSpot;
            
            // merge historic queue and previous time spot
            if (Average.CurrentDataLength > 0)
            {
                Value = VolumetricAverage_Double.VolumeBasedAverage(
                value1: Average.Value, volume1: (Average.CurrentDataLength * ValueResolution).TotalMinutes,
                value2: CurrentTimeSpotVolumetricAverage, volume2: currentSpotTimeSpan.TotalMinutes);
            } else
            {
                Value = CurrentTimeSpotVolumetricAverage;
            }
            
            if (Value != 0)
            {
                { }
            }
        }
        private void AddBackupValue(DateTime time, double value)
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
                    double value = double.Parse(split[1]);
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
            PreviousValue = default;
            Average.Clear();
            Value = 0;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
#endif
}
