using System;
using QuickStatistics.Net.AiHelpers;
using Xunit;

namespace Statistics_unit_tests.Normalization_NS;

public class NormalizeDateTime
{
    [Fact]
    public void NormalizeDateTimeDouble_ReturnsCorrectNormalizedValues()
    {
        // Arrange
        DateTime testDate = new DateTime(2024, 11, 28, 12, 0, 0, DateTimeKind.Utc);

        // Act
        double[] result = Normalize.NormalizeToDouble(testDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.InRange(result[0], 0.0, 1.0);
        Assert.InRange(result[1], 0.0, 1.0);
    }

    [Fact]
    public void DenormalizeDateTimeDouble_ReturnsCorrectDateTime()
    {
        // Arrange
        DateTime originalDate = new DateTime(2024, 11, 28, 12, 0, 0, DateTimeKind.Utc);
        double[] normalized = Normalize.NormalizeToDouble(originalDate);

        // Act
        DateTime result = Normalize.DenormalizeDateTime(normalized);

        // Assert
        Assert.Equal(originalDate, result);
    }

    [Fact]
    public void NormalizeDateTimeFloat_ReturnsCorrectNormalizedValues()
    {
        // Arrange
        DateTime testDate = new DateTime(2024, 11, 28, 12, 0, 0, DateTimeKind.Utc);

        // Act
        float[] result = Normalize.NormalizeToFloat(testDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Length);
        foreach (float value in result)
        {
            Assert.InRange(value, 0.0f, 1.0f);
        }
    }

    [Fact]
    public void DenormalizeDateTimeFloat_ReturnsCorrectDateTime()
    {
        // Arrange
        DateTime originalDate = new DateTime(2024, 11, 28, 12, 0, 0, DateTimeKind.Utc);
        float[] normalized = Normalize.NormalizeToFloat(originalDate);

        // Act
        DateTime result = Normalize.DenormalizeDateTime(normalized);

        // Assert
        Assert.Equal(originalDate, result);
    }

    [Fact]
    public void DenormalizeDateTimeDouble_InvalidInput_ThrowsException()
    {
        // Arrange
        double[] invalidInput = { 0.5 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Normalize.DenormalizeDateTime(invalidInput));
    }

    [Fact]
    public void DenormalizeDateTimeFloat_InvalidInput_ThrowsException()
    {
        // Arrange
        float[] invalidInput = { 0.5f, 0.5f };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Normalize.DenormalizeDateTime(invalidInput));
    }
}