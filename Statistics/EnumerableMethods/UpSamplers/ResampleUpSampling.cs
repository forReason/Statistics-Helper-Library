
namespace QuickStatistics.Net.EnumerableMethods.UpSamplers;

public static partial class UpSampler
{
    /// <summary>
        /// Up-samples an array to a larger array using resampling (inserting zeros and applying a low-pass filter).
        /// </summary>
        /// <param name="source">The array to up-sample.</param>
        /// <param name="targetLength">The desired target length.</param>
        /// <returns>An up-sampled array using resampling.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
        public static double[] UpSampleResample(IEnumerable<double> source, int targetLength)
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

            // Calculate the zero insertion factor
            int zeroInsertionFactor = targetLength / sourceLength;
            int remainder = targetLength % sourceLength;

            double[] result = new double[targetLength];

            // Insert zeros
            int resultIndex = 0;
            for (int i = 0; i < sourceLength; i++)
            {
                result[resultIndex++] = sourceArray[i];
                for (int j = 1; j < zeroInsertionFactor; j++)
                {
                    result[resultIndex++] = 0; // Insert zero
                }
            }

            // Apply low-pass filter
            result = ApplyLowPassFilter(result, (int)Math.Ceiling(targetLength / (double)sourceLength));

            return result;
        }

    private static double[] ApplyLowPassFilter(double[] signal, int windowSize)
    {

        double[] filteredSignal = new double[signal.Length];

        // Apply moving average filter
        for (int i = 0; i < signal.Length; i++)
        {
            int startIndex = Math.Max(0, i - windowSize + 1);
            int endIndex = Math.Min(signal.Length - 1, i + windowSize - 1);

            double sum = 0;
            for (int j = startIndex; j <= endIndex; j++)
            {
                sum += signal[j];
            }

            filteredSignal[i] = sum / (endIndex - startIndex + 1);
        }

        return filteredSignal;
    }
}