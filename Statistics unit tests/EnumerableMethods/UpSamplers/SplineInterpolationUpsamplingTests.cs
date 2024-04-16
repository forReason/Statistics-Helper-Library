using System;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class SplineInterpolationUpsamplingTests
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
        double[] result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleSplineInterpolation(source, targetLength));
    }

    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 5, 
        new double[] { 1.0, 1.4375, 2.0, 2.46875, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 6, 
        new double[] { 1.0, 1.3039999999999998, 1.8320000000000001, 2.1680000000000001, 2.5760000000000001, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0 }, 4, 
        new double[] { 1.0, 1.1851851851851853, 1.5925925925925923, 2.0 })]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 10, 
        new double[] { 1.0, 1.6641975308641976, 2.5506172839506172, 2.0, -1.4098765432098757, 
            -4.5530864197530851, -5, -0.84444444444444822, 3.5777777777777757, 8 })]
    public void ReturnsExpectedValues(double[] source, int targetLength, double[] expected)
    {
        // Act
        var result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

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
        var result = UpSampler.UpSampleSplineInterpolation(source, 10);

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
        double[] result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
}