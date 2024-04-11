using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class NearestNeighborDownSamplingTests
{
    [Fact]
    public void ReturnsCorrectlyDownsampledArray_Regular()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 10).Select(x => (double)x);
        int targetLength = 5;
        double[] expected = { 1, 3, 5, 8, 10 };

        // Act
        double[] result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownsampledArray_LongArray()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(0, 1000).Select(x => (double)x);
        int targetLength = 10;
        double[] expected = { 0, 111, 222, 333, 444, 555, 666, 777, 888, 999 };

        // Act
        double[] result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownsampledArray_NoMiddleValue()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 100).Select(x => (double)x);
        int targetLength = 4;
        double[] expected = { 1, 34, 67, 100 };

        // Act
        double[] result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownsampledArray_ShortArray()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 4).Select(x => (double)x);
        int targetLength = 3;
        double[] expected = { 1, 3, 4 }; // Adjusted expected values

        // Act
        double[] result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownsampledArray_SingleValue()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 10).Select(x => (double)x);
        int targetLength = 2;
        double[] expected = { 1,10 };// Adjusted expected values

        // Act
        double[] result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DownSample_TargetLengthGreaterThanSource_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0 };
        int targetLength = 5;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleNearestNeighbor(source, targetLength));
    }

    [Fact]
    public void DownSample_TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleNearestNeighbor(source, targetLength));
    }

    [Fact]
    public void DownSample_SourceLengthEqualsTargetLength_ReturnsIdenticalArray()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 3;

        // Act
        double[] result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(source.ToArray(), result);
    }
}
