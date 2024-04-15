using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.Variance_NS;

namespace QuickStatistics.Net.ExtensionMethods;

public static class ArrayExtensions_Double
{
    /// <summary>
    /// down-samples an array according to the requested down sampling Method
    /// </summary>
    /// <param name="source">the source array to down sample</param>
    /// <param name="targetLength">the requested target length</param>
    /// <param name="method">the sampling method to use</param>
    /// <returns>a new, shortened array</returns>
    /// <exception cref="ArgumentOutOfRangeException">the target length or the input length do not match</exception>
    public static double[] DownSample(this IEnumerable<double> source, int targetLength, DownSamplingMethod method )
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
    public static double[] UpSample(this IEnumerable<double> source, int targetLength, UpSamplingMethod method )
    {
        // TODO: Implement upsampling Methods
        return method switch
        {
            UpSamplingMethod.LinearInterpolation => EnumerableMethods.UpSamplers.UpSampler.UpSampleLinearInterpolation(source, targetLength),
            UpSamplingMethod.SplineInterpolation => throw new NotImplementedException(),
            UpSamplingMethod.NearestNeighbor => throw new NotImplementedException(),
            UpSamplingMethod.Repetition => throw new NotImplementedException(),
            UpSamplingMethod.Resample => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, "method is not implemented")
        };
    }

    public static double GetMedian(this IEnumerable<double> source, bool inputIsSorted = false)
    {
        return Median_NS.Median_Double.GetMedian(source, inputIsSorted);
    }
    public static double GetAverage(this IEnumerable<double> source)
    {
        return ProgressingAverage_Double.CalculateAverage(source);
    }
    public static double GetStandardDeviation(this IEnumerable<double> source)
    {
        return StandardDeviation.CalculateStandardDeviation(source);
    }
}