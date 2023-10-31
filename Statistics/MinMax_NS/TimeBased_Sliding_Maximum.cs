namespace QuickStatistics.Net.MinMax_NS
{
    /// <summary>
    /// Represents a time-based sliding minimum value tracker.
    /// <br/>
    /// Allows tracking of maximum values over a specific time duration.
    /// </summary>
    public class TimeBased_Sliding_Maximum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBased_Sliding_Maximum"/> class.
        /// </summary>
        /// <param name="duration">The duration to track.</param>
        /// <param name="subStepDuration">specifies how many values should be pooled together first in order to improve storage and performance<br/>
        /// note that this may increase duration by a maximum of subStepDuration</param>
        public TimeBased_Sliding_Maximum(TimeSpan duration, TimeSpan subStepDuration)
        {
            Clear();
            Duration = duration;
            SubStepDuration = subStepDuration;
            CurrentTimeSpot = new Objects.TimeSpot_Value<double>(DateTime.MinValue, double.MinValue);
        }
        /// <summary>
        /// Gets the current maximum value.
        /// <br/>
        /// Resets to <see cref="double.MinValue"/> when cleared.
        /// </summary>
        public double CurrentMaximum { get { return Math.Max(_CurrentMaximum, CurrentTimeSpot.Value); } }
        /// <summary>
        /// Gets or sets the current maximum value.
        /// <br/>
        /// Resets to <see cref="double.MaxValue"/> when cleared.
        /// </summary>
        private double _CurrentMaximum { get; set; }
        /// <summary>
        /// Gets or sets the tracking duration.
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Gets or sets the sub-step duration for value updates.
        /// </summary>
        public TimeSpan SubStepDuration { get; set; }
        private Objects.TimeSpot_Value<double> CurrentTimeSpot = new Objects.TimeSpot_Value<double>();

        private List<Objects.TimeSpot_Value<double>> MaximumValues = new List<Objects.TimeSpot_Value<double>>();
        /// <summary>
        /// Adds a new value to the tracking list.
        /// <br/>
        /// Assumes the current time if no time is provided. Good for real-time data like sensor readings.
        /// </summary>
        /// <param name="input">The value to add.</param>
        public void AddValue(double input, DateTime? time)
        {
            if (time == null) time = DateTime.Now;
            if (DateTime.Now > CurrentTimeSpot.Time + SubStepDuration)
            {
                if (CurrentTimeSpot.Time != DateTime.MinValue)
                {
                    Objects.TimeSpot_Value<double> value = CurrentTimeSpot;
                    AddValue(value);
                }
                CurrentTimeSpot = new Objects.TimeSpot_Value<double>(time.Value, input);
            }
            else
            {
                CurrentTimeSpot.Value = Math.Max(CurrentTimeSpot.Value, input);
            }
        }
        /// <summary>
        /// Adds a new value to the tracking list.
        /// <br/>
        /// Evaluates the time of the added input. Good for historical data.
        /// </summary>
        /// <param name="input">The value to add.</param>
        public void AddValue(Objects.TimeSpot_Value<double> input)
        {
            // calculate the oldest date to keep in tracking list
            DateTime targetDate = input.Time - Duration;
            // maximum logic
            if (input.Value >= CurrentMaximum)
            {
                // new maximum found, clear tracking list
                // this is the best case scenario
                MaximumValues.Clear();
                _CurrentMaximum = input.Value;
            }
            else
            {
                // unfortunately, no new maximum was found
                // go backwards the maximum tracking list and check for smaller values
                // clear the list of all smaller values. The list should therefore always
                // be in descending order
                for (int b = MaximumValues.Count - 1; b >= 0; b--)
                {
                    if (MaximumValues[b].Value <= input.Value)
                    {
                        // a lower value has been found. We have a newer, higher value
                        // clear this waste value from the tracking list
                        MaximumValues.RemoveAt(b);
                    }
                    else
                    {
                        // there are no more lower values. 
                        // stop looking for smaller values to save time
                        break;
                    }
                }
            }
            // append new value to tracking list, no matter if higher or lower
            // all future values might be lower
            MaximumValues.Add(input);
            // check if the oldest value is too old to be kept in the tracking list
            while (MaximumValues[0].Time < targetDate)
            {
                // oldest value is to be removed
                MaximumValues.RemoveAt(0);
                // update maximum
                _CurrentMaximum = MaximumValues[0].Value;
            }
        }
        /// <summary>
        /// Clears the tracking list and resets the current maximum value to <see cref="double.MinValue"/>.
        /// </summary>
        public void Clear()
        {
            this._CurrentMaximum = double.MinValue;
            CurrentTimeSpot = new Objects.TimeSpot_Value<double>(DateTime.MinValue, double.MinValue);
            this.MaximumValues.Clear();
        }
        /// <summary>
        /// Returns the string representation of the current maximum value.
        /// </summary>
        /// <returns>String representation of <see cref="CurrentMaximum"/>.</returns>
        public override string ToString()
        {
            return this.CurrentMaximum.ToString();
        }
    }
}
