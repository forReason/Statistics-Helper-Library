namespace QuickStatistics.Net.Median_NS
{
    /// <summary>
    /// moving median (or: Rolling Median) calculates the median on a rolling Time window and thus is suitable for indefinite data inflow
    /// </summary>
    public class MovingMedian_Double
    {
        private readonly SortedSet<(double value, int id)> minHeap;
        private readonly SortedSet<(double value, int id)> maxHeap;
        private readonly Queue<double> window;
        private readonly int windowSize;
        private int idCounter;

        public MovingMedian_Double(int windowSize)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be greater than 0.");
            }

            this.windowSize = windowSize;
            window = new Queue<double>(windowSize);
            minHeap = new SortedSet<(double value, int id)>(Comparer<(double value, int id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
            maxHeap = new SortedSet<(double value, int id)>(Comparer<(double value, int id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
            idCounter = 0;
        }

        public void AddValue(double value)
        {
            if (window.Count == windowSize)
            {
                RemoveValue(window.Dequeue());
            }
            window.Enqueue(value);
            InsertValue(value);
            RebalanceHeaps();
        }

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

        private void InsertValue(double value)
        {
            var entry = (value, idCounter++);
            if (maxHeap.Count == 0 || value <= maxHeap.Min.value)
            {
                maxHeap.Add(entry);
            }
            else
            {
                minHeap.Add(entry);
            }
        }

        private void RemoveValue(double value)
        {
            var entryToRemove = maxHeap.Contains((value, -1)) ? maxHeap.GetViewBetween((value, int.MinValue), (value, int.MaxValue)).Min : minHeap.GetViewBetween((value, int.MinValue), (value, int.MaxValue)).Min;

            if (maxHeap.Contains(entryToRemove))
            {
                maxHeap.Remove(entryToRemove);
            }
            else
            {
                minHeap.Remove(entryToRemove);
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
    }
}
