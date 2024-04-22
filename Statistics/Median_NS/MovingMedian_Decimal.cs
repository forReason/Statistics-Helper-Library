using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.ExtensionMethods;
using QuickStatistics.Net.Math_NS;

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
        /// returns the Minimum value in the current sliding Window
        /// </summary>
        public decimal Minimum => _MaxHeap.Max.value; // counterintuitive but its correct

        /// <summary>
        /// returns the maximum value in the current sliding window
        /// </summary>
        public decimal Maximum => _MinHeap.Max.value; // counterintuitive but its correct
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
    /// gets the x percentile value in the current time window with a worst case cost of O(log(n/2))
    /// </summary>
    /// <remarks>This needs to iterate over the internal sorted dictionary.
    /// It`s hence not recommended to pull this metric for every single data entry in a tight loop</remarks>
    /// <param name="percentile"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public decimal GetPercentile(decimal percentile)
    {
        if (percentile < 0 || percentile > 1.0m)
            throw new ArgumentOutOfRangeException($"{nameof(percentile)} must be in the range of [0.0 - 1.0]");
        if (percentile <= 0) return Minimum;
        if (percentile >= 1.0m) return Maximum;

        decimal preciseIndex = (window.Count-1) * percentile;
        (decimal lowValue, decimal bigValue) bracket = GetBracket(preciseIndex);
        decimal weight = preciseIndex - (int)preciseIndex;
        return VolumetricAverage_Decimal.VolumeBasedAverage(bracket.lowValue, 1-weight, bracket.bigValue, weight);
    }

    /// <summary>
    /// returns the two bracketing values of a precise index
    /// </summary>
    /// <param name="preciceIndex">the index which to obtain the brackets for</param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"> can only look up smaller than max-index-epsilon</exception>
    internal (decimal lowValue, decimal bigValue) GetBracket(decimal preciceIndex)
    {
        int lowerBracketIndex = (int)preciceIndex;
        if (lowerBracketIndex == _MaxHeap.Count - 1)
            return (_MaxHeap.Min.value, _MinHeap.Min.value);
        decimal lowValue = 0, bigValue = 0;
        if (lowerBracketIndex < _MaxHeap.Count)
            // go backwards because of _MaxHeap sorting
        {
            int currentIndex = _MaxHeap.Count - 1;
            int upperBracketIndex = lowerBracketIndex + 1;
            foreach ((decimal value, ulong id) item in _MaxHeap)
            {
                if (currentIndex == upperBracketIndex)
                    bigValue = item.value;
                else if (currentIndex == lowerBracketIndex)
                {
                    lowValue = item.value;
                    break;
                }

                currentIndex--;
            }
        }
        else
        {
            int currentIndex = 0;
            lowerBracketIndex -= _MaxHeap.Count;
            int upperBracketIndex = lowerBracketIndex + 1;
            foreach ((decimal value, ulong id) item in _MinHeap)
            {
                if (currentIndex == lowerBracketIndex)
                    lowValue = item.value;
                else if (currentIndex == upperBracketIndex)
                {
                    bigValue = item.value;
                    break;
                }

                currentIndex++;
            }
        }

        return (lowValue, bigValue);
    }
    /// <summary>
    /// generates a distribution map for the values in the current Heap
    /// </summary>
    /// <param name="steps">the resolution</param>
    /// <returns></returns>
    public SortedDictionary<decimal, int> GenerateDistribution(int steps = 10)
    {
        if (!ContainsValues) return [];
        steps = Math.Max(steps, 1);
        // Combine and sort all values from both heaps
        List<decimal> combinedValues = new List<decimal>(_MaxHeap.Select(item => item.value).Reverse());
        combinedValues.AddRange(_MinHeap.Select(item => item.value));

        decimal minimum = Minimum;
        decimal maximum = Maximum;
        decimal coverage = Difference.Get(minimum, maximum);
        decimal epsilon = Math.Max(1e-28M * coverage, 1e-28M);
        decimal stepSize = (coverage + epsilon) / steps;
        List<decimal>[] distribution = new List<decimal>[steps];
        for (int i = 0; i < distribution.Length; i++)
        {
            distribution[i] = new List<decimal>();
        }

        foreach ((decimal value, ulong id) entry in window)
        {
            decimal distanceFromMin = Difference.Get(minimum, entry.value);
            int step = (int)(distanceFromMin / stepSize);
            distribution[step].Add(entry.value);
        }

        SortedDictionary<decimal, int> DistributionMap = new SortedDictionary<decimal, int>();
        foreach (List<decimal> collection in distribution)
        {
            DistributionMap[collection.GetMedian()] = collection.Count;
        }

        return DistributionMap;
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
