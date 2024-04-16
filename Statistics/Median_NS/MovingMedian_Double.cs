namespace QuickStatistics.Net.Median_NS
{
    /// <summary>
    /// The MovingMedian_Double class implements a rolling or moving median calculation. It is designed to continuously provide the median value from a dynamically changing set of data. 
    /// This implementation is particularly suited for scenarios where data is received in a stream, and the median of the recent 'window' of values is required at any point in time.
    /// </summary>
    /// <remarks>
    /// This class is not thread safe!<br/>
    /// The class uses two heaps (minHeap and maxHeap) to efficiently calculate the median in a rolling window.<br/>
    /// The class also maintains a dictionary for quick lookups and deletions, making it suitable for large datasets with frequent updates.
    /// </remarks>
    public class MovingMedian_Double
    {
        /// <summary>
        /// contains the upper half of the dataset
        /// </summary>
        private readonly SortedSet<(double value, ulong id)> _MinHeap;
        /// <summary>
        /// contains the lower half of the dataset
        /// </summary>
        private readonly SortedSet<(double value, ulong id)> _MaxHeap;
        private readonly Queue<(double value, ulong id)> window;
        private readonly int windowSize;
        private readonly Dictionary<double, HashSet<ulong>> valueToIds;
        private ulong idCounter;
        /// <summary>
        /// Indicates whether any values have been added to the median calculator.
        /// </summary>
        public bool ContainsValues => window.Count > 0;
        /// <summary>
        /// Gets the current median value of the elements.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no values have been added yet.</exception>
        public double Value => GetMedian();
        /// <summary>
        /// Initializes a new instance of the MovingMedian_Double class with a specified window size.
        /// </summary>
        /// <param name="windowSize">The size of the rolling window for which the median is calculated.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the window size is less than or equal to zero.</exception>
        public MovingMedian_Double(int windowSize)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be greater than 0.");
            }

            this.windowSize = windowSize;
            window = new Queue<(double value, ulong id)>(windowSize);
            valueToIds = new Dictionary<double, HashSet<ulong>>();
            _MinHeap = new SortedSet<(double value, ulong id)>(
                Comparer<(double value, ulong id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
            _MaxHeap = new SortedSet<(double value, ulong id)>(
                Comparer<(double value, ulong id)>.Create((x, y) => x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
            idCounter = 0;
        }
        /// <summary>
        /// Adds a new value to the rolling window and updates the median calculation.
        /// </summary>
        /// <param name="value">The value to be added to the rolling window.</param>
        public void AddValue(double value)
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
        public double GetMedian()
        {
            if (_MaxHeap.Count == 0)
            {
                throw new InvalidOperationException("No values added yet.");
            }

            if (_MaxHeap.Count == _MinHeap.Count)
            {
                return (_MaxHeap.Min.value + _MinHeap.Min.value) / 2.0;
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

        private void InsertValue((double value, ulong id) entry)
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

        private void RemoveValue((double value, ulong id) entry)
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

        public ReadOnlySpan<double> GenerateDistribution(int resolution)
        {
            if (!ContainsValues) return []; 
            resolution = Math.Clamp(resolution, 1, window.Count);
            // Combine and sort all values from both heaps
            var combinedValues = new List<double>(_MinHeap.Select(item => item.value));
            combinedValues.AddRange(_MaxHeap.Select(item => item.value));
            combinedValues.Sort();

            if (combinedValues.Count <= resolution)
            {
                // If the total number of points is less than or equal to the desired resolution,
                // just return the combined and sorted values.
                return combinedValues.ToArray();
            }

            var output = new double[resolution];
            // The 'step' is now the fractional index step in the combined list for each output point.
            double step = (double)(combinedValues.Count - 1) / (resolution - 1);

            for (int i = 0; i < resolution; i++)
            {
                double idx = i * step;
                int lowerIdx = (int)Math.Floor(idx);
                int upperIdx = (int)Math.Ceiling(idx);

                if (upperIdx >= combinedValues.Count) upperIdx = combinedValues.Count - 1; // Boundary check

                // If the calculated index is exactly an integer, no interpolation is needed.
                if (lowerIdx == upperIdx || lowerIdx == idx)
                {
                    output[i] = combinedValues[lowerIdx];
                }
                else
                {
                    // Linear interpolation for indices between two data points
                    double fraction = idx - lowerIdx;
                    output[i] = combinedValues[lowerIdx] + fraction * (combinedValues[upperIdx] - combinedValues[lowerIdx]);
                }
            }

            return output;
            
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
