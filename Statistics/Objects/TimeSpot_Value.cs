using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Objects
{
    internal struct TimeSpot_Value<T>
    {
        internal TimeSpot_Value(DateTime time, T value)
        {
            this.Time = time;
            this.Value = value;
        }
        DateTime Time { get; set; }
        T Value { get; set; }
    }
}
