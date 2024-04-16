namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
    /// <summary>
    /// Performs reservoir sampling on an input array to produce a downsampled array.
    /// </summary>
    /// <param name="sourceArray">The source array to sample from.</param>
    /// <param name="desiredSampleSize">The desired length of the downsampled array.</param>
    /// <returns>A down-sampled array where each element of the source had an equal probability of being included.</returns>
    public static double[] ReservoirSample(IEnumerable<double> source, int desiredSampleSize)
    {
        if (desiredSampleSize < 0)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), $"{nameof(desiredSampleSize)} must be >= 0!");
        Random randomNumberGenerator = new Random();
        IList<double> sourceArray = source as IList<double> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0 || desiredSampleSize == 0) return [];
        if (sourceLength == desiredSampleSize)
        {
            return source.ToArray();
        }
        if (desiredSampleSize > sourceLength)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), $"{nameof(desiredSampleSize)} must be <= {sourceLength}.");
    
        double[] sampledSubset = new double[desiredSampleSize];
        // Initially fill the sampled subset array with the first elements
        for (int initialFillIndex = 0; initialFillIndex < desiredSampleSize; initialFillIndex++)
        {
            sampledSubset[initialFillIndex] = sourceArray[initialFillIndex];
        }

        // Begin filling the random subset with an evenly distributed selection
        double skipWeight = Math.Exp(Math.Log(randomNumberGenerator.NextDouble()) / desiredSampleSize);
        int currentIndex = desiredSampleSize;

        while (currentIndex < sourceLength)
        {
            int elementsToSkip = (int)(Math.Floor(Math.Log(randomNumberGenerator.NextDouble()) / Math.Log(1 - skipWeight)) + 1);

            if (currentIndex >= sourceLength || currentIndex + elementsToSkip < currentIndex) 
                // Either reached the end or detected overflow
            {
                break;
            }
            // Replace a randomly chosen item in the subset with the current item from the source array
            int replacementIndex = randomNumberGenerator.Next(desiredSampleSize);
            sampledSubset[replacementIndex] = sourceArray[currentIndex];
            skipWeight *= Math.Exp(Math.Log(randomNumberGenerator.NextDouble()) / desiredSampleSize);

            currentIndex += elementsToSkip; // Update currentIndex to the new position after skipping
        }

        return sampledSubset;
    }
}