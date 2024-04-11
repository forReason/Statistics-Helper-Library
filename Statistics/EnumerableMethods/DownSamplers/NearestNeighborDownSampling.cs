using QuickStatistics.Net.Median_NS;

namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
    /// <summary>
    /// Down-samples an array to a smaller array using a Nearest Neighbor approach.
    /// </summary>
    /// <param name="source">The array to down-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>A down-sampled array where each element is selected using the Nearest Neighbor method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static double[] DownSampleNearestNeighbor(IEnumerable<double> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<double> sourceArray = source as IList<double> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength <= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be smaller than the source length.");
        if (targetLength == 1)
            return new[] {sourceArray[(int)Math.Round(sourceLength/2.0,0)] };

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
}