namespace QuickStatistics.Net.Objects
{
    public struct TimeSpot_Value<T>
    {
        public TimeSpot_Value(DateTime time, T value)
        {
            this.Time = time;
            this.Value = value;
        }
        public DateTime Time { get; set; }
        public T Value { get; set; }
    }
}
