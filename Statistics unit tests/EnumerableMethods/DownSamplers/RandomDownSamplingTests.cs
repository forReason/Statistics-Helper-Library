using System;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class RandomDownSamplingTests
{
    [Fact]
    public void ReservoirSample_UniformDistributionTest()
    {
        // Arrange
        double[] sourceArray = Enumerable.Range(1, 10000).Select(x => (double)x).ToArray();
        int desiredSampleSize = 10;
        int iterations = 10000000;
        int[] selectionCounts = new int[sourceArray.Length];

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var sampledSubset = DownSampler.ReservoirSample(sourceArray, desiredSampleSize);
            foreach (var item in sampledSubset)
            {
                // Assuming item is an integer for indexing purposes
                selectionCounts[(int)item - 1]++;
            }
        }

        // Assert
        // Check if each element was selected with approximately equal frequency
        // Allow a margin of error, e.g., within 5% of the mean selection count
        double meanSelectionCount = selectionCounts.Average();
        double allowedDeviation = meanSelectionCount * 0.2; // 5% deviation

        foreach (var count in selectionCounts)
        {
            Assert.InRange(count, meanSelectionCount - allowedDeviation, meanSelectionCount + allowedDeviation);
        }
    }
    [Fact]
    public void ReservoirSample_ReturnsCorrectSampleSize()
    {
        // Arrange
        double[] sourceArray = Enumerable.Range(1, 100).Select(x => (double)x).ToArray();
        int desiredSampleSize = 10;

        // Act
        var sampledSubset = DownSampler.ReservoirSample(sourceArray, desiredSampleSize);

        // Assert
        Assert.Equal(desiredSampleSize, sampledSubset.Length);
    }

    [Fact]
    public void ReservoirSample_ElementsAreFromSource()
    {
        // Arrange
        double[] sourceArray = Enumerable.Range(1, 100).Select(x => (double)x).ToArray();
        int desiredSampleSize = 10;

        // Act
        var sampledSubset = DownSampler.ReservoirSample(sourceArray, desiredSampleSize);

        // Assert
        foreach (var item in sampledSubset)
        {
            Assert.Contains(item, sourceArray);
        }
    }

    [Fact]
    public void ReservoirSample_ThrowsArgumentOutOfRangeExceptionForInvalidSampleSize()
    {
        // Arrange
        double[] sourceArray = Enumerable.Range(1, 100).Select(x => (double)x).ToArray();
        int invalidSampleSize = 101; // Larger than source array length

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.ReservoirSample(sourceArray, invalidSampleSize));
    }

    [Fact]
    public void ReservoirSample_ThrowsArgumentOutOfRangeExceptionForNegativeSampleSize()
    {
        // Arrange
        double[] sourceArray = Enumerable.Range(1, 100).Select(x => (double)x).ToArray();
        int invalidSampleSize = -1; // Negative size

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.ReservoirSample(sourceArray, invalidSampleSize));
    }
}