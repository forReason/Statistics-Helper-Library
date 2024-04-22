using System.Numerics;
using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.Variance_NS;

namespace QuickStatistics.Net.ExtensionMethods;

#if NET7_0_OR_GREATER
public static class ArrayExtensions_T<T> where T : INumber<T>
{
    /// <summary>
    /// down-samples an array according to the requested down sampling Method
    /// </summary>
    /// <param name="source">the source array to down sample</param>
    /// <param name="targetLength">the requested target length</param>
    /// <param name="method">the sampling method to use</param>
    /// <returns>a new, shortened array</returns>
    /// <exception cref="ArgumentOutOfRangeException">the target length or the input length do not match</exception>
    public static T[] DownSample<T>(IEnumerable<T> source, int targetLength, DownSamplingMethod method ) where T : INumber<T>
    {
        return method switch
        {
            DownSamplingMethod.Average => EnumerableMethods.DownSamplers.DownSampler.DownSampleAverage(source, targetLength),
            DownSamplingMethod.Median => EnumerableMethods.DownSamplers.DownSampler.DownSampleMedian(source, targetLength),
            DownSamplingMethod.RandomReservoir => EnumerableMethods.DownSamplers.DownSampler.DownSampleMedian(source, targetLength),
            DownSamplingMethod.NearestNeighbor => EnumerableMethods.DownSamplers.DownSampler.DownSampleNearestNeighbor(source, targetLength),
            DownSamplingMethod.MaxPooling => EnumerableMethods.DownSamplers.DownSampler.DownSampleMaxPooling(source,targetLength),
            DownSamplingMethod.MinPooling => EnumerableMethods.DownSamplers.DownSampler.DownSampleMinPooling(source,targetLength),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, "method is not implemented")
        };
    }
    
    /// <summary>
    /// increases the resolution of an array by various interpolation methods
    /// </summary>
    /// <param name="source">the source array which the resolution should be increased of</param>
    /// <param name="targetLength">the target total resolution (must be larger than original resolution)</param>
    /// <param name="method">the method of interpolating</param>
    /// <returns>an array with higher, interpolated resolution</returns>
    /// <exception cref="ArgumentOutOfRangeException">input arguments were invalid</exception>
    public static T[] UpSample<T>(IEnumerable<T> source, int targetLength, UpSamplingMethod method ) where T : INumber<T>
    {
        return method switch
        {
            UpSamplingMethod.LinearInterpolation => EnumerableMethods.UpSamplers.UpSampler.UpSampleLinearInterpolation(source, targetLength),
            UpSamplingMethod.SplineInterpolation => EnumerableMethods.UpSamplers.UpSampler.UpSampleSplineInterpolation(source, targetLength),
            UpSamplingMethod.NearestNeighbor => EnumerableMethods.UpSamplers.UpSampler.UpSampleNearestNeighbor(source, targetLength),
            UpSamplingMethod.Repetition => EnumerableMethods.UpSamplers.UpSampler.UpSampleRepetition(source, targetLength),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, "method is not implemented")
        };
    }
    
    /// <summary>
    /// returns the median value of an array as double
    /// </summary>
    /// <remarks>because the calculator iterates over the entire array, it might be more performant to use the standalone functions</remarks>
    /// <param name="source">the input array</param>
    /// <param name="inputIsSorted">saves processing power</param>
    /// <returns>median value of the input array</returns>
    public static double GetMedian<T>(IEnumerable<T> source, bool inputIsSorted = false) where T : INumber<T>
    {
        return Median_NS.Median.GetMedian(source, inputIsSorted);
    }
     
    /// <summary>
    /// returns the average value of an array
    /// </summary>
    /// <remarks>because the calculator iterates over the entire array, it might be more performant to use the standalone functions</remarks>
    /// <param name="source">the input array</param>>
    /// <returns>average value of the input array</returns>
    public static double GetAverage<T>(IEnumerable<T> source) where T : INumber<T>
    {
        return ProgressingAverage<T>.CalculateAverage(source);
    }
    
    /// <summary>
    /// returns the standard deviation between the data points within the array
    /// </summary>
    /// <remarks>because the calculator iterates over the entire array, it might be more performant to use the standalone functions</remarks>
    /// <param name="source">the input array to calculate the std deviation for</param>
    /// <returns>the standard deviation of the array</returns>
    public static double GetStandardDeviation<T>(IEnumerable<T> source) where T : INumber<T>
    {
        return StandardDeviation<T>.CalculateStandardDeviation(source);
    }
}
#endif