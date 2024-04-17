using QuickStatistics.Net.Median_NS;
using System;
using System.Collections.Generic;
using Xunit;

namespace Statistics_unit_tests.Median_NS
{
    public class MovingMedianTests
    {
        [Fact]
        public void TestAddingOneValue()
        {
            MovingMedian_Double med = new MovingMedian_Double(10);
            med.AddValue(10.5);
            //Assert.Equal(1, med.Count);
            Assert.Equal(10.5, med.GetMedian());
        }
        [Theory]
        [InlineData(new double[] { 1, 3, 2 }, 2 )]
        [InlineData(new double[] { 1, 4, 3, 5,7,23,2,1,9,3 }, 3.5 )]
        public void ReturnsExpectedResult(double[] source, double expected)
        {
            // Act
            MovingMedian_Double med = new MovingMedian_Double(source.Length);
            foreach(double input in source) med.AddValue(input);
            //Assert.Equal(1, med.Count);
            Assert.Equal(expected, med.GetMedian());
        }
        [Theory]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0,0 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.1,1 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.2,2 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.3,3 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.4,4 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.5,5 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.6,6 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.7,7 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.8,8 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.9,9 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 1,10 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.05,0.5 )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.95,9.5 )]
        public void TestGetPercentile(double[] source, double percentile ,double expected)
        {
            // Act
            MovingMedian_Double med = new MovingMedian_Double(source.Length);
            foreach(double input in source) med.AddValue(input);
            double result = med.GetPercentile(percentile);
            Assert.Equal(expected, result);
        }
        [Theory]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0,0,1  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 0.5,0,1  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 3,3,4  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 4,4,5  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 4.5,4,5  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 5,5,6  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 8,8,9  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 8.7,8,9  )]
        [InlineData(new double[] { 0, 1, 2, 3, 4,5,6,7,8,9,10 }, 9.9,9, 10  )]
        public void GetBracket(double[] source, double preciseIndex , double firstResult, double secondResult)
        {
            // Act
            MovingMedian_Double med = new MovingMedian_Double(source.Length);
            foreach(double input in source) med.AddValue(input);
            (double,double) result = med.GetBracket(preciseIndex);
            Assert.Equal(firstResult, result.Item1);
            Assert.Equal(secondResult, result.Item2);
        }
        [Fact]
        public void TestAddingMultipleValues()
        {
            MovingMedian_Decimal med = new MovingMedian_Decimal(10);
            var maxShortValue = (short.MaxValue - 10); // Subtract 10 to reach the overflow point quickly
            decimal expectedMedianBeforeOverflow, expectedMedianAfterOverflow;

            // Add values until idCounter is about to overflow
            for (short i = 0; i < maxShortValue; i++)
            {
                med.AddValue(i);
            }

            // Calculate expected median before overflow
            expectedMedianBeforeOverflow = ((maxShortValue + (maxShortValue - 10)) / 2m) - 0.5m;

            // Check the correctness of the median before overflow
            var medianBeforeOverflow = med.GetMedian();

            // Add a few more values to cause an overflow
            for (short i = 0; i < 20; i++)
            {
                med.AddValue(i);
            }

            Assert.Equal(expectedMedianBeforeOverflow, medianBeforeOverflow);
        }
        [Fact]
        public void TestEmptyValue()
        {
            MovingMedian_Double med = new MovingMedian_Double(10);

            Assert.Throws<InvalidOperationException>(() => med.GetMedian());
        }
        [Fact]
        public void TestClear()
        {
            // add pre clear values
            MovingMedian_Double med = new MovingMedian_Double(10);
            med.AddValue(5);
            med.AddValue(5);
            med.AddValue(5);
            Assert.Equal(5, med.GetMedian());

            // clear
            med.Clear();
            Assert.False(med.ContainsValues);

            // add pre clear values
            med.AddValue(10);
            med.AddValue(10);
            med.AddValue(10);
            Assert.Equal(10, med.GetMedian());
        }

        [Fact]
        public void Test_IdCounter_Overflow_And_Median_Correctness()
        {
            var medianCalculator = new MovingMedianOverflowTestClass(10);
            var maxShortValue = (short.MaxValue - 10); // Subtract 10 to reach the overflow point quickly
            decimal expectedMedianBeforeOverflow, expectedMedianAfterOverflow;

            // Add values until idCounter is about to overflow
            for (short i = 0; i < maxShortValue; i++)
            {
                medianCalculator.AddValue(i);
            }

            // Calculate expected median before overflow
            expectedMedianBeforeOverflow = ((maxShortValue + (maxShortValue - 10)) / 2m)-0.5m;

            // Check the correctness of the median before overflow
            var medianBeforeOverflow = medianCalculator.GetMedian();

            // Add a few more values to cause an overflow
            for (short i = 0; i < 20; i++)
            {
                medianCalculator.AddValue(i);
            }

            // Calculate expected median after overflow
            // This calculation depends on your algorithm's handling of the overflow
            // Here's an example assuming the latest 10 values are considered for the median
            expectedMedianAfterOverflow = 14.5m; // Median of last 10 values (10, 11, 12, 13, 14, 15, 16, 17, 18, 19)

            // Check the correctness of the median after overflow
            var medianAfterOverflow = medianCalculator.GetMedian();

            // Assert for correct behavior before and after overflow
            Assert.Equal(expectedMedianBeforeOverflow, medianBeforeOverflow);
            Assert.Equal(expectedMedianAfterOverflow, medianAfterOverflow);
        }

        [Fact]
        public void GenerateDistribution()
        {
            Random rng = new Random();
            int numbers = 100;
            MovingMedian_Double median = new MovingMedian_Double(numbers);
            for (int i = 0; i < numbers; i++)
            {
                median.AddValue(rng.NextDouble()*1000);
            }

            SortedDictionary<double, int> result = median.GenerateDistribution(15);
            string excelKeys = string.Join(",", result.Keys);
            string excelValues = string.Join(",", result.Values);
            Console.WriteLine(excelKeys);
        }
    }
}