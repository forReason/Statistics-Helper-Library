using System.Numerics;

namespace QuickStatistics.Net.EnumerableMethods.DownSamplers;

public static partial class DownSampler
{
    /// <summary>
    /// Performs reservoir sampling on an input array to produce a down sampled array.
    /// </summary>
    /// <param name="source">The source array to sample from.</param>
    /// <param name="desiredSampleSize">The desired length of the down sampled array.</param>
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
#if NET7_0_OR_GREATER
    /// <summary>
    /// Performs reservoir sampling on an input array to produce a down sampled array.
    /// </summary>
    /// <param name="source">The source array to sample from.</param>
    /// <param name="desiredSampleSize">The desired length of the down sampled array.</param>
    /// <returns>A down-sampled array where each element of the source had an equal probability of being included.</returns>
    public static T[] ReservoirSample<T>(IEnumerable<T> source, int desiredSampleSize) where T : INumber<T>
    {
        if (desiredSampleSize < 0)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), $"{nameof(desiredSampleSize)} must be >= 0!");
        Random randomNumberGenerator = new Random();
        IList<T> sourceArray = source as IList<T> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0 || desiredSampleSize == 0) return [];
        if (sourceLength == desiredSampleSize)
        {
            return source.ToArray();
        }
        if (desiredSampleSize > sourceLength)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), $"{nameof(desiredSampleSize)} must be <= {sourceLength}.");
    
        T[] sampledSubset = new T[desiredSampleSize];
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
    #endif
    /// <summary>
    /// Performs reservoir sampling on an input array to produce a down sampled array.
    /// </summary>
    /// <param name="source">The source array to sample from.</param>
    /// <param name="desiredSampleSize">The desired length of the down sampled array.</param>
    /// <returns>A down-sampled array where each element of the source had an equal probability of being included.</returns>
    public static decimal[] ReservoirSample(IEnumerable<decimal> source, int desiredSampleSize)
    {
        if (desiredSampleSize < 0)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), $"{nameof(desiredSampleSize)} must be >= 0!");
        Random randomNumberGenerator = new Random();
        IList<decimal> sourceArray = source as IList<decimal> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == 0 || desiredSampleSize == 0) return [];
        if (sourceLength == desiredSampleSize)
        {
            return source.ToArray();
        }
        if (desiredSampleSize > sourceLength)
            throw new ArgumentOutOfRangeException(nameof(desiredSampleSize), $"{nameof(desiredSampleSize)} must be <= {sourceLength}.");
    
        decimal[] sampledSubset = new decimal[desiredSampleSize];
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