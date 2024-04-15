
namespace QuickStatistics.Net.EnumerableMethods.UpSamplers;

public static partial class UpSampler
{
    /// <summary>
    /// Up-samples an array to a larger array using repetition.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using repetition.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static double[] UpSampleRepetition(IEnumerable<double> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<double> sourceArray = source as IList<double> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength > targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");

        double[] result = new double[targetLength];
        int repetitionFactor = targetLength / sourceLength;
        int remainder = targetLength % sourceLength;

        // Fill the result array with repeated values from the source array
        int resultIndex = 0;
        for (int i = 0; i < sourceLength; i++)
        {
            for (int j = 0; j < repetitionFactor; j++)
            {
                result[resultIndex++] = sourceArray[i];
            }
        }

        // Fill the remaining elements with the last value of the source array
        for (int i = 0; i < remainder; i++)
        {
            result[resultIndex++] = sourceArray[sourceLength - 1];
        }

        return result;
    }
}