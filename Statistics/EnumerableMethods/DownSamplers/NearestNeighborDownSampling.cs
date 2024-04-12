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
        int sourceLength = source.Count();
        if (sourceLength == targetLength)
            return source.ToArray();
        if (sourceLength <= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be smaller than the source length.");
        // TODO: if (targetLength == 1) return new[] {source[(int)Math.Round(sourceLength/2.0,0)] };

        double[] result = new double[targetLength];
        // Adjusting the factor calculation to effectively "center" each selection within its segment
        double factor = (double)(sourceLength - 1) / (targetLength - 1);

        int index = 0;
        int currentFillIndex = 0;
        double lastItem = default;
        foreach(double item in source)
        {
            if ((int)(index / factor) >= currentFillIndex)
            {
                result[currentFillIndex] = slidingAverageWindow.Value;
                targetFill++;
            }
            index++;
        }

        return result;
    }
}