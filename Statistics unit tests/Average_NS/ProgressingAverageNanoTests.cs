using QuickStatistics.Net.Average_NS;
using System;
using Xunit;

namespace Statistics_unit_tests.Average_NS
{
    public class ProgressingAverage_NanoTests
    {
        [Fact]
        public void AddValue_Double_ShouldCalculateCorrectAverage()
        {
            // Arrange
            double currentValue = 0;
            ulong elementCount = 0;
            double inputValue = 10;

            // Act
            bool result = ProgressingAverage_Nano.AddValue(ref currentValue, ref elementCount, inputValue);

            // Assert
            Assert.True(result);
            Assert.Equal(10, currentValue);
            Assert.Equal(1ul, elementCount);
        }
        [Fact]
        public void AddValue_Double_ShouldCalculateCorrectAverageBasic()
        {
            // Arrange
            double currentValue = 0;
            ulong elementCount = 0;

            // Act
            ProgressingAverage_Nano.AddValue(ref currentValue, ref elementCount, 0);
            ProgressingAverage_Nano.AddValue(ref currentValue, ref elementCount, 10);

            // Assert
            Assert.Equal(5, currentValue);
            Assert.Equal(2ul, elementCount);
        }

        [Fact]
        public void AddValue_Float_ShouldCalculateCorrectAverage()
        {
            // Arrange
            float currentValue = 0;
            uint elementCount = 0;
            float inputValue = 5;

            // Act
            bool result = ProgressingAverage_Nano.AddValue(ref currentValue, ref elementCount, inputValue);

            // Assert
            Assert.True(result);
            Assert.Equal(5f, currentValue);
            Assert.Equal(1u, elementCount);
        }

        [Fact]
        public void AddValue_Double_MaxCountReached_ShouldReturnFalse()
        {
            // Arrange
            double currentValue = 0;
            ulong elementCount = ulong.MaxValue;
            double inputValue = 10;

            // Act
            bool result = ProgressingAverage_Nano.AddValue(ref currentValue, ref elementCount, inputValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StaticPositiveValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                ulong count = 0;
                double value = 0;
                double result = rng.NextDouble() * i;
                for (uint b = 0; b < max; b += stepSize)
                {
                    ProgressingAverage_Nano.AddValue(ref value, ref count, result);
                }
                if (value != result)
                {
                    throw new Exception("Value does not add up!");
                }
            }
        }
        /// <summary>
        /// tests a generating the average from a range of random doubles (-0.5 to 0.5)
        /// </summary>
        [Fact]
        public void RandomValue()
        {
            // positive tests
            Random rng = new Random();
            ulong count = 0;
            double value = 0;
            double result = 0;
            uint steps = 0;
            for (uint b = 0; b < 2000; b++)
            {
                double random = rng.NextDouble() - 0.5;
                result += random;
                steps++;
                ProgressingAverage_Nano.AddValue(ref value, ref count, random);
            }
            double endResult = result / (double)steps;
            if (Math.Round(value, 6) != Math.Round(endResult, 6))
            {
                throw new Exception("Value does not add up!");
            }
        }
    }
}
