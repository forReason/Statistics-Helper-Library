using System;
using System.Collections.Generic;

namespace Statistics_unit_tests.Median_NS
{
    internal class MovingMedianOverflowTestClass
    {
        private readonly SortedSet<(decimal value, short id)> minHeap;
        private readonly SortedSet<(decimal value, short id)> maxHeap;
        private readonly Queue<(decimal value, short id)> window;
        private readonly int windowSize;
        private readonly Dictionary<decimal, HashSet<short>> valueToIds;
        private short idCounter;
        public bool ContainsValues => window.Count > 0;

        public MovingMedianOverflowTestClass(int windowSize)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be greater than 0.");
            }

            this.windowSize = windowSize;
            window = new Queue<(decimal value, short id)>(windowSize);
            valueToIds = new Dictionary<decimal, HashSet<short>>();
            minHeap = new SortedSet<(decimal value, short id)>(Comparer<(decimal value, short id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
            maxHeap = new SortedSet<(decimal value, short id)>(Comparer<(decimal value, short id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
            idCounter = 0;
        }

        public void AddValue(decimal value)
        {
            if (window.Count == windowSize)
            {
                RemoveValue(window.Dequeue());
            }

            var entry = (value, idCounter++);
            window.Enqueue(entry);
            InsertValue(entry);
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

        private void InsertValue((decimal value, short id) entry)
        {
            if (!valueToIds.ContainsKey(entry.value))
            {
                valueToIds[entry.value] = new HashSet<short>();
            }
            valueToIds[entry.value].Add(entry.id);

            if (maxHeap.Count == 0 || entry.value <= maxHeap.Min.value)
            {
                maxHeap.Add(entry);
            }
            else
            {
                minHeap.Add(entry);
            }

            RebalanceHeaps();
        }


        private void RemoveValue((decimal value, short id) entry)
        {
            if (!valueToIds.ContainsKey(entry.value))
                return;

            valueToIds[entry.value].Remove(entry.id);
            if (valueToIds[entry.value].Count == 0)
                valueToIds.Remove(entry.value);

            if (maxHeap.Contains(entry))
                maxHeap.Remove(entry);
            else if (minHeap.Contains(entry))
                minHeap.Remove(entry);
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
        public void Clear()
        {
            window.Clear();
            minHeap.Clear();
            maxHeap.Clear();
            valueToIds.Clear();
            idCounter = 0;
        }
    }
}
