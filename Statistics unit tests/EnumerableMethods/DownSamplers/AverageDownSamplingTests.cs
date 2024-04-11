using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class AverageDownSamplingTests
{
    [Fact]
    public void ReturnsCorrectlyDownsampledArra_Regular()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 10).Select(x => (double)x);
        int targetLength = 5;
        double[] expected = { 1.5, 3.5, 5.5, 7.5, 9.5 };

        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void ReturnsCorrectlyDownsampledArray_LongArray()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 100).Select(x => (double)x);
        int targetLength = 5;
        double[] expected = { 10.5, 30.5, 50.5, 70.5, 90.5 }; 

        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void ReturnsCorrectlyDownsampledArray_NoMiddleValue()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 100).Select(x => (double)x);
        int targetLength = 4;
        double[] expected = { 13, 38, 63, 88 };

        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void ReturnsCorrectlyDownsampledArray_ShortArray()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 4).Select(x => (double)x);
        int targetLength = 3;
        double[] expected = { 1.5, 2.5, 3.5 };

        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void ReturnsCorrectlyDownsampledArray_SingleValue()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 10).Select(x => (double)x);
        int targetLength = 1;
        double[] expected = { 5.5 };

        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DownSample_TargetLengthGreaterThanSource_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0 };
        int targetLength = 5;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleAverage(source, targetLength));
    }

    [Fact]
    public void DownSample_TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleAverage(source, targetLength));
    }

    [Fact]
    public void DownSample_SourceLengthEqualsTargetLength_ReturnsIdenticalArray()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 3;

        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

        // Assert
        Assert.Equal(source.ToArray(), result);
    }
}