namespace QuickStatistics.Net.Median_NS
{
    /// <summary>
    /// The MovingMedian_Decimal class implements a rolling or moving median calculation. It is designed to continuously provide the median value from a dynamically changing set of data. 
    /// This implementation is particularly suited for scenarios where data is received in a stream, and the median of the recent 'window' of values is required at any point in time.
    /// </summary>
    /// <remarks>
    /// The class uses two heaps (minHeap and maxHeap) to efficiently calculate the median in a rolling window.
    /// The class also maintains a dictionary for quick lookups and deletions, making it suitable for large datasets with frequent updates.
    /// </remarks>
    public class MovingMedian_Decimal
    {
        private readonly SortedSet<(decimal value, ulong id)> _MinHeap;
        private readonly SortedSet<(decimal value, ulong id)> _MaxHeap;
        private readonly Queue<(decimal value, ulong id)> window;
        private readonly int windowSize;
        private readonly Dictionary<decimal, HashSet<ulong>> valueToIds;
        private ulong idCounter;
        /// <summary>
        /// Indicates whether any values have been added to the median calculator.
        /// </summary>
        public bool ContainsValues => window.Count > 0;
        /// <summary>
        /// Gets the current median value of the elements.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no values have been added yet.</exception>
        public decimal Value => GetMedian();
        /// <summary>
        /// Initializes a new instance of the MovingMedian_Decimal class with a specified window size.
        /// </summary>
        /// <param name="windowSize">The size of the rolling window for which the median is calculated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the window size is less than or equal to zero.</exception>
        public MovingMedian_Decimal(int windowSize)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be greater than 0.");
            }

            this.windowSize = windowSize;
            window = new Queue<(decimal value, ulong id)>(windowSize);
            valueToIds = new Dictionary<decimal, HashSet<ulong >>();
            _MinHeap = new SortedSet<(decimal value, ulong id)>(Comparer<(decimal value, ulong id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
            _MaxHeap = new SortedSet<(decimal value, ulong id)>(Comparer<(decimal value, ulong id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
            idCounter = 0;
        }
        /// <summary>
        /// Adds a new value to the rolling window and updates the median calculation.
        /// </summary>
        /// <param name="value">The value to be added to the rolling window.</param>
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
        /// <summary>
        /// Retrieves the current median value of the elements within the rolling window.
        /// </summary>
        /// <returns>The median value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no values have been added yet.</exception>
        public decimal GetMedian()
        {
            if (_MaxHeap.Count == 0)
            {
                throw new InvalidOperationException("No values added yet.");
            }

            if (_MaxHeap.Count == _MinHeap.Count)
            {
                return (_MaxHeap.Min.value + _MinHeap.Min.value) / 2.0m;
            }
            else if (_MinHeap.Count > _MaxHeap.Count)
            {
                return _MinHeap.Min.value;
            }
            else
            {
                return _MaxHeap.Min.value;
            }
        }

        private void InsertValue((decimal value, ulong id) entry)
        {
            if (!valueToIds.ContainsKey(entry.value))
            {
                valueToIds[entry.value] = new HashSet<ulong>();
            }
            valueToIds[entry.value].Add(entry.id);

            if (_MaxHeap.Count == 0 || entry.value <= _MaxHeap.Min.value)
            {
                _MaxHeap.Add(entry);
            }
            else
            {
                _MinHeap.Add(entry);
            }

            RebalanceHeaps();
        }


        private void RemoveValue((decimal value, ulong id) entry)
        {
            if (!valueToIds.ContainsKey(entry.value))
                return;

            valueToIds[entry.value].Remove(entry.id);
            if (valueToIds[entry.value].Count == 0)
                valueToIds.Remove(entry.value);

            if (_MaxHeap.Contains(entry))
                _MaxHeap.Remove(entry);
            else if (_MinHeap.Contains(entry))
                _MinHeap.Remove(entry);
        }

        private void RebalanceHeaps()
        {
            if (_MaxHeap.Count > _MinHeap.Count + 1)
            {
                _MinHeap.Add(_MaxHeap.Min);
                _MaxHeap.Remove(_MaxHeap.Min);
            }
            else if (_MinHeap.Count > _MaxHeap.Count + 1)
            {
                _MaxHeap.Add(_MinHeap.Min);
                _MinHeap.Remove(_MinHeap.Min);
            }
        }
        /// <summary>
        /// Clears all the data from the rolling window and resets the internal state for fresh reuse.
        /// </summary>
        public void Clear()
        {
            window.Clear();
            _MinHeap.Clear();
            _MaxHeap.Clear();
            valueToIds.Clear();
            idCounter = 0;
        }
    }
}
