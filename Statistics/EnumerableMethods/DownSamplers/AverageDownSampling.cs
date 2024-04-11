using QuickStatistics.Net.Average_NS;

namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
    /// <summary>
    /// down-samples an array to a smaller array using an averaging approach.
    /// </summary>
    /// <remarks>
    /// In an ideal case, the smaller array is smaller by a factor of a full number. Eg [100] to [25] (factor 4)<br/>
    /// The smaller the source array, the larger the aliasing uncertainty gets. Eg [4] to [3] (factor 1.33333333...)
    /// </remarks>
    /// <param name="source">the array to down-sample</param>
    /// <param name="targetLength">the desired target length</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">the source array must be longer than the target array and targetLength must be > 1</exception>
    public static double[] DownSampleAverage(IEnumerable<double> source, int targetLength)
    {
        // precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be greater than 1!");
        int sourceLength = source.Count();
        if (sourceLength < targetLength)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} cannot be longer than {nameof(source)}!");
        if (sourceLength == targetLength)
        {
            return source.ToArray();
        }

        // preparations for conversions
        double[] result = new double[targetLength];
        double factor = sourceLength / (double)targetLength;
        SimpleMovingAverage_Double slidingAverageWindow = new((int)Math.Ceiling(factor));
        // downsample
        int i = 0;
        int targetFill = 0;
        foreach (double input in source)
        {
            slidingAverageWindow.AddValue(input);
            i++;
            if ((int)(i / factor) > targetFill)
            {
                result[targetFill] = slidingAverageWindow.Value;
                targetFill++;
            }
        }

        // finalize
        return result;
    }
}