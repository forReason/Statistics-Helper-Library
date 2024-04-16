
using System.Numerics;

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
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength > targetLength)
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
#if NET7_0_OR_GREATER
    /// <summary>
    /// Up-samples an array to a larger array using generic linear interpolation.
    /// </summary>
    /// <remarks>Since this is a generic method, it utilizes internal double conversion, which might lead to conversion errors t -> double -> t</remarks>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using linear interpolation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static T[] UpSampleLinearInterpolation<T>(IEnumerable<T> source, int targetLength) where T : INumber<T>
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<T> sourceArray = source as IList<T> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength > targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength),
                "Target length must be greater than the source length.");

        T[] result = new T[targetLength];
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
                double lowerValue = Convert.ToDouble(sourceArray[lowerIndex]);
                double upperValue = Convert.ToDouble(sourceArray[upperIndex]);
                double weight = sourceIndex - lowerIndex;

                // Perform linear interpolation
                result[targetIndex] = T.CreateTruncating(lowerValue + (upperValue - lowerValue) * weight);
            }
        }

        return result;
    }
    #endif
    /// <summary>
    /// Up-samples an array to a larger array using linear interpolation.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using linear interpolation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static decimal[] UpSampleLinearInterpolation(IEnumerable<decimal> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<decimal> sourceArray = source as IList<decimal> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength > targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength),
                "Target length must be greater than the source length.");

        decimal[] result = new decimal[targetLength];
        // Calculate the step size between each sample in the target array
        decimal stepSize = (decimal)(sourceLength - 1) / (targetLength - 1);

        for (int targetIndex = 0; targetIndex < targetLength; targetIndex++)
        {
            decimal sourceIndex = targetIndex * stepSize;
            int lowerIndex = (int)Math.Floor(sourceIndex);
            int upperIndex = (int)Math.Ceiling(sourceIndex);

            if (lowerIndex == upperIndex)
            {
                result[targetIndex] = sourceArray[lowerIndex];
            }
            else
            {
                decimal lowerValue = sourceArray[lowerIndex];
                decimal upperValue = sourceArray[upperIndex];
                decimal weight = sourceIndex - lowerIndex;

                // Perform linear interpolation
                result[targetIndex] = lowerValue + (upperValue - lowerValue) * weight;
            }
        }

        return result;
    }
}