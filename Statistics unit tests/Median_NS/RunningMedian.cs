using QuickStatistics.Net.Average_NS;
using QuickStatistics.Net.Median_NS;
using System;
using Xunit;

namespace Statistics_unit_tests.Median_NS
{
    public class RunningMedian
    {
        [Fact]
        public void TestAddingOneValue()
        {
            RunningMedian_Double med = new RunningMedian_Double { };
            med.AddValue(10.5);
            //Assert.Equal(1, med.Count);
            Assert.Equal(10.5, med.GetMedian());
        }
    }
}
