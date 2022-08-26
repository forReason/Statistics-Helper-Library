using Statistics.Objects;

namespace Statistics.Average_NS
{
    public class Moving_Average_Double
    {
        public Moving_Average_Double(TimeSpan totalTime, TimeSpan valueResolution)
        {
            this.TotalTime = totalTime;
            this.ValueResolution = valueResolution;
            Clear();
        }
        // "Settings"
        /// <summary>
        ///  TotalTime specifies window of time in the past which should keepp track of (eg last 2hours)
        /// </summary>
        public TimeSpan TotalTime { get; set; }
        /// <summary>
        /// ValueResolution specifies how much time be consolidated into one dataPoint (mainly Memory saving Feature)
        /// </summary>
        public TimeSpan ValueResolution { get; set; }
        // Working variables
        private Queue<TimeSpot_Value<double>> ValueQueue { get; set; }
        private DateTime CurrentTimeSpot { get; set; }
        private Progressing_Average_Double CurrentTimeSpotAverage = new Progressing_Average_Double();
        private double QueueValue { get; set; }
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
        public void AddValuePoint(double value)
        {
            AddValuePoint(value, DateTime.Now);
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
        public void AddValuePoint(double value, DateTime timeStamp)
        {
            // check if new value needs to be added to queue
            if (CurrentTimeSpot + ValueResolution < timeStamp)
            {
                double spotValue = CurrentTimeSpotAverage.Value / ValueQueue.Count;
                ValueQueue.Enqueue(new TimeSpot_Value<double>(CurrentTimeSpot, spotValue));
                QueueValue += spotValue;
                CurrentTimeSpot = timeStamp;
                CurrentTimeSpotAverage.Clear();
            }
            // check if oldest timeSpot needs to be dequeued
            TimeSpot_Value<double> checkRemovethis;
            if (ValueQueue.TryPeek(out checkRemovethis))
            {
                if (checkRemovethis.Time + TotalTime < timeStamp)
                {
                    ValueQueue.Dequeue();
                    QueueValue -= checkRemovethis.Value;
                }
            }
            // Update current TimeSpot
            CurrentTimeSpotAverage.AddValue(value);
            // update public value
            this.Value = Volumetric_Average.VolumeBasedAverage(value1: QueueValue, volume1: TotalTime.TotalMinutes, value2: CurrentTimeSpotAverage.Value, volume2: ValueResolution.TotalMinutes);
        }
        /// <summary>
        /// resets the average to 0 and clears all its internal values.
        /// </summary>
        /// <remarks>
        /// setting (time) persist
        /// </remarks>
        public void Clear()
        {
            ValueQueue = new Queue<TimeSpot_Value<double>>();
            CurrentTimeSpot = DateTime.MinValue;
            CurrentTimeSpotAverage.Clear();
            QueueValue = 0;
            Value = 0;
        }
    }
}
