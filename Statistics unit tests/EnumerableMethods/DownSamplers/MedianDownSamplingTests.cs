using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class MedianDownSamplingTests
{
    [Theory]
    [InlineData(new double[] { 3.34}, 1, new double[] { 3.34 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 0, new double[] {  })]
    [InlineData(new double[] { 1.0, 2.0 }, 1, new double[] { 1.5 })]
    [InlineData(new double[] { 1, 3, 2, 7, 6, 4 }, 2, new double[] { 2, 6 })]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, 3, new double[] { 1.5, 3.5, 4.5 })]
    [InlineData(new double[] { 10, 20, 30, 40, 50, 60 }, 2, new double[] { 20, 50 })]
    public void ReturnsExpectedResult(double[] source, int targetLength, double[] expected)
    {
        // Act
        var result = DownSampler.DownSampleMedian(source, targetLength);

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
        double[] expected = { 10.5, 30.5, 50.5, 70.5, 90.5 };

        // Act
        double[] result = DownSampler.DownSampleMedian(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownSampledArray_NoMiddleValue()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 100).Select(x => (double)x);
        int targetLength = 4;
        // Adjusting for medians in 4 equally sized segments
        double[] expected = { 12, 37, 62, 87 };

        // Act
        double[] result = DownSampler.DownSampleMedian(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TargetLengthGreaterThanSource_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0 };
        int targetLength = 5;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMedian(source, targetLength));
    }

    [Fact]
    public void TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMedian(source, targetLength));
    }

    [Fact]
    public void SourceLengthEqualsTargetLength_ReturnsIdenticalArray()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 3;

        // Act
        double[] result = DownSampler.DownSampleMedian(source, targetLength);

        // Assert
        Assert.Equal(source.ToArray(), result);
    }
}