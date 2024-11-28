using System;
using System.Globalization;
using QuickStatistics.Net.AiHelpers;
using Xunit;

namespace Statistics_unit_tests.Normalization_NS;

public class NormalizeDecimal
{
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("123456789.987654321")]
    [InlineData("-123456789.987654321")]
    [InlineData("0.000000000000000000000000001")]   // Adjusted small value
    [InlineData("-0.000000000000000000000000001")]  // Adjusted small value
    [InlineData("79228162514264337593543950335")]   // decimal.MaxValue
    [InlineData("-79228162514264337593543950335")]  // decimal.MinValue
    public void FloatArrayNormalizationRoundTrip(string decimalString)
    {
        // Arrange
        decimal originalValue = decimal.Parse(decimalString, CultureInfo.InvariantCulture);

        // Act
        float[] normalized = DecimalExtensions.ToNormalizedFloatArray(originalValue);
        decimal reconstructedValue = DecimalExtensions.DecimalFromNormalized(normalized);

        // Assert
        Assert.Equal(originalValue, reconstructedValue);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("123456789.987654321")]
    [InlineData("-123456789.987654321")]
    [InlineData("0.000000000000000000000000001")]   // Adjusted small value
    [InlineData("-0.000000000000000000000000001")]  // Adjusted small value
    [InlineData("79228162514264337593543950335")]   // decimal.MaxValue
    [InlineData("-79228162514264337593543950335")]  // decimal.MinValue
    public void DoubleArrayNormalizationRoundTrip(string decimalString)
    {
        // Arrange
        decimal originalValue = decimal.Parse(decimalString, CultureInfo.InvariantCulture);

        // Act
        double[] normalized = DecimalExtensions.ToNormalizedDoubleArray(originalValue);
        decimal reconstructedValue = DecimalExtensions.DecimalFromNormalized(normalized);

        // Assert
        Assert.Equal(originalValue, reconstructedValue);
    }

        [Fact]
        public void FloatArrayNormalization_InvalidArrayLength()
        {
            // Arrange
            float[] invalidArray = new float[7]; // Should be length 8

            // Act & Assert
            Assert.Throws<ArgumentException>(() => invalidArray.DecimalFromNormalized());
        }

        [Fact]
        public void DoubleArrayNormalization_InvalidArrayLength()
        {
            // Arrange
            double[] invalidArray = new double[4]; // Should be length 5

            // Act & Assert
            Assert.Throws<ArgumentException>(() => invalidArray.DecimalFromNormalized());
        }

        [Fact]
        public void FloatArrayNormalization_InvalidNormalizedValues()
        {
            // Arrange
            float[] invalidArray = new float[8] { 0, 0, 0, 0, 0, 0, 0, 1.5f }; // Sign element out of range

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => invalidArray.DecimalFromNormalized());
        }

        [Fact]
        public void DoubleArrayNormalization_InvalidNormalizedValues()
        {
            // Arrange
            double[] invalidArray = new double[5] { 0, 0, 0, 0, -0.1 }; // Sign element out of range

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => invalidArray.DecimalFromNormalized());
        }

        [Fact]
        public void FloatArrayNormalization_NullArray()
        {
            // Arrange
            float[] nullArray = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DecimalExtensions.DecimalFromNormalized(nullArray));
        }

        [Fact]
        public void DoubleArrayNormalization_NullArray()
        {
            // Arrange
            double[] nullArray = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DecimalExtensions.DecimalFromNormalized(nullArray));
        }

        [Theory]
        [InlineData(28)] // Maximum scale
        [InlineData(0)]  // Minimum scale
        public void FloatArrayNormalization_ScaleEdgeCases(int scale)
        {
            // Arrange
            decimal value = new decimal(1, 0, 0, false, (byte)scale);

            // Act
            float[] normalized = value.ToNormalizedFloatArray();
            decimal reconstructedValue = normalized.DecimalFromNormalized();

            // Assert
            Assert.Equal(value, reconstructedValue);
        }

        [Theory]
        [InlineData(28)] // Maximum scale
        [InlineData(0)]  // Minimum scale
        public void DoubleArrayNormalization_ScaleEdgeCases(int scale)
        {
            // Arrange
            decimal value = new decimal(1, 0, 0, false, (byte)scale);

            // Act
            double[] normalized = value.ToNormalizedDoubleArray();
            decimal reconstructedValue = normalized.DecimalFromNormalized();

            // Assert
            Assert.Equal(value, reconstructedValue);
        }
}