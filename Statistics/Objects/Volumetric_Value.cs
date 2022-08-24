using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Objects
{
    internal class Volumetric_Value<T>
    {
        internal Volumetric_Value(double volume, T value)
        {
            this.Volume = volume;
            this.Value = value;
        }
        double Volume { get; set; }
        T Value { get; set; }
    }
}
