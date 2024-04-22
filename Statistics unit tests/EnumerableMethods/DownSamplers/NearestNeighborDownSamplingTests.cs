using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class NearestNeighborDownSamplingTests
{
    [Theory]
    [InlineData(new double[] { 3.34}, 1, new double[] { 3.34 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 0, new double[] {  })]
    [InlineData(new double[] { 1.0, 2.0 }, 1, new double[] { 2 })]
    [InlineData(new double[] { 1, 3, 2, 4, 6, 7 }, 2, new double[] { 1, 7 })]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, 3, new double[] { 1, 3, 5 })]
    [InlineData(new double[] { 10, 20, 30, 40, 50, 60 }, 3, new double[] { 10, 30, 60 })]
    public void ReturnsExpectedResult(double[] source, int targetLength, double[] expected)
    {
        // Arrange

        // Act
        var result = DownSampler.DownSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsExpectedResult_LongArray()
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
    public void ReturnsCorrectlyDownSampledArray_NoMiddleValue()
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
    public void ReturnsCorrectlyDownSampledArray_SingleValue()
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
    public void TargetLengthGreaterThanSource_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0 };
        int targetLength = 5;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleNearestNeighbor(source, targetLength));
    }

    [Fact]
    public void TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleNearestNeighbor(source, targetLength));
    }

    [Fact]
    public void SourceLengthEqualsTargetLength_ReturnsIdenticalArray()
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
