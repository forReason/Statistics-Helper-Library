﻿using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.Median_NS;
using System;
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
    }
}