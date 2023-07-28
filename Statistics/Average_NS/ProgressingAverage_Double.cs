
namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// This class provides a simple, fast, and precise method to compute the average of a finite number of inputs. <br/><br/>
    /// thread safe
    /// </summary>
    /// <remarks>
    /// This class is not suitable for an indefinite number of inputs.
    /// For an indefinite number of inputs, please refer to <see cref="SimpleMovingAverage_Double"/>.
    /// </remarks>
    public class ProgressingAverage_Double
    {
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the ProgressingAverage_Double class.
        /// </summary>
        public ProgressingAverage_Double()
        {
            Clear();
        }

        /// <summary>
        /// Gets the current average value.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Gets the current count of values that have been added.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Adds a new value to the average calculation.
        /// </summary>
        /// <param name="input">The value to add.</param>
        /// <exception cref="OverflowException">Thrown when the maximum count of values is reached.</exception>
        public void AddValue(double input)
        {
            lock (_lockObject)
            {
                Count++;
                if (Count == int.MaxValue)
                {
                    throw new OverflowException("Max amount has been reached! Use precise average or moving avg instead!");
                }
                Value += (input - Value) / Count;
            }
        }

        /// <summary>
        /// Adds an array of new values to the average calculation.
        /// </summary>
        /// <param name="input">The array of values to add.</param>
        public void AddValue(double[] input)
        {
            foreach (var item in input)
            {
                AddValue(item);
            }
        }

        /// <summary>
        /// Adds a list of new values to the average calculation.
        /// </summary>
        /// <param name="input">The list of values to add.</param>
        public void AddValue(List<double> input)
        {
            foreach (var item in input)
            {
                AddValue(item);
            }
        }

        /// <summary>
        /// Clears the current average calculation.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                Count = 0;
                Value = 0;
            }
        }

        /// <summary>
        /// Returns a string that represents the current average value.
        /// </summary>
        /// <returns>A string that represents the current average value.</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
