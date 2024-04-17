using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.Median_NS;
using System;
using Xunit;

namespace Statistics_unit_tests.Median_NS
{
    public class ProgressingMedianTests
    {
        [Fact]
        public void TestAddingOneValue()
        {
            ProgressingMedian_Double med = new ProgressingMedian_Double { };
            med.AddValue(10.5);
            //Assert.Equal(1, med.Count);
            Assert.Equal(10.5, med.GetMedian());
        }
        [Fact]
        public void TestAddingMultipleValues()
        {
            ProgressingMedian_Decimal med = new ProgressingMedian_Decimal();
            var maxShortValue = (short.MaxValue - 10); // Subtract 10 to reach the overflow point quickly
            decimal expectedMedianBeforeOverflow, expectedMedianAfterOverflow;

            // Add values until idCounter is about to overflow
            for (short i = 0; i < maxShortValue; i++)
            {
                med.AddValue(i);
            }

            // Calculate expected median before overflow
            expectedMedianBeforeOverflow = maxShortValue / 2 -1;

            // Check the correctness of the median before overflow
            var medianBeforeOverflow = med.GetMedian();

            // Assert for correct behavior before and after overflow
            Assert.Equal(expectedMedianBeforeOverflow, medianBeforeOverflow);
        }
        [Fact]
        public void TestEmptyValue()
        {
            ProgressingMedian_Double med = new ProgressingMedian_Double();

            Assert.Throws<InvalidOperationException>(() => med.GetMedian());
        }
        [Fact]
        public void TestClear()
        {
            // add pre clear values
            ProgressingMedian_Double med = new ProgressingMedian_Double();
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
    }
}
