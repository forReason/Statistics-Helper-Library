using System;
using QuickStatistics.Net.Math_NS;
using Xunit;

namespace Statistics_unit_tests.Math_NS;

public class DecimalMathSqrtTests
{
    [Theory]
    [InlineData(4, 2)]
    [InlineData(9, 3)]
    [InlineData(16, 4)]
    [InlineData(25, 5)]
    public void ShouldReturnCorrectRoots(decimal input, decimal expected)
    {
        decimal result = DecimalMath.Sqrt(input);
        Assert.Equal(expected, result, 5); // 5 decimal places of precision
    }

    [Fact]
    public void ShouldThrowOnNegativeInput()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => DecimalMath.Sqrt(-1));
        Assert.Equal("number", ex.ParamName);
        Assert.Contains("negative numbers is not defined", ex.Message);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0.0001, 0.01)]
    [InlineData(0.25, 0.5)]
    public void ShouldHandleSmallNumbers(decimal input, decimal expected)
    {
        decimal result = DecimalMath.Sqrt(input);
        Assert.Equal(expected, result, 5); // 5 decimal places of precision
    }

    [Theory]
    [InlineData(1000000, 1000)]
    [InlineData(100000000, 10000)]
    public void ShouldHandleLargeNumbers(decimal input, decimal expected)
    {
        decimal result = DecimalMath.Sqrt(input);
        Assert.Equal(expected, result, 5); // 5 decimal places of precision
    }

    [Fact]
    public void PerformanceTest()
    {
        decimal number = 12345.6789m;
        var watch = System.Diagnostics.Stopwatch.StartNew();

        DecimalMath.Sqrt(number);

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Assert.True(elapsedMs < 1000, "Expected the calculation to take less than 1 second.");
    }
}