using QuickStatistics.Net.Average_NS;

namespace QuickStatistics.Net.ExtensionMethods;

public static class ArrayExtensions_Double
{
    public static double[] DownSample(this IEnumerable<double> source, int targetLength, DownSamplingMethod method )
    {
        // convert
        return method switch
        {
            DownSamplingMethod.Average => EnumerableMethods.DownSamplers.DownSampler.DownSampleAverage(source, targetLength),
            DownSamplingMethod.Median => EnumerableMethods.DownSamplers.DownSampler.DownSampleMedian(source, targetLength),
            DownSamplingMethod.RandomReservoir => EnumerableMethods.DownSamplers.DownSampler.DownSampleMedian(source, targetLength),
            DownSamplingMethod.NearestNeighbor => EnumerableMethods.DownSamplers.DownSampler.DownSampleNearestNeighbor(source, targetLength),
            DownSamplingMethod.MaxPooling => throw new NotImplementedException("MaxPooling is not yet implemented"),
            DownSamplingMethod.MinPooling => throw new NotImplementedException("MinPooling is not yet implemented"),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, "method is not implemented")
        };
    }
}