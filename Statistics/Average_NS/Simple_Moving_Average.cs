using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Average_NS
{
    public class Simple_Moving_Average_Double
    {
        public Simple_Moving_Average_Double(int length)
        {
            MaxDataLength = length;
        }
        private Queue<double> AverageQueue = new Queue<double>();
        public double Value { get; private set; }
        public int MaxDataLength { get; private set; }
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        public void AddValue(double input)
        {
            double weightedInput = input / MaxDataLength;
            // prime queue
            while (AverageQueue.Count < MaxDataLength-1)
            {
                AverageQueue.Enqueue(weightedInput);
                Value += weightedInput;
            }
            AverageQueue.Enqueue(weightedInput);
            double change = weightedInput;
            change -= AverageQueue.Dequeue();
            Value += change;
        }
        public void Clear()
        {
            AverageQueue.Clear();
            Value = default(double);
        }
    }
}
