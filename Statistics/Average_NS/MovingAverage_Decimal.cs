using System.Text;

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// calculates a time-normalized moving average for type decimal.<br/>
    /// time-Normalized means that it is intended for uses where the input values are not in a steady timely manner.
    /// </summary>
    public class MovingAverage_Decimal
    {
        /// <summary>
        /// calculates a time-normalized moving average for type double.<br/>
        /// time-Normalized means that it is intended for uses where the input values are not in a steady timely manner.
        /// </summary>
        /// <param name="totalTime">the total timespan which shoud be tracked</param>
        /// <param name="valueResolution">the duration each datapoint should cover ( must be smaller than totaltime )</param>
        /// <param name="backupPath">(where to stora / restore backups)</param>
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
        ///  TotalTime specifies window of time in the past which should keepp track of (eg last 2hours)
        /// </summary>
        public TimeSpan TotalTime { get; private set; }
        /// <summary>
        /// ValueResolution specifies how much time be consolidated into one dataPoint (mainly Memory saving Feature)
        /// </summary>
        public TimeSpan ValueResolution { get; private set; }
        /// <summary>
        /// the total amount of steps which fit into the Time Window "TotalTime"
        /// </summary>
        private int Steps { get; set; }
        public void SetResolution(TimeSpan totalTime, TimeSpan valueResolution) 
        {
            this.TotalTime = totalTime;
            this.ValueResolution = valueResolution;
            Steps = (int)Math.Round((this.TotalTime.TotalMinutes / this.ValueResolution.TotalMinutes));
            Average = new SimpleMovingAverage_Decimal(Steps);
        }
        // Working variables
        private DateTime CurrentTimeSpot { get; set; }
        private DateTime LastTimeStamp { get; set; }
        public FileInfo? BackupFile { get; set; }
        private decimal CurrentTimeSpotVolumetricAverage = 0;
        private decimal PreviousValue = 0;
        private SimpleMovingAverage_Decimal Average { get; set; }
        /// <summary>
        /// Value represents the current Moving Average
        /// </summary>
        public decimal Value { get; private set; }
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
