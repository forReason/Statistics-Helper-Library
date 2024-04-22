using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.ExtensionMethods;
using QuickStatistics.Net.Math_NS;

namespace QuickStatistics.Net.Median_NS;

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
    private ulong idCounter;

    /// <summary>
    /// Indicates whether any values have been added to the median calculator.
    /// </summary>
    public bool ContainsValues => window.Count > 0;

    /// <summary>
    /// Gets the current median value of the elements.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no values have been added yet.</exception>
    public double Value {
        get
        {
            if (_Value is null) _Value = GetMedian();
            return _Value.Value;
        }
    }

    public double? _Value = null;

    /// <summary>
    /// returns the Minimum value in the current sliding Window
    /// </summary>
    public double Minimum => _MaxHeap.Max.value; // counterintuitive but its correct

    /// <summary>
    /// returns the maximum value in the current sliding window
    /// </summary>
    public double Maximum => _MinHeap.Max.value; // counterintuitive but its correct

    /// <summary>
    /// gets the standard deviation for the current data point
    /// </summary>
    /// <remarks>
    /// The standard deviation is cached when accessed until a new datapoint is added.
    /// However, the calculation if not in cache is rather expensive
    /// </remarks>
    public double StdDeviation 
    {
        get
        {
            if (_StdDeviation is null)
                _StdDeviation = Variance_NS.StandardDeviation_Double.CalculateStandardDeviation(window.Select(c => c.value));
            return _StdDeviation.Value;
        }
    }
    private double? _StdDeviation = null;

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
        _MinHeap = new SortedSet<(double value, ulong id)>(
            Comparer<(double value, ulong id)>.Create((x, y) =>
                x.value == y.value ? x.id.CompareTo(y.id) : x.value.CompareTo(y.value)));
        _MaxHeap = new SortedSet<(double value, ulong id)>(
            Comparer<(double value, ulong id)>.Create((x, y) =>
                x.value == y.value ? x.id.CompareTo(y.id) : y.value.CompareTo(x.value)));
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
    public double GetPercentile(double percentile)
    {
        if (percentile < 0 || percentile > 1.0)
            throw new ArgumentOutOfRangeException($"{nameof(percentile)} must be in the range of [0.0 - 1.0]");
        if (percentile <= 0) return Minimum;
        if (percentile >= 1.0) return Maximum;

        double preciseIndex = (window.Count-1) * percentile;
        (double lowValue, double bigValue) bracket = GetBracket(preciseIndex);
        double weight = preciseIndex - (int)preciseIndex;
        return VolumetricAverage_Double.VolumeBasedAverage(bracket.lowValue, 1-weight, bracket.bigValue, weight);
    }

    /// <summary>
    /// calculates the value for a given sigma factor.
    /// </summary>
    /// <remarks>common Sigma factors are in the range of -3 to +3</remarks>
    /// <param name="sigmaFactor">the sigma factor to calculate the value for</param>
    /// <returns>the calculated value at point sigma </returns>
    public double GetSigmaValue(double sigmaFactor)
    {
        return Variance_NS.NormalDistribution.GetXForSigma(Value, StdDeviation,sigmaFactor);
    }

    /// <summary>
    /// returns the two bracketing values of a precise index
    /// </summary>
    /// <param name="preciceIndex">the index which to obtain the brackets for</param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"> can only look up smaller than max-index-epsilon</exception>
    internal (double lowValue, double bigValue) GetBracket(double preciceIndex)
    {
        int lowerBracketIndex = (int)preciceIndex;
        if (lowerBracketIndex == _MaxHeap.Count - 1)
            return (_MaxHeap.Min.value, _MinHeap.Min.value);
        double lowValue = 0, bigValue = 0;
        if (lowerBracketIndex < _MaxHeap.Count)
            // go backwards because of _MaxHeap sorting
        {
            int currentIndex = _MaxHeap.Count - 1;
            int upperBracketIndex = lowerBracketIndex + 1;
            foreach ((double value, ulong id) item in _MaxHeap)
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
            foreach ((double value, ulong id) item in _MinHeap)
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
    public SortedDictionary<double, int> GenerateDistribution(int steps = 10)
    {
        if (!ContainsValues) return [];
        steps = Math.Max(steps, 1);
        // Combine and sort all values from both heaps
        List<double> combinedValues = new List<double>(_MaxHeap.Select(item => item.value).Reverse());
        combinedValues.AddRange(_MinHeap.Select(item => item.value));

        double minimum = Minimum;
        double maximum = Maximum;
        double coverage = Difference.Get(minimum, maximum);
        double
            epsilon = Math.Max(1e-10 * coverage,
                Double.Epsilon); // Use a small epsilon relative to the coverage or the smallest positive Double
        double stepSize = (coverage + epsilon) / steps;
        List<double>[] distribution = new List<double>[steps];
        for (int i = 0; i < distribution.Length; i++)
        {
            distribution[i] = new List<double>();
        }

        foreach ((double value, ulong id) entry in window)
        {
            double distanceFromMin = Difference.Get(minimum, entry.value);
            int step = (int)(distanceFromMin / stepSize);
            distribution[step].Add(entry.value);
        }

        SortedDictionary<double, int> DistributionMap = new SortedDictionary<double, int>();
        foreach (List<double> collection in distribution)
        {
            DistributionMap[collection.GetMedian()] = collection.Count;
        }

        return DistributionMap;
    }
    /// <summary>
    /// generates a normal distribution or bell curve
    /// </summary>
    /// <param name="resolution">the number of Data points to generate</param>
    /// <returns></returns>
    public SortedDictionary<double, double> GenerateNormalDistribution(int resolution = 7)
    {
        SortedDictionary<double, double> curve = new SortedDictionary<double, double>();
        double start = Value - 3 * StdDeviation;
        double end = Value + 3 * StdDeviation;
        double stepSize = (end - start) / resolution;

        for (double x = start; x <= end; x += stepSize)
        {
            curve[x] = Variance_NS.NormalDistribution.ProbabilityDensityFunction(x, Value, StdDeviation);
        }

        return curve;
    }

    /// <summary>
    /// Clears all the data from the rolling window and resets the internal state for fresh reuse.
    /// </summary>
    public void Clear()
    {
        window.Clear();
        _MinHeap.Clear();
        _MaxHeap.Clear();
        idCounter = 0;
    }
}
