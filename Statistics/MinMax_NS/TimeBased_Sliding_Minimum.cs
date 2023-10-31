
namespace QuickStatistics.Net.MinMax_NS
{
    /// <summary>
    /// Represents a time-based sliding minimum value tracker.
    /// <br/>
    /// Allows tracking of minimum values over a specific time duration.
    /// </summary>
    public class TimeBased_Sliding_Minimum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBased_Sliding_Minimum"/> class.
        /// </summary>
        /// <param name="duration">The duration to track.</param>
        /// <param name="subStepDuration">specifies how many values should be pooled together first in order to improve storage and performance<br/>
        /// note that this may increase duration by a maximum of subStepDuration</param>
        public TimeBased_Sliding_Minimum(TimeSpan duration, TimeSpan subStepDuration)
        {
            Clear();
            Duration = duration;
            SubStepDuration = subStepDuration;
            CurrentTimeSpot = new Objects.TimeSpot_Value<double>(DateTime.MinValue, double.MaxValue);
        }
        /// <summary>
        /// Gets the current minimum value.
        /// <br/>
        /// Resets to <see cref="double.MaxValue"/> when cleared.
        /// </summary>
        public double CurrentMinimum { get { return Math.Min(_CurrentMinimum, CurrentTimeSpot.Value); } }
        /// <summary>
        /// Gets or sets the current minimum value.
        /// <br/>
        /// Resets to <see cref="double.MaxValue"/> when cleared.
        /// </summary>
        private double _CurrentMinimum { get; set; }

        /// <summary>
        /// Gets or sets the tracking duration.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// specifies how many values should be pooled together in order to improve storage and performance<br/>
        /// note that this may increase duration by a maximum of subStepDuration
        /// </summary>
        public TimeSpan SubStepDuration { get; set; }

        private Objects.TimeSpot_Value<double> CurrentTimeSpot = new Objects.TimeSpot_Value<double>();

        private List<Objects.TimeSpot_Value<double>> MinimumValues = new List<Objects.TimeSpot_Value<double>>();

        /// <summary>
        /// Adds a new value to the tracking list.
        /// <br/>
        /// Assumes the current time if no time is provided. Good for real-time data like sensor readings.
        /// </summary>
        /// <param name="input">The value to add.</param>
        public void AddValue(double input, DateTime? time)
        {
            if (time == null) time = DateTime.Now;
            if (time > CurrentTimeSpot.Time + SubStepDuration)
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
                CurrentTimeSpot.Value = Math.Min(CurrentTimeSpot.Value, input);
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
            // minimum logic
            if (input.Value <= CurrentMinimum)
            {
                // new minimum found, clear tracking list
                // this is the best case scenario
                MinimumValues.Clear();
                _CurrentMinimum = input.Value;
            }
            else
            {
                // unfortunately, no new maximum was found
                // go backwards the maximum tracking list and check for smaller values
                // clear the list of all smaller values. The list should therefore always
                // be in descending order
                for (int b = MinimumValues.Count - 1; b >= 0; b--)
                {
                    if (MinimumValues[b].Value >= input.Value)
                    {
                        // a higher value has been found. We have a newer, lower value
                        // clear this waste value from the tracking list
                        MinimumValues.RemoveAt(b);
                    }
                    else
                    {
                        // there are no more higher values. 
                        // stop looking for larger values to save time
                        break;
                    }
                }
            }
            // append new value to tracking list, no matter if higher or lower
            // all future values might be lower
            MinimumValues.Add(input);
            // check if the oldest value is too old to be kept in the tracking list
            while (MinimumValues[0].Time < targetDate)
            {
                // oldest value is to be removed
                MinimumValues.RemoveAt(0);
                // update maximum
                _CurrentMinimum = MinimumValues[0].Value;
            }
        }
        /// <summary>
        /// Clears the tracking list and resets the current minimum value to <see cref="double.MaxValue"/>.
        /// </summary>
        public void Clear()
        {
            this._CurrentMinimum = double.MaxValue;
            CurrentTimeSpot = new Objects.TimeSpot_Value<double>(DateTime.MinValue, double.MaxValue);
            this.MinimumValues.Clear();
        }

        /// <summary>
        /// Returns the string representation of the current minimum value.
        /// </summary>
        /// <returns>String representation of <see cref="CurrentMinimum"/>.</returns>
        public override string ToString()
        {
            return this.CurrentMinimum.ToString();
        }
    }
}
