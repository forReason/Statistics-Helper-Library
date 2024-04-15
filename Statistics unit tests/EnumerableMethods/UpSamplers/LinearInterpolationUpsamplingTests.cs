using System;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class LinearInterpolationUpsamplingTests
{
    [Fact]
    public void UpSampleLinearInterpolation_ReturnsCorrectLength()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = 6;

        // Act
        double[] result = UpSampler.UpSampleLinearInterpolation(source, targetLength);

        // Assert
        Assert.Equal(targetLength, result.Length);
    }

    [Fact]
    public void UpSampleLinearInterpolation_ThrowsException_IfTargetLengthIsInvalid()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleLinearInterpolation(source, targetLength));
    }
    
    [Fact]
    public void UpSampleLinearInterpolation_ReturnsExpectedValues()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = 9;
        double[] expected = { 1, 1.25 ,1.5, 1.75, 2, 2.25, 2.5, 2.75, 3 };

        // Act
        double[] result = UpSampler.UpSampleLinearInterpolation(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
}