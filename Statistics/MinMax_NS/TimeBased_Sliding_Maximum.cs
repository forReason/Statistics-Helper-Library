using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.MinMax_NS
{
    internal class TimeBased_Sliding_Maximum
    {
        public TimeBased_Sliding_Maximum(TimeSpan duration)
        {
            Clear();
            Duration = duration;
        }
        public double CurrentMaximum { get; set; }
        public TimeSpan Duration { get; set; }

        private List<Objects.TimeSpot_Value<double>> MaximumValues = new List<Objects.TimeSpot_Value<double>>();
        /// <summary>
        /// this function assumes the dateTime of the added point is right now
        /// good for realtime / sensor data
        /// </summary>
        /// <param name="input"></param>
        public void AddValue(double input)
        {
            Objects.TimeSpot_Value<double> value = new Objects.TimeSpot_Value<double> {Time = DateTime.Now, Value = input };
            AddValue(value);
        }
        /// <summary>
        /// this function evaluates the time of the added input (good for historic data)
        /// </summary>
        /// <param name="input"></param>
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
                CurrentMaximum = input.Value;
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
                CurrentMaximum = MaximumValues[0].Value;
            }
        }
        public void Clear()
        {
            this.CurrentMaximum = double.MinValue;
            this.MaximumValues.Clear();
        }
        public override string ToString()
        {
            return this.CurrentMaximum.ToString();
        }
    }
}
