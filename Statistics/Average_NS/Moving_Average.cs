using Statistics.Objects;
using System.Text;

namespace Statistics.Average_NS
{
    public class Moving_Average_Double
    {
        public Moving_Average_Double(TimeSpan totalTime, TimeSpan valueResolution, string backupPath = "")
        {
            SetResolution(totalTime, valueResolution);
            Clear();
            RestoreBackup(backupPath);
            // NOTE: THIS MUST OCCUR AFTER RESTOREBACKUP
            // OTHERWISE INFINITE LOOP
            BackupPath = backupPath;
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
        private int Steps { get; set; }
        public void SetResolution(TimeSpan totalTime, TimeSpan valueResolution) 
        {
            this.TotalTime = totalTime;
            this.ValueResolution = valueResolution;
            Steps = (int)(this.TotalTime.TotalMinutes / this.ValueResolution.TotalMinutes);
            Average = new Simple_Moving_Average_Double(Steps);
        }
        // Working variables
        private DateTime CurrentTimeSpot { get; set; }
        private DateTime LastTimeStamp { get; set; }
        public string BackupPath { get; set; }
        private Progressing_Average_Double CurrentTimeSpotAverage = new Progressing_Average_Double();
        private Simple_Moving_Average_Double Average { get; set; }
        /// <summary>
        /// Value represents the current Moving Average
        /// </summary>
        public double Value { get; private set; }
        /// <summary>
        /// adds a single value point and assumes the date time of the value point is now
        /// </summary>
        /// <remarks>
        /// handy for live fed data
        /// </remarks>
        /// <param name="value"></param>
        public void AddValue(double value)
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
        public void AddValue(double value, DateTime timeStamp)
        {
            LastTimeStamp = timeStamp;
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
                    int missingSteps = (int)((timeStamp - CurrentTimeSpot)/ ValueResolution);
                    for (int i = 0; i < missingSteps; i++)
                    { // fill up gap
                        Average.AddValue(CurrentTimeSpotAverage.Value);
                        AddBackupValue(CurrentTimeSpot, CurrentTimeSpotAverage.Value);
                        CurrentTimeSpot += ValueResolution;
                    }
                    StoreBackup();
                }
                // prepare for new timeSpot
                CurrentTimeSpot = timeStamp;
                CurrentTimeSpotAverage.Clear();
            }
            // add value to current time frame accumulation
            CurrentTimeSpotAverage.AddValue(value);
            TimeSpan currentSpotTimeSpan = LastTimeStamp - CurrentTimeSpot; 
            // merge historic queue and previous time spot
            Value = Volumetric_Average.VolumeBasedAverage(
                value1: Average.Value, volume1: (Average.CurrentDataLength * ValueResolution).TotalMinutes,
                value2: CurrentTimeSpotAverage.Value, volume2: currentSpotTimeSpan.TotalMinutes);
            if (Value != 0)
            {
                { }
            }
        }
        private void AddBackupValue(DateTime time, double value)
        {
            if (BackupPath == null || BackupPath =="")
            { return; }
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
            if (BackupPath == null || BackupPath == "")
            { return; }
            File.WriteAllLines(BackupPath, BackupLines);
        }
        StringBuilder BackupStringBuilder = new StringBuilder();
        Queue<string> BackupLines = new Queue<string>();
        private void RestoreBackup(string backupPath)
        {
            if (File.Exists(backupPath))
            {
                string[] lines = File.ReadAllLines(BackupPath);
                foreach (string line in lines)
                {
                    string[] split = line.Split(';');
                    DateTime time = DateTime.Parse(split[0]);
                    double value = double.Parse(split[1]);
                    Average.AddValue(value);
                    BackupLines.Append(line);
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
            CurrentTimeSpotAverage.Clear();
            Average.Clear();
            Value = 0;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
        
    }
}
