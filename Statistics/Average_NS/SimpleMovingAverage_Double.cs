namespace QuickStatistics.Net.Average_NS
{
    public class SimpleMovingAverage_Double
    {
        public SimpleMovingAverage_Double(int length)
        {
            MaxDataLength = length;
            Clear();
        }
        private Queue<double> AverageQueue = new Queue<double>();
        public double Value { get; private set; }
        public int MaxDataLength { get; private set; }
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        public void AddValue(double input)
        {
            double weightedInput = input / MaxDataLength;
            AverageQueue.Enqueue(weightedInput);
            if (AverageQueue.Count <= MaxDataLength)
            {// queue is not full yet
                Value += (input - Value) / AverageQueue.Count;
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
}
