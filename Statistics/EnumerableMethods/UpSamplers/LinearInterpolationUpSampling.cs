
namespace QuickStatistics.Net.EnumerableMethods.UpSamplers;

public static partial class UpSampler
{
    /// <summary>
    /// Up-samples an array to a larger array using linear interpolation.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using linear interpolation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static double[] UpSampleLinearInterpolation(IEnumerable<double> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<double> sourceArray = source as IList<double> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength >= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength),
                "Target length must be greater than the source length.");

        double[] result = new double[targetLength];
        // Calculate the step size between each sample in the target array
        double stepSize = (double)(sourceLength - 1) / (targetLength - 1);

        for (int targetIndex = 0; targetIndex < targetLength; targetIndex++)
        {
            double sourceIndex = targetIndex * stepSize;
            int lowerIndex = (int)Math.Floor(sourceIndex);
            int upperIndex = (int)Math.Ceiling(sourceIndex);

            if (lowerIndex == upperIndex)
            {
                result[targetIndex] = sourceArray[lowerIndex];
            }
            else
            {
                double lowerValue = sourceArray[lowerIndex];
                double upperValue = sourceArray[upperIndex];
                double weight = sourceIndex - lowerIndex;

                // Perform linear interpolation
                result[targetIndex] = lowerValue + (upperValue - lowerValue) * weight;
            }
        }

        return result;
    }
}