using System.Numerics;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// progressing average is a simple, fast and precise method to get the average value<br/>
    /// has an internal conversion to double. Please use the alternative <see cref="ProgressingAverage_Decimal"/> for large numbers
    /// </summary>
    /// <remarks>
    /// it can only work on a finite number of inputs, so not suitable for indefinite input<br/>
    /// for that purpose, please refer to <see cref="SimpleMovingAverage"/>
    /// </remarks>
    public class ProgressingAverage<T> where T : INumber<T>
    {
        public ProgressingAverage()
        {
            Clear();
        }
        public double Value { get; private set; }
        public int Count { get; private set; }
        public void AddValue(T input)
        {
            Count++;
            if (Count == int.MaxValue)
            {
                throw new IndexOutOfRangeException("max amount has been reached! use preciseaverage or moving avg instead!");
            }
            Value += (Convert.ToDouble(input) - Value) / Count;
        }
        public void AddValue(T[] input)
        {
            foreach (var item in input) { AddValue(item); }
        }
        public void AddValue(List<T> input)
        {
            foreach (var item in input) { AddValue(item); }
        }
        public void Clear()
        {
            Count = 0;
            Value = 0;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
#endif
}
