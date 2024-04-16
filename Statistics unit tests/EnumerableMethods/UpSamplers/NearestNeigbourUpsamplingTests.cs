using System;
using System.Collections.Generic;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class NearestNeighbourUpsamplingTests
{
    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 5)]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 6)]
    [InlineData(new double[] { 1.0, 2.0 }, 4)]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 10)]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 100)]
    public void ReturnsCorrectLength(double[] source, int targetLength)
    {
        // Act
        double[] result = UpSampler.UpSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(targetLength, result.Length);
    }
    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 2)]
    [InlineData(new double[] { 1.0, 2.0 }, 1)]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 3)]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, -1)]
    public void ThrowsException_IfTargetLengthIsInvalid(double[] source, int targetLength)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleNearestNeighbor(source, targetLength));
    }
    
    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 5, 
        new double[] { 1.0, 1.0, 2.0, 3.0, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 6, 
        new double[] { 1.0, 1.0, 2.0, 2.0, 3.0, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0 }, 4, 
        new double[] { 1.0, 1.0, 2.0, 2.0 })]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 10, 
        new double[] { 1.0, 1.0, 2.0, 2.0, 2.0, -5, -5, -5, 8, 8 })]
    public void ReturnsExpectedValues(double[] source, int targetLength, double[] expected)
    {
        // Act
        double[] result = UpSampler.UpSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void WithEmptySource_ShouldReturnEmptyArray()
    {
        // Arrange
        double[] source = {};
        double[] expected = {};

        // Act
        var result = UpSampler.UpSampleNearestNeighbor(source, 10);

        // Assert
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 3, 
        new double[] { 1.0, 2.0, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0 }, 2, 
        new double[] { 1.0, 2.0 })]
    [InlineData(new double[] { 1.0, 2.0, -5, 8 }, 4, 
        new double[] { 1.0, 2.0, -5, 8 })]
    public void WithSameLength_DoesNotChange(double[] source, int targetLength, double[] expected)
    {
        // Act
        double[] result = UpSampler.UpSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
}