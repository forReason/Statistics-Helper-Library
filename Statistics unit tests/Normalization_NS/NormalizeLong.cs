using System;
using QuickStatistics.Net.AiHelpers;
using Xunit;

namespace Statistics_unit_tests.Normalization_NS;

public class NormalizeLong
{
    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void NormalizeToFloat_ShouldNormalizeCorrectly(long value)
    {
        // Act
        var result = Normalize.NormalizeToFloat(value);

        // Assert
        Assert.Equal(8, result.Length);
        Assert.InRange(result[0], 0.0f, 1.0f);
        Assert.InRange(result[1], 0.0f, 1.0f);
        Assert.InRange(result[2], 0.0f, 1.0f);
        Assert.InRange(result[3], 0.0f, 1.0f);
        Assert.InRange(result[4], 0.0f, 1.0f);
        Assert.InRange(result[5], 0.0f, 1.0f);
        Assert.InRange(result[6], 0.0f, 1.0f);
        Assert.InRange(result[7], 0.0f, 1.0f);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void DenormalizeLong_FromFloatArray_ShouldDenormalizeCorrectly(long value)
    {
        // Arrange
        var normalizedArray = Normalize.NormalizeToFloat(value);

        // Act
        var result = Normalize.DenormalizeLong(normalizedArray);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void DenormalizeLong_FromFloatArray_ShouldThrowForInvalidArray()
    {
        // Arrange
        float[] invalidArray = null;
        float[] singleElementArray = { 0.5f };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Normalize.DenormalizeLong(invalidArray));
        Assert.Throws<ArgumentException>(() => Normalize.DenormalizeLong(singleElementArray));
    }
    [Theory]
    [InlineData(long.MinValue)]
    [InlineData(-1234567890123456789)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1234567890123456789)]
    [InlineData(long.MaxValue)]
    public void NormalizeToDoubleArrayAndBack_ShouldMaintainValue(long originalValue)
    {
        // Act
        var normalizedArray = Normalize.NormalizeToDouble(originalValue);
        var denormalizedValue = Normalize.DenormalizeLong(normalizedArray);

        // Assert
        Assert.Equal(originalValue, denormalizedValue);
    }

    [Fact]
    public void DenormalizeLong_FromDoubleArray_ShouldThrowForInvalidArray()
    {
        // Arrange
        double[] invalidArray = null;
        double[] singleElementArray = { 0.5 };
        double[] outOfRangeArray = { 1.5, -0.1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Normalize.DenormalizeLong(invalidArray));
        Assert.Throws<ArgumentException>(() => Normalize.DenormalizeLong(singleElementArray));
        Assert.Throws<ArgumentOutOfRangeException>(() => Normalize.DenormalizeLong(outOfRangeArray));
    }
}