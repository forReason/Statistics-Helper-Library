using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.MinMax_NS
{
    public class Sliding_Minimum
    {
        /// <summary>
        /// Automatically calculates a maximum for datapoints in the range of the given length
        /// </summary>
        /// <param name="length"></param>
        public Sliding_Minimum(int dataPointLength)
        {
            Values = new Circular_MinMax_Array<double>(dataPointLength);
            Clear();
        }
        private Circular_MinMax_Array<double> Values { get; set; }
        /// <summary>
        /// Represents the minimum in the selected Time Window
        /// </summary>
        public double Value { get; private set; }
        public void AddPoint(double input)
        {
            // if new value has been found, all other values can be discarded, hence this function
            if (input >= Value)
            {
                Value = input;
                Values.NewMinMaxFound(input);
            }
            else
            {
                double? lastOut = Values.AppendPoint(input);
                if (lastOut != null)
                {
                    if (lastOut == Value)
                    { // lastOut was minimum! find new min index
                        int minIndex = 0;
                        Value = double.MaxValue;
                        for (int i = 0; i < Values.Length; i++)
                        {
                            double value = Values.GetValueAt(i);
                            if (value < Value)
                            {
                                minIndex = i;
                                Value = value;
                            }
                        }
                        Values.CutTail(minIndex);
                    }
                }
            }
        }
        public void Clear()
        {
            Values.Clear();
            Value = double.MaxValue;
        }
    }
}
