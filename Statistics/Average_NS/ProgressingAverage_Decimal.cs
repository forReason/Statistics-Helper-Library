﻿namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// Provides a simple, fast, and precise method to compute the average of a finite number of inputs. <br/><br/>
    /// thread safe
    /// </summary>
    /// <remarks>
    /// This class is not suitable for an indefinite number of inputs. For that purpose, please refer to <see cref="SimpleMovingAverage_Decimal"/>.<br/>
    /// The theoretical maximum amount of Data Points is decimal.MaxValue. <br/>
    /// However, the precision becomes less the closer you reach count = decimal.MaxValue
    /// </remarks>
    public class ProgressingAverage_Decimal
    {
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the ProgressingAverage_Decimal class.
        /// </summary>
        public ProgressingAverage_Decimal()
        {
            Clear();
        }

        /// <summary>
        /// Gets the current average value.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Gets the current count of values that have been added.
        /// </summary>
        public decimal Count { get; private set; }

        /// <summary>
        /// Adds a new value to the average calculation.
        /// </summary>
        /// <param name="input">The value to add.</param>
        /// <exception cref="OverflowException">Thrown when the maximum count of values is reached.</exception>
        public void AddValue(decimal input)
        {
            lock (_lockObject)
            {
                Count++;
                if (Count == decimal.MaxValue)
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
        public void AddValue(decimal[] input)
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
        public void AddValue(List<decimal> input)
        {
            foreach (var item in input)
            {
                AddValue(item);
            }
        }

        /// <summary>
        /// Calculates the average over a set of double values
        /// </summary>
        /// <param name="input">the values to calculate average for</param>
        /// <returns>double average</returns>
        public static decimal CalculateAverage(IEnumerable<decimal> input)
        {
            int Count = 0;
            decimal Value = 0;
            foreach (decimal item in input)
            {
                Count++;
                Value += (item - Value) / Count;
            }

            return Value;
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
