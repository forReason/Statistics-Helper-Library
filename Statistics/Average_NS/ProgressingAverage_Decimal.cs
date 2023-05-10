
namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// progressing average is a simple, fast and precise method to get the average value
    /// </summary>
    /// <remarks>
    /// it can only work on a finite number of inputs, so not suitable for indefinite input<br/>
    /// for that purpose, please refer to <see cref="SimpleMovingAverage_Decimal"/>
    /// </remarks>
    public class ProgressingAverage_Decimal
    {
        public ProgressingAverage_Decimal()
        {
            Clear();
        }
        public decimal Value { get; private set; }
        private decimal _Count { get; set; }
        public void AddValue(decimal input)
        {
            Value += (input - Value) / _Count;
        }
        public void AddValue(decimal[] input)
        {
            foreach (var item in input) { AddValue(item); }
        }
        public void AddValue(List<decimal> input)
        {
            foreach (var item in input) { AddValue(item); }
        }
        public void Clear()
        {
            _Count = 0;
            Value = 0;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
