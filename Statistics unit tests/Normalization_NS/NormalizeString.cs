using QuickStatistics.Net.AiHelpers;
using Xunit;

namespace Statistics_unit_tests.Normalization_NS;

public class NormalizeString
{
    [Fact]
        public void NormalizeString_ShouldReturnTwoFloats()
        {
            // Arrange
            string input = "TestString";

            // Act
            float[] result = Normalize.NormalizeToFloat(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);

            // Validate range of normalized floats
            Assert.InRange(result[0], 0.0f, 1.0f);
            Assert.InRange(result[1], 0.0f, 1.0f);
        }

        [Fact]
        public void NormalizeString_ShouldBeConsistent()
        {
            // Arrange
            string input = "ConsistentTest";

            // Act
            float[] result1 = Normalize.NormalizeToFloat(input);
            float[] result2 = Normalize.NormalizeToFloat(input);

            // Assert
            Assert.Equal(result1[0], result2[0]);
            Assert.Equal(result1[1], result2[1]);
        }

        [Fact]
        public void NormalizeString_DifferentInputs_ShouldProduceDifferentResults()
        {
            // Arrange
            string input1 = "InputOne";
            string input2 = "Inputone";

            // Act
            float[] result1 = Normalize.NormalizeToFloat(input1);
            float[] result2 = Normalize.NormalizeToFloat(input2);

            // Assert
            Assert.NotEqual(result1[0], result2[0]);
            Assert.NotEqual(result1[1], result2[1]);
        }

        [Fact]
        public void NormalizeString_EmptyString_ShouldReturnValidFloats()
        {
            // Arrange
            string input = "";

            // Act
            float[] result = Normalize.NormalizeToFloat(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);

            // Validate range of normalized floats
            Assert.InRange(result[0], 0.0f, 1.0f);
            Assert.InRange(result[1], 0.0f, 1.0f);
        }

        [Fact]
        public void NormalizeString_LongString_ShouldReturnValidFloats()
        {
            // Arrange
            string input = new string('a', 1000); // Very long string

            // Act
            float[] result = Normalize.NormalizeToFloat(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);

            // Validate range of normalized floats
            Assert.InRange(result[0], 0.0f, 1.0f);
            Assert.InRange(result[1], 0.0f, 1.0f);
        }
}