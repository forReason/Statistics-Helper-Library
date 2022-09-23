using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.MinMax_NS
{
    public class Sliding_Maximum
    {
        /// <summary>
        /// Automatically calculates a maximum for datapoints in the range of the given length
        /// </summary>
        /// <param name="length"></param>
        public Sliding_Maximum(int length)
        {
            Values = new Circular_MinMax_Array<double>(length);
            Clear();
        }
        private Circular_MinMax_Array<double> Values{get; set;}
        /// <summary>
        /// Represents the maximum in the selected Time Window
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
                    { // lastOut was maximum! find new max index
                        int maxIndex = 0;
                        Value = double.MinValue;
                        for (int i = 0; i < Values.Length; i++)
                        {
                            double value = Values.GetValueAt(i);
                            if (value > Value)
                            {
                                maxIndex = i;
                                Value = value;
                            }
                        }
                        Values.CutTail(maxIndex);
                    }
                }
            }
        }
        public void Clear()
        {
            Values.Clear();
            Value = double.MinValue;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
