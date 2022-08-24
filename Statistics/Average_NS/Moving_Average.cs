using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistics.Objects;

namespace Statistics.Average_NS
{
    public class Moving_Average
    {
        public Moving_Average(TimeSpan totalTime, TimeSpan valueResolution)
        {
            this.TotalTime = totalTime;
            this.ValueResolution = valueResolution;
        }
        // "Settings"
        /// <summary>
        ///  
        /// </summary>
        TimeSpan TotalTime { get; set; }
        TimeSpan ValueResolution { get; set; }
        // Working variables
        Queue<TimeSpot_Value<double>> queue = new Queue<TimeSpot_Value<double>>();
        private TimeSpot_Value<double> spotValue;
        private int ResolutionValuePoints = 0;
        public double AddValuePoint(double value)
        {
            return AddValuePoint(value, DateTime.Now);
        }
        public double AddValuePoint(double value, DateTime timeStamp)
        {
            // create new spot valu if its null
            //if (this.spotValue.Equals(default(SpotValue<double>))) spotValue = new SpotValue<double>(timeStamp,0);
            throw new NotImplementedException();
            return 0;
        }
    }
}
