using System;
using System.Collections.Generic;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class NearestNeighbourUpsamplingTests
{
    [Fact]
    public void UpSampleNearestNeighbor_WithValidInput_ShouldReturnCorrectlyUpSampledArray()
    {
        // Arrange
        var source = new List<double> { 1.0, 2.0, 3.0 };
        int targetLength = 6;
        var expected = new double[] { 1.0, 1.0, 2.0, 2.0, 3.0, 3.0 };

        // Act
        var result = UpSampler.UpSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpSampleNearestNeighbor_WithEmptySource_ShouldReturnEmptyArray()
    {
        // Arrange
        var source = new List<double>();
        int targetLength = 10;
        var expected = new double[] { };

        // Act
        var result = UpSampler.UpSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpSampleNearestNeighbor_WithTargetLengthGreaterThanSourceLength_ShouldCorrectlyInterpolate()
    {
        // Arrange
        var source = new List<double> { 1.0, 2.0, 3.0 };
        int targetLength = 5;
        var expected = new double[] { 1.0, 1.0, 2.0, 3.0, 3.0 };

        // Act
        var result = UpSampler.UpSampleNearestNeighbor(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpSampleNearestNeighbor_WithInvalidTargetLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var source = new List<double> { 1.0, 2.0, 3.0 };
        int targetLength = -1;

        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleNearestNeighbor(source, targetLength));
        Assert.Equal("targetLength", ex.ParamName);
    }
}