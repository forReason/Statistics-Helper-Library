using System;
using System.Collections.Generic;
using System.Linq;
using QuickStatistics.Net.EnumerableMethods.DownSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.DownSamplers;

public class AverageDownSamplingTests
{
    [Theory]
    [InlineData(new double[] { 3.34}, 1, new double[] { 3.34 })]
    [InlineData(new double[] { 1.0, 2.0, 3.0 }, 0, new double[] {  })]
    [InlineData(new double[] { 1.0, 2.0 }, 1, new double[] { 1.5 })]
    [InlineData(new double[] { 1.0, 2.0, -5, 8, 10, 22, 3,1,6,7,4,2  }, 3, 
        new double[] { 1.5, 9, 4.75})]
    public void ReturnsExpectedResult(double[] source, int targetLength, double[] expected)
    {
        // Act
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

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
        double[] result = DownSampler.DownSampleAverage(source, targetLength);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleAverage(source, targetLength));
    }

    [Fact]
    public void TargetLengthLessThanZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        IEnumerable<double> source = new double[] { 1.0, 2.0, 3.0 };
        int targetLength = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => DownSampler.DownSampleAverage(source, targetLength));
    }

    [Fact]
    public void SourceLengthEqualsTargetLength_ReturnsIdenticalArray()
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