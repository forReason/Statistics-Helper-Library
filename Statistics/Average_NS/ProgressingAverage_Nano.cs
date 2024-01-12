namespace QuickStatistics.Net.Average_NS
{
    /// <summary>
    /// This class aims to provide the most minimal averaging solution which might me used for tremendous Datasets. <br/>
    /// This is relevant when you need to track many Averages in parallel and want to avoid class keeping overhead
    /// </summary>
    public static class ProgressingAverage_Nano
    {

        /// <summary>
        /// Adds a new value to the average calculation.
        /// </summary>
        /// <remarks>
        /// start with AddValue(inputValue,0,inputValue).<br/>
        /// the method modifies currentValue and elementCount in place, make sure you handle thread safety
        /// </remarks>
        /// <param name="currentValue">The current average to add to.</param>
        /// <param name="elementCount">The current element count.</param>
        /// <param name="inputValue">The value to add.</param>
        /// <returns>True if the value has been added, false if max count has been reached.</returns>
        public static bool AddValue(ref decimal currentValue, ref ulong elementCount, decimal inputValue)
        {
            if (elementCount == decimal.MaxValue)
            {
                return false;
            }
            elementCount++;
            
            currentValue += (inputValue - currentValue) / elementCount;
            return true;
        }
        /// <summary>
        /// Adds a new value to the average calculation.
        /// </summary>
        ///<remarks>
        /// start with AddValue(inputValue,0,inputValue).<br/>
        /// the method modifies currentValue and elementCount in place, make sure you handle thread safety
        /// </remarks>
        /// <param name="currentValue">The current average to add to.</param>
        /// <param name="elementCount">The current element count.</param>
        /// <param name="inputValue">The value to add.</param>
        /// <returns>True if the value has been added, false if max count has been reached.</returns>
        public static bool AddValue(ref double currentValue, ref ulong elementCount, double inputValue)
        {
            if (elementCount == ulong.MaxValue)
            {
                return false;
            }
            elementCount++;
            
            currentValue += (inputValue - currentValue) / elementCount;
            return true;
        }
        /// <summary>
        /// Adds a new value to the average calculation.
        /// </summary>
        /// <remarks>
        /// start with AddValue(inputValue,0,inputValue).<br/>
        /// the method modifies currentValue and elementCount in place, make sure you handle thread safety
        /// </remarks>
        /// <param name="currentValue">The current average to add to.</param>
        /// <param name="elementCount">The current element count.</param>
        /// <param name="inputValue">The value to add.</param>
        /// <returns>True if the value has been added, false if the max element count has been reached</returns>
        public static bool AddValue(ref double currentValue, ref uint elementCount, double inputValue)
        {
            if (elementCount == uint.MaxValue)
            {
                return false;
            }
            elementCount++;

            currentValue += (inputValue - currentValue) / elementCount;
            return true;
        }
        /// <summary>
        /// Adds a new value to the average calculation.<br/>
        /// this function is optimized for storage space and does it's internal calculation with double.
        /// </summary>
        /// <remarks>
        /// start with AddValue(inputValue,0,inputValue).<br/>
        /// the method modifies currentValue and elementCount in place, make sure you handle thread safety
        /// </remarks>
        /// <param name="currentValue">The current average to add to.</param>
        /// <param name="elementCount">The current element count.</param>
        /// <param name="inputValue">The value to add.</param>
        /// <returns>True if the value has been added, false if the max element count has been reached</returns>
        public static bool AddValue(ref float currentValue, ref uint elementCount, float inputValue)
        {
            if (elementCount == uint.MaxValue)
            {
                return false;
            }
            elementCount++;
            
            currentValue += (float)(((double)inputValue - (double)currentValue) / elementCount);
            return true;
        }
        /// <summary>
        /// Adds a new value to the average calculation.<br/>
        /// this function is optimized for storage space and does it's internal calculation with double.
        /// </summary>
        /// <remarks>
        /// start with AddValue(inputValue,0,inputValue).<br/>
        /// the method modifies currentValue and elementCount in place, make sure you handle thread safety
        /// </remarks>
        /// <param name="currentValue">The current average to add to.</param>
        /// <param name="elementCount">The current element count.</param>
        /// <param name="inputValue">The value to add.</param>
        /// <returns>True if the value has been added, false if the max element count has been reached</returns>
        public static bool AddValue(ref float currentValue, ref ushort elementCount, float inputValue)
        {
            if (elementCount == ushort.MaxValue)
            {
                return false;
            }
            elementCount++;

            currentValue += (float)(((double)inputValue - (double)currentValue) / elementCount);
            return true;
        }
    }
}
