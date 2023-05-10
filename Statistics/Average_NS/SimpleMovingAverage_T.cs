using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// a very simple and fast method to get the moving average on sliding data.
    /// This function is particularly useful for large datasets or infinite data inflow
    /// </summary>
    /// <remarks>has an internal conversion to double. For large data types, please use <see cref="SimpleMovingAverage_Decimal"/></remarks>
    public class SimpleMovingAverage<T> where T : INumber<T>
    {
        public SimpleMovingAverage(int length)
        {
            MaxDataLength = length;
            Clear();
        }
        private Queue<double> AverageQueue = new Queue<double>();
        public double Value { get; private set; }
        public int MaxDataLength { get; private set; }
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        public void AddValue(T input)
        {
            double weightedInput = Convert.ToDouble(input) / MaxDataLength;
            AverageQueue.Enqueue(weightedInput);
            if (AverageQueue.Count <= MaxDataLength)
            {// queue is not full yet
                Value += (Convert.ToDouble(input) - Value) / AverageQueue.Count;
            }
            else
            { // queue is full. now in sliding window
                double change = weightedInput;
                change -= AverageQueue.Dequeue();
                Value += change;
            }
        }
        public void Clear()
        {
            AverageQueue.Clear();
            Value = 0;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
#endif
}
