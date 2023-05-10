namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// a very simple and fast method to get the moving average on sliding data.
    /// This function is particularly useful for large datasets or infinite data inflow
    /// </summary>
    public class SimpleMovingAverage_Decimal
    {
        public SimpleMovingAverage_Decimal(int length)
        {
            MaxDataLength = length;
            Clear();
        }
        private Queue<decimal> AverageQueue = new Queue<decimal>();
        public decimal Value { get; private set; }
        public int MaxDataLength { get; private set; }
        public int CurrentDataLength { get { return AverageQueue.Count; } }
        public void AddValue(decimal input)
        {
            decimal weightedInput = input / MaxDataLength;
            AverageQueue.Enqueue(weightedInput);
            if (AverageQueue.Count <= MaxDataLength)
            {// queue is not full yet
                Value += (input - Value) / AverageQueue.Count;
            }
            else
            { // queue is full. now in sliding window
                decimal change = weightedInput;
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
