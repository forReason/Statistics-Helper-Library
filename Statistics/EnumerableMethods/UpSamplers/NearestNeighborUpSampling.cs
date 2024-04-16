namespace QuickStatistics.Net.EnumerableMethods.UpSamplers;

public static partial class UpSampler
{
    /// <summary>
    /// Up-samples an array to a larger array using a Nearest Neighbor approach.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array where each element is selected using the Nearest Neighbor method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static double[] UpSampleNearestNeighbor(IEnumerable<double> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<double> sourceArray = source as IList<double> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength >= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");

        double[] result = new double[targetLength];
        // Adjusting the factor calculation to effectively "center" each selection within its segment
        double factor = (double)(sourceLength - 1) / (targetLength - 1);

        for (int targetIndex = 0; targetIndex < targetLength; targetIndex++)
        {
            int nearestSourceIndex = (int)Math.Round(targetIndex * factor);
            result[targetIndex] = sourceArray[nearestSourceIndex];
        }

        return result;
    }
    /// <summary>
    /// Up-samples an array to a larger array using a Nearest Neighbor approach.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array where each element is selected using the Nearest Neighbor method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static T[] UpSampleNearestNeighbor<T>(IEnumerable<T> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<T> sourceArray = source as IList<T> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength >= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");

        T[] result = new T[targetLength];
        // Adjusting the factor calculation to effectively "center" each selection within its segment
        double factor = (double)(sourceLength - 1) / (targetLength - 1);

        for (int targetIndex = 0; targetIndex < targetLength; targetIndex++)
        {
            int nearestSourceIndex = (int)Math.Round(targetIndex * factor);
            result[targetIndex] = sourceArray[nearestSourceIndex];
        }

        return result;
    }
    /// <summary>
    /// Up-samples an array to a larger array using a Nearest Neighbor approach.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array where each element is selected using the Nearest Neighbor method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static decimal[] UpSampleNearestNeighbor(IEnumerable<decimal> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<decimal> sourceArray = source as IList<decimal> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength >= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");

        decimal[] result = new decimal[targetLength];
        // Adjusting the factor calculation to effectively "center" each selection within its segment
        double factor = (double)(sourceLength - 1) / (targetLength - 1);

        for (int targetIndex = 0; targetIndex < targetLength; targetIndex++)
        {
            int nearestSourceIndex = (int)Math.Round(targetIndex * factor);
            result[targetIndex] = sourceArray[nearestSourceIndex];
        }

        return result;
    }
}