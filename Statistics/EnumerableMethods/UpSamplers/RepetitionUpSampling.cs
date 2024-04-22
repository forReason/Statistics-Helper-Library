
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
        if (sourceLength == 0) return [];
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength > targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");
        
        double[] result = new double[targetLength];

        // Fill the result array with repeated values from the source array
        for (int sourceIndex = 0; sourceIndex < sourceLength; sourceIndex++)
        {
            double percentFillEnd = (sourceIndex + 1) / (double)sourceLength;
            double percentFillStart = sourceIndex / (double)sourceLength;
            double startIndex = (targetLength * percentFillStart);
            double endIndex = (targetLength * percentFillEnd);
            for (int targetIndex = (int)Math.Ceiling(startIndex); targetIndex <= Math.Min((int)endIndex,result.Length-1 ); targetIndex++)
            {
                result[targetIndex] = sourceArray[sourceIndex];
            }
        }
        
        return result;
    }
    /// <summary>
    /// Up-samples an array to a larger array using repetition.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using repetition.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static T[] UpSampleRepetition<T>(IEnumerable<T> source, int targetLength)
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
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");
        
        T[] result = new T[targetLength];

        // Fill the result array with repeated values from the source array
        for (int sourceIndex = 0; sourceIndex < sourceLength; sourceIndex++)
        {
            double percentFillEnd = (sourceIndex + 1) / (double)sourceLength;
            double percentFillStart = sourceIndex / (double)sourceLength;
            double startIndex = (targetLength * percentFillStart);
            double endIndex = (targetLength * percentFillEnd);
            for (int targetIndex = (int)Math.Ceiling(startIndex); targetIndex <= Math.Min((int)endIndex,result.Length-1 ); targetIndex++)
            {
                result[targetIndex] = sourceArray[sourceIndex];
            }
        }
        
        return result;
    }
    /// <summary>
    /// Up-samples an array to a larger array using repetition.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using repetition.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static decimal[] UpSampleRepetition(IEnumerable<decimal> source, int targetLength)
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
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than the source length.");
        
        decimal[] result = new decimal[targetLength];

        // Fill the result array with repeated values from the source array
        for (int sourceIndex = 0; sourceIndex < sourceLength; sourceIndex++)
        {
            double percentFillEnd = (sourceIndex + 1) / (double)sourceLength;
            double percentFillStart = sourceIndex / (double)sourceLength;
            double startIndex = (targetLength * percentFillStart);
            double endIndex = (targetLength * percentFillEnd);
            for (int targetIndex = (int)Math.Ceiling(startIndex); targetIndex <= Math.Min((int)endIndex,result.Length-1 ); targetIndex++)
            {
                result[targetIndex] = sourceArray[sourceIndex];
            }
        }
        
        return result;
    }
}