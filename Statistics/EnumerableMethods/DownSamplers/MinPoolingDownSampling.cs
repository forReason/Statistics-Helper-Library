using System.Numerics;
using QuickStatistics.Net.MinMax_NS;

namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
    /// <summary>
    /// down-samples an array to a smaller array using a min pooling approach.<br/>
    /// Example: {1,3,2,4,6,7} should be down-sampled into [2], this means, the minimum of each block is used: {1,4}
    /// </summary>
    /// <remarks>
    /// In an ideal case, the smaller array is smaller by a factor of a full number. Eg [100] to [25] (factor 4)<br/>
    /// The smaller the source array, the larger the aliasing uncertainty gets. Eg [4] to [3] (factor 1.33333333...)
    /// </remarks>
    /// <param name="source">the array to down-sample</param>
    /// <param name="targetLength">the desired target length</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">the source array must be longer than the target array and targetLength must be > 1</exception>
    public static double[] DownSampleMinPooling(IEnumerable<double> source, int targetLength)
    {
        // precondition checks
        if (targetLength < 0)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be >= 0!");
        int sourceLength = source.Count();
        if (sourceLength == 0 || targetLength == 0) return [];
        if (sourceLength == targetLength)
        {
            return source.ToArray();
        }
        if (sourceLength < targetLength)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be < {nameof(source)}!");

        // preparations for conversions
        double[] result = new double[targetLength];
        double factor = sourceLength / (double)targetLength;
        Sliding_Minimum maximum = new Sliding_Minimum((int)Math.Ceiling(factor));
        // down-sample
        int i = 0;
        int targetFill = 0;
        foreach (double input in source)
        {
            maximum.AddPoint(input);
            i++;
            if ((int)(i / factor) > targetFill)
            {
                result[targetFill] = maximum.Value;
                targetFill++;
            }
        }

        // finalize
        return result;
    }
#if NET7_0_OR_GREATER
    /// <summary>
    /// down-samples an array to a smaller array using a generic min pooling approach.<br/>
    /// Example: {1,3,2,4,6,7} should be down-sampled into [2], this means, the minimum of each block is used: {1,4}
    /// </summary>
    /// <remarks>
    /// Since this is a generic method, it utilizes internal double conversion, which might lead to conversion errors t -> double -> t <br/>
    /// In an ideal case, the smaller array is smaller by a factor of a full number. Eg [100] to [25] (factor 4)<br/>
    /// The smaller the source array, the larger the aliasing uncertainty gets. Eg [4] to [3] (factor 1.33333333...)
    /// </remarks>
    /// <param name="source">the array to down-sample</param>
    /// <param name="targetLength">the desired target length</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">the source array must be longer than the target array and targetLength must be > 1</exception>
    public static T[] DownSampleMinPooling<T>(IEnumerable<T> source, int targetLength)where T : INumber<T>
    {
        // precondition checks
        if (targetLength < 0)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be >= 0!");
        int sourceLength = source.Count();
        if (sourceLength == 0 || targetLength == 0) return [];
        if (sourceLength == targetLength)
        {
            return source.ToArray();
        }
        if (sourceLength < targetLength)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be < {nameof(source)}!");

        // preparations for conversions
        T[] result = new T[targetLength];
        double factor = sourceLength / (double)targetLength;
        Sliding_Minimum maximum = new Sliding_Minimum((int)Math.Ceiling(factor));
        // down-sample
        int i = 0;
        int targetFill = 0;
        foreach (T input in source)
        {
            double inputValue = Convert.ToDouble(input);
            maximum.AddPoint(inputValue);
            i++;
            if ((int)(i / factor) > targetFill)
            {
                result[targetFill] = T.CreateTruncating(maximum.Value);
                targetFill++;
            }
        }

        // finalize
        return result;
    }
#endif
    /// <summary>
    /// down-samples an array to a smaller array using a min pooling approach.<br/>
    /// Example: {1,3,2,4,6,7} should be down-sampled into [2], this means, the minimum of each block is used: {1,4}
    /// </summary>
    /// <remarks>
    /// In an ideal case, the smaller array is smaller by a factor of a full number. Eg [100] to [25] (factor 4)<br/>
    /// The smaller the source array, the larger the aliasing uncertainty gets. Eg [4] to [3] (factor 1.33333333...)
    /// </remarks>
    /// <param name="source">the array to down-sample</param>
    /// <param name="targetLength">the desired target length</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">the source array must be longer than the target array and targetLength must be > 1</exception>
    public static decimal[] DownSampleMinPooling(IEnumerable<decimal> source, int targetLength)
    {
        // precondition checks
        if (targetLength < 0)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be >= 0!");
        int sourceLength = source.Count();
        if (sourceLength == 0 || targetLength == 0) return [];
        if (sourceLength == targetLength)
        {
            return source.ToArray();
        }
        if (sourceLength < targetLength)
            throw new ArgumentOutOfRangeException($"{nameof(targetLength)} must be < {nameof(source)}!");

        // preparations for conversions
        decimal[] result = new decimal[targetLength];
        double factor = sourceLength / (double)targetLength;
        Sliding_Minimum_Decimal maximum = new Sliding_Minimum_Decimal((int)Math.Ceiling(factor));
        // down-sample
        int i = 0;
        int targetFill = 0;
        foreach (decimal input in source)
        {
            maximum.AddPoint(input);
            i++;
            if ((int)(i / factor) > targetFill)
            {
                result[targetFill] = maximum.Value;
                targetFill++;
            }
        }

        // finalize
        return result;
    }
}