using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Objects
{
    public struct TimeSpot_Value<T>
    {
        public TimeSpot_Value(DateTime time, T value)
        {
            this.Time = time;
            this.Value = value;
        }
        public DateTime Time { get; set; }
        public T Value { get; set; }
    }
}
