namespace QuickStatistics.Net.Objects
{
    /// <summary>
    /// Represents a time-stamped value.
    /// <br/>
    /// Provides two constructors for time-stamped values - one with an explicit time and one that uses the current time.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public struct TimeSpot_Value<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpot_Value{T}"/> struct with specified time and value.
        /// </summary>
        /// <param name="time">The timestamp of the value.</param>
        /// <param name="value">The value.</param>
        public TimeSpot_Value(DateTime time, T value)
        {
            this.Time = time;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpot_Value{T}"/> struct with the current time and specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public TimeSpot_Value(T value)
        {
            this.Time = DateTime.Now;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the timestamp for the value.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value { get; set; }
    }

}
