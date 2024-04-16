using System;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class LinearInterpolationUpsamplingTests
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
        double[] result = UpSampler.UpSampleLinearInterpolation(source, targetLength);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleLinearInterpolation(source, targetLength));
    }
    
    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 5, 
        new double[] { 1.0, 1.5, 2.0, 2.5, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 6, 
        new double[] { 1.0, 1.3999999999999999, 1.8, 2.2000000000000002, 2.6000000000000001, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0 }, 4, 
        new double[] { 1.0, 1.3333333333333333, 1.6666666666666665, 2.0 })]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 10, 
        new double[] { 1.0, 1.3333333333333333, 1.6666666666666665, 2.0, -0.33333333333333304, 
            -2.6666666666666661, -5, -0.66666666666667052, 3.6666666666666643, 8 })]
    public void ReturnsExpectedValues(double[] source, int targetLength, double[] expected)
    {
        // Act
        double[] result = UpSampler.UpSampleLinearInterpolation(source, targetLength);

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
        var result = UpSampler.UpSampleLinearInterpolation(source, 10);

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
        double[] result = UpSampler.UpSampleLinearInterpolation(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
}