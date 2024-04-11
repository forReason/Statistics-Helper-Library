namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
    /// <summary>
    /// Performs reservoir sampling on an input array to produce a downsampled array.
    /// </summary>
    /// <param name="sourceArray">The source array to sample from.</param>
    /// <param name="desiredSampleSize">The desired length of the downsampled array.</param>
    /// <returns>A down-sampled array where each element of the source had an equal probability of being included.</returns>
    public static double[] ReservoirSample(double[] sourceArray, int desiredSampleSize)
    {
        Random randomNumberGenerator = new Random();
        if (desiredSampleSize < 1 || desiredSampleSize > sourceArray.Length)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), "Desired sample size must be within the bounds of the source array size.");
    
        double[] sampledSubset = new double[desiredSampleSize];
        // Initially fill the sampled subset array with the first elements
        for (int initialFillIndex = 0; initialFillIndex < desiredSampleSize; initialFillIndex++)
        {
            sampledSubset[initialFillIndex] = sourceArray[initialFillIndex];
        }

        // Begin filling the random subset with an evenly distributed selection
        double skipWeight = Math.Exp(Math.Log(randomNumberGenerator.NextDouble()) / desiredSampleSize);
        int currentIndex = desiredSampleSize;

        while (currentIndex < sourceArray.Length)
        {
            int elementsToSkip = (int)(Math.Floor(Math.Log(randomNumberGenerator.NextDouble()) / Math.Log(1 - skipWeight)) + 1);

            if (currentIndex >= sourceArray.Length || currentIndex + elementsToSkip < currentIndex) 
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