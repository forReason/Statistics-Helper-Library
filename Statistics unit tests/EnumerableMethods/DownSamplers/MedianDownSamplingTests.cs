using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class MedianDownSamplingTests
{
    [Fact]
    public void ReturnsCorrectlyDownsampledArray_Regular()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 10).Select(x => (double)x);
        int targetLength = 5;
        // For median downsampling, choose the median of each segment
        double[] expected = { 1.5, 3.5, 5.5, 7.5, 9.5 };

        // Act
        double[] result = DownSampler.DownSampleMedian(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownsampledArray_LongArray()
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
    public void ReturnsCorrectlyDownsampledArray_NoMiddleValue()
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
    public void ReturnsCorrectlyDownsampledArray_ShortArray()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 4).Select(x => (double)x);
        int targetLength = 3;
        // For median downsampling, select median values based on segment division
        double[] expected = { 1.5, 2.5, 3.5 }; // Might adjust based on exact downsampling logic

        // Act
        double[] result = DownSampler.DownSampleMedian(source, targetLength);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsCorrectlyDownsampledArray_SingleValue()
    {
        // Arrange
        IEnumerable<double> source = Enumerable.Range(1, 10).Select(x => (double)x);
        int targetLength = 1;
        // Median of 1-10
        double[] expected = { 5.5 };

        // Act
        double[] result = DownSampler.DownSampleMedian(source, targetLength);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMedian(source, targetLength));
    }

    [Fact]
    public void DownSample_TargetLengthLessThanOne_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleMedian(source, targetLength));
    }

    [Fact]
    public void DownSample_SourceLengthEqualsTargetLength_ReturnsIdenticalArray()
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