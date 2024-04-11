using QuickStatistics.Net.Median_NS;

namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
   private static readonly Random random = new Random();

    /// <summary>
    /// Down-samples an array to a smaller array using the Random Reservoir Sampling method with exponential skips.
    /// </summary>
    /// <remarks>
    /// This method selects a random subset where each subset has the same probability of being chosen, optimized for large datasets.<br/>
    /// The method is particularly useful when dealing with streaming data or large datasets where not all data can be kept in memory.
    /// </remarks>
    /// <param name="source">The enumerable source of data to down-sample.</param>
    /// <param name="targetLength">The desired target length of the down-sampled data.</param>
    /// <returns>A down-sampled array containing a random subset of the original data.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is less than 1 or greater than the source length.</exception>
    public static double[] ReservoirSample(IEnumerable<double> source, int targetLength)
    {
        // Precondition checks
        var sourceArray = source.ToArray();
        int sourceLength = sourceArray.Length;
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        if (sourceLength < targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length cannot be longer than the source array!");

        // Initialize the reservoir with the first part of the source
        double[] reservoir = new double[targetLength];
        for (int i = 0; i < targetLength; i++)
        {
            reservoir[i] = sourceArray[i];
        }

        // Start the sampling process
        double w = Math.Exp(Math.Log(random.NextDouble()) / targetLength);
        int i = targetLength;

        while (i < sourceLength)
        {
            i += (int)Math.Floor(Math.Log(random.NextDouble()) / Math.Log(1 - w)) + 1;
            if (i < sourceLength)
            {
                int replaceIndex = random.Next(targetLength); // randomInteger(1, k) - 1 for zero-based index
                reservoir[replaceIndex] = sourceArray[i];
                w *= Math.Exp(Math.Log(random.NextDouble()) / targetLength);
            }
        }

        return reservoir;
    }

    private static Random random = new Random();

    /// <summary>
    /// Generates a random number between 0 (inclusive) and 1 (exclusive).
    /// </summary>
    /// <returns>A double-precision floating point number in the range [0, 1).</returns>
    private static double RandomDouble()
    {
        return random.NextDouble();
    }
}