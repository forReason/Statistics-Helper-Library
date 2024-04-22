using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class MaxPoolingDownSamplingTests
{
    [Theory]
    [InlineData(new double[] { 3.34}, 1, new double[] { 3.34 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 0, new double[] {  })]
    [InlineData(new double[] { 1.0, 2.0 }, 1, new double[] { 2.0 })]
    [InlineData(new double[] { 1, 3, 2, 4, 6, 7 }, 2, new double[] { 3, 7 })]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, 3, new double[] { 2, 4, 5 })]
    [InlineData(new double[] { 10, 20, 30, 40, 50, 60 }, 2, new double[] { 30, 60 })]
    public void ReturnsExpectedResult(double[] source, int targetLength, double[] expected)
    {
        // Act
        var result = DownSampler.DownSampleMaxPooling(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void ReturnsExpectedResult_LongArray()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 100).Select(x => (double)x);
        int targetLength = 5;
        // For a range of 1-100 divided into 5 parts, each segment has 20 elements, median is middle
        double[] expected = { 20, 40, 60, 80, 100 };

        // Act
        double[] result = DownSampler.DownSampleMaxPooling(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var source = new double[] { 1, 2, 3, 4, 5 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMaxPooling(source, targetLength));
    }

    [Fact]
    public void SourceLengthLessThanTargetLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var source = new double[] { 1, 2, 3, 4, 5 };
        int targetLength = 6;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMaxPooling(source, targetLength));
    }
}