using System;
using System.Collections.Generic;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class RepetitionUpsamplingTests
{
    [Fact]
    public void UpSampleRepetition_WithValidInput_ShouldReturnCorrectlyUpSampledArray()
    {
        // Arrange
        var source = new List<double> { 1.0, 2.0, 3.0 };
        int targetLength = 6;
        var expected = new double[] { 1.0, 1.0, 2.0, 2.0, 3.0, 3.0 };

        // Act
        var result = UpSampler.UpSampleRepetition(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpSampleRepetition_WithEmptySource_ShouldReturnEmptyArray()
    {
        // Arrange
        var source = new List<double>();
        int targetLength = 10;
        var expected = new double[] { };

        // Act
        var result = UpSampler.UpSampleRepetition(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 5, new double[] { 1.0, 1.0, 2.0, 2.0, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 6, new double[] { 1.0, 1.0, 2.0, 2.0, 3.0, 3.0 })]
    [InlineData(new double[] { 1.0, 2.0 }, 4, new double[] { 1.0, 1.0, 2.0, 2.0 })]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, }, 10, new double[] { 1.0, 1.0, 1.0, 2.0, 2.0, -5, -5, -5, 8, 8 })]
    public void UpSampleRepetition_WithTargetLengthGreaterThanSourceLength_ShouldCorrectlyInterpolate(double[] source, int targetLength, double[] expected)
    {
        // Arrange
        var sourceList = new List<double>(source);

        // Act
        var result = UpSampler.UpSampleRepetition(sourceList, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpSampleRepetition_WithInvalidTargetLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var source = new List<double> { 1.0, 2.0, 3.0 };
        int targetLength = -1;

        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleRepetition(source, targetLength));
        Assert.Equal("targetLength", ex.ParamName);
    }
}