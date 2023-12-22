using QuickStatistics.Net.Math_NS;
using System;
using System.Diagnostics;
using Xunit;

namespace Statistics_unit_tests.Math_NS;

public class DifferenceTests
{
    [Fact]
    public void TestByteDifference()
    {
        Random random = new Random();
        for (int i = 0; i < 10000; i++)
        {
            byte value1 = (byte)random.Next(0, 256); // Generate a random byte value (0-255)
            byte value2 = (byte)random.Next(0, 256); // Generate another random byte value (0-255)

            int diff = Math.Abs((int)value1 - (int)value2); // Calculate the difference as int
            byte expected = (byte)(diff <= byte.MaxValue ? diff : byte.MaxValue); // Ensure it's within byte range

            byte result = Difference.Get(value1, value2); // Actual result from the method

            Assert.Equal(expected, result); // Assert that the expected result matches the actual result
        }
    }

    [Fact]
    public void TestSpeedDifference()
    {
        Random random = new Random();
        
        Stopwatch mathAbsStopwatch = new Stopwatch();
        Stopwatch differenceStopwatch = new Stopwatch();
        mathAbsStopwatch.Start();
        for (int i = 0; i < 500000; i++)
        {
            byte value1 = (byte)random.Next(0, 256); // Generate a random byte value (0-255)
            byte value2 = (byte)random.Next(0, 256); // Generate another random byte value (0-255)
            
            byte diffMath = (byte)Math.Abs(value1 - value2); // Calculate the difference as int
            
        }
        mathAbsStopwatch.Stop();
        differenceStopwatch.Start();
        for (int i = 0; i < 500000; i++)
        {
            byte value1 = (byte)random.Next(0, 256); // Generate a random byte value (0-255)
            byte value2 = (byte)random.Next(0, 256); // Generate another random byte value (0-255)
            
            byte diffCustom = Difference.Get(value1, value2);
        }
        differenceStopwatch.Stop();
        Assert.True(differenceStopwatch.ElapsedMilliseconds /2  <= mathAbsStopwatch.ElapsedMilliseconds, $"{differenceStopwatch.Elapsed} vs {mathAbsStopwatch.Elapsed}");
    }


    // Additional tests for other numeric types...
}
