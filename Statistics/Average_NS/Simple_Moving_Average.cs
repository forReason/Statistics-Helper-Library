using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Average_NS
{
    public class Simple_Moving_Average
    {
        public Simple_Moving_Average(int length)
        {
            TargetDataLength = length;
        }
        private Queue<double> AverageQueue = new Queue<double>();
        public double Value { get; private set; }
        public int TargetDataLength { get; private set; }
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        public void AddValue(double input)
        {
            double weightedInput = input / TargetDataLength;
            // prime queue
            while (AverageQueue.Count < TargetDataLength)
            {
                AverageQueue.Enqueue(weightedInput);
                Value += weightedInput;
            }

        }

    }
}
