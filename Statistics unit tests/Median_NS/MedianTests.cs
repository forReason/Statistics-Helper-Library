using QuickStatistics.Net.Median_NS;
using System;
using System.Collections.Generic;
using Xunit;

namespace Statistics_unit_tests.Median_NS
{
    public class MedianTests
    {
        [Fact]
        public void GetMedian_Array_ReturnsCorrectMedian()
        {
            // Arrange
            double[] numbers = { 5, 1, 9, 3, 7 };

            // Act
            double result = Median_Double.GetMedian(numbers);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetMedian_SortedArray_ReturnsCorrectMedian()
        {
            // Arrange
            double[] numbers = { 1, 3, 5, 7, 9 };

            // Act
            double result = Median_Double.GetMedian(numbers, true);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetMedian_ArrayEvenLength_ReturnsCorrectMedian()
        {
            // Arrange
            double[] numbers = { 1, 2, 3, 4 };

            // Act
            double result = Median_Double.GetMedian(numbers);

            // Assert
            Assert.Equal(2.5, result);
        }

        [Fact]
        public void GetMedian_List_ReturnsCorrectMedian()
        {
            // Arrange
            List<double> numbers = new List<double> { 5, 1, 9, 3, 7 };

            // Act
            double result = Median_Double.GetMedian(numbers);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetMedian_EmptyArray_ThrowsArgumentException()
        {
            // Arrange
            double[] numbers = { };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Median_Double.GetMedian(numbers));
        }

        [Fact]
        public void GetMedian_NullArray_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Median_Double.GetMedian((double[])null));
        }

        [Fact]
        public void GetMedian_EmptyList_ThrowsArgumentException()
        {
            // Arrange
            List<double> numbers = new List<double>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Median_Double.GetMedian(numbers));
        }

        [Fact]
        public void GetMedian_NullList_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Median_Double.GetMedian((List<double>)null));
        }
    }
}
