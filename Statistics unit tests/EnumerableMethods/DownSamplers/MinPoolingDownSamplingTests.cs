using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class MinPoolingDownSamplingTests
{
    [Theory]
    [InlineData(new double[] { 3.34}, 1, new double[] { 3.34 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 0, new double[] {  })]
    [InlineData(new double[] { 1.0, 2.0 }, 1, new double[] { 1 })]
    [InlineData(new double[] { 1, 3, 2, 4, 6, 7 }, 2, new double[] { 1, 4 })]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, 3, new double[] { 1, 3, 4 })]
    [InlineData(new double[] { 10, 20, 30, 40, 50, 60 }, 2, new double[] { 10, 40 })]
    public void ReturnsExpectedResult(double[] source, int targetLength, double[] expected)
    {
        // Arrange

        // Act
        var result = DownSampler.DownSampleMinPooling(source, targetLength);

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
        double[] expected = { 1, 21, 41, 61, 81 };

        // Act
        double[] result = DownSampler.DownSampleMinPooling(source, targetLength);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMinPooling(source, targetLength));
    }

    [Fact]
    public void SourceLengthLessThanTargetLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var source = new double[] { 1, 2, 3, 4, 5 };
        int targetLength = 6;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMinPooling(source, targetLength));
    }
}