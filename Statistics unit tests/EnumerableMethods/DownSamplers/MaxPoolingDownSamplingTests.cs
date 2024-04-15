using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class MaxPoolingDownSamplingTests
{
    [Theory]
    [InlineData(new double[] { 1, 3, 2, 4, 6, 7 }, 2, new double[] { 3, 7 })]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, 3, new double[] { 2, 4, 5 })]
    [InlineData(new double[] { 10, 20, 30, 40, 50, 60 }, 2, new double[] { 30, 60 })]
    public void DownSampleMaxPooling_ValidInput_ReturnsExpectedResult(double[] source, int targetLength, double[] expected)
    {
        // Arrange

        // Act
        var result = DownSampler.DownSampleMaxPooling(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DownSampleMaxPooling_TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var source = new double[] { 1, 2, 3, 4, 5 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMaxPooling(source, targetLength));
    }

    [Fact]
    public void DownSampleMaxPooling_SourceLengthLessThanTargetLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var source = new double[] { 1, 2, 3, 4, 5 };
        int targetLength = 6;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMaxPooling(source, targetLength));
    }
}