using System;
using System.Text;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class SplineInterpolationUpsamplingTests
{
    [Fact]
    public void UpSampleSplineInterpolation_ReturnsCorrectLength()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = 6;

        // Act
        double[] result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

        // Assert
        Assert.Equal(targetLength, result.Length);
    }

    [Fact]
    public void UpSampleSplineInterpolation_ThrowsException_IfTargetLengthIsInvalid()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleSplineInterpolation(source, targetLength));
    }

    [Fact]
    public void UpSampleSplineInterpolation_ReturnsExpectedValues()
    {
        // Arrange
        double[] source = { 1, 3, 5 };
        int targetLength = 20;
        double[] expected = { 1, 1.6, 2.2, 2.8, 3 };

        // Act
        double[] result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

        StringBuilder SplineString = new();
        foreach (double value in result)
        {
            SplineString.Append(Math.Round(value,4).ToString());
            SplineString.Append(',');
        }
        // Assert
        Assert.Equal(expected, result);
    }
}