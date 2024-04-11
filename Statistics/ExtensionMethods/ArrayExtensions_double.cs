using QuickStatistics.Net.Average_NS;

namespace QuickStatistics.Net.ExtensionMethods;

public static class ArrayExtensions_Double
{
    public static double[] DownSample(this IEnumerable<double> source, int targetLength, DownSamplingMethod method )
    {
        // convert
        switch (method)
        {
            case DownSamplingMethod.Average:
                return EnumerableMethods.DownSamplers.DownSampler.DownSampleAverage(source, targetLength);
            case DownSamplingMethod.Median:
                return EnumerableMethods.DownSamplers.DownSampler.DownSampleMedian(source, targetLength);
            case DownSamplingMethod.RandomReservoir:
                break;
            case DownSamplingMethod.NearestNeighbor:
                break;
            case DownSamplingMethod.MaxPooling:
                break;
            case DownSamplingMethod.MinPooling:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }

        return new double[]{};
    }
}