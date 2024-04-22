using QuickStatistics.Net.MinMax_NS;

namespace QuickStatistics.Net.Median_NS
{
    /// <summary>
    /// running median can be used to quickly gather the median of all values added (so far)
    /// note, you might run into memory constraints, so this method is not suitable for infinite data flow
    /// </summary>
    public class ProgressingMedian_Double
    {
        private readonly SortedSet<(double value, ulong id)> minHeap;
        private readonly SortedSet<(double value, ulong id)> maxHeap;
        /// <summary>
        /// retrieves the current Median Value.
        /// </summary>
        /// <remarks>Values need to be added before accessing</remarks>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when no Values are added yet</exception>
        public double Value => GetMedian();
        /// <summary>
        ///  the amount of Values added so far
        /// </summary>
        public ulong ValueCount { get; private set; }
        /// <summary>
        /// true when values have been added already
        /// </summary>
        public bool ContainsValues => ValueCount > 0;

        public ProgressingMedian_Double()
        {
            minHeap = new SortedSet<(double value, ulong id)>(Comparer<(double value, ulong id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
            maxHeap = new SortedSet<(double value, ulong id)>(Comparer<(double value, ulong id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
            ValueCount = 0;
        }

        /// <summary>
        /// Adds a value to the Median calculation
        /// </summary>
        /// <remarks></remarks>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException">when the maximum number of values is rdded alreaty</exception>
        public void AddValue(double value)
        {
            if (ValueCount == ulong.MaxValue){
                throw new InvalidOperationException("Max Data count reached! please use MovingMedian instead!");
            }
            var entry = (value, ValueCount++);

            if (maxHeap.Count == 0 || value <= maxHeap.Min.value)
            {
                maxHeap.Add(entry);
            }
            else
            {
                minHeap.Add(entry);
            }

            RebalanceHeaps();
        }

        /// <summary>
        /// retrieves the current Median Value.
        /// </summary>
        /// <remarks>Values need to be added before accessing</remarks>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when no Values are added yet</exception>
        public double GetMedian()
        {
            if (maxHeap.Count == 0)
            {
                throw new InvalidOperationException("No values added yet.");
            }

            if (maxHeap.Count == minHeap.Count)
            {
                return (maxHeap.Min.value + minHeap.Min.value) / 2.0;
            }
            else
            {
                return maxHeap.Min.value;
            }
        }

        private void RebalanceHeaps()
        {
            if (maxHeap.Count > minHeap.Count + 1)
            {
                minHeap.Add(maxHeap.Min);
                maxHeap.Remove(maxHeap.Min);
            }
            else if (minHeap.Count > maxHeap.Count + 1)
            {
                maxHeap.Add(minHeap.Min);
                minHeap.Remove(minHeap.Min);
            }
        }
        /// <summary>
        /// empties the median making it ready for reuse
        /// </summary>
        public void Clear()
        {
            ValueCount = 0;
            minHeap.Clear();
            maxHeap.Clear();
        }
    }
}