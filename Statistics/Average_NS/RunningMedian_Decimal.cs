

namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// running median can be used to quickly gather the median of all values added (so far)
    /// note, you might run into memory constraints, so this method is not suitable for infinite data flow
    /// </summary>
    public class RunningMedian_Decimal
    {
        private readonly SortedSet<(decimal value, int id)> minHeap;
        private readonly SortedSet<(decimal value, int id)> maxHeap;
        private int idCounter;

        public RunningMedian_Decimal()
        {
            minHeap = new SortedSet<(decimal value, int id)>(Comparer<(decimal value, int id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
            maxHeap = new SortedSet<(decimal value, int id)>(Comparer<(decimal value, int id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
            idCounter = 0;
        }

        public void AddValue(decimal value)
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

            RebalanceHeaps();
        }

        public decimal GetMedian()
        {
            if (maxHeap.Count == 0)
            {
                throw new InvalidOperationException("No values added yet.");
            }

            if (maxHeap.Count == minHeap.Count)
            {
                return (maxHeap.Min.value + minHeap.Min.value) / 2.0m;
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
    }
}