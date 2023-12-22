using System;
using QuickStatistics.Net.Average_NS;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using System.IO;
using System.Linq;
using QuickStatistics.Net.Math_NS;

namespace Statistics_unit_tests.Average_NS
{
    public class MovingAverageTests
    {
        [Fact]
        public void StaticPositiveValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue/2;
            uint stepSize = max / 20;
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = duration/ 20;
            TimeSpan sleep = stepDuration / 20;
            int microStep = (int)(duration.TotalSeconds / sleep.TotalSeconds);
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(duration,stepDuration);
            for (uint i = 0; i < max; i += stepSize)
            {
                timebasedAverage.Clear();
                double result = rng.NextDouble() * i;
                DateTime time = DateTime.Now;
                for (uint b = 0; b < microStep; b ++)
                {
                    time = time.Add(sleep);
                    timebasedAverage.AddValue(result,time);
                    //Task.Delay(sleep).Wait();
                    Assert.Equal(timebasedAverage.Value, result);
                }
            }
        }
        [Fact]
        public void StaticPositiveValues2()
        {
            // Initialize random number generator for controlled randomness
            Random rng = new Random(0); // Using a fixed seed for predictability
            uint max = int.MaxValue / 2;
            uint stepSize = max / 20;
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = TimeSpan.FromSeconds(duration.TotalSeconds / 20);
            int microStep = (int)(duration.TotalSeconds / stepDuration.TotalSeconds);

            MovingAverage_Double timebasedAverage = new MovingAverage_Double(duration, stepDuration);

            for (uint i = 0; i < max; i += stepSize)
            {
                timebasedAverage.Clear();
                double result = rng.NextDouble() * i;
                DateTime time = DateTime.UtcNow; // Using UtcNow for consistency

                for (int b = 0; b < microStep; b++)
                {
                    time = time.Add(stepDuration);
                    timebasedAverage.AddValue(result, time);

                    // Allow a small tolerance for floating point precision issues
                    double tolerance = 0.0001;
                    Assert.True(Math.Abs(timebasedAverage.Value - result) <= tolerance,
                                $"Expected: {result}, Actual: {timebasedAverage.Value}");
                }
            }
        }
        [Fact]
        public void AddOneValue()
        {
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = TimeSpan.FromSeconds(duration.TotalSeconds / 20);

            MovingAverage_Double timebasedAverage = new MovingAverage_Double(duration, stepDuration);
            timebasedAverage.AddValue(43879820.956968062, DateTime.Now);
            Assert.Equal(43879820.956968062, timebasedAverage.Value);
        }

        [Fact]
        public void PositiveValues_LargeDataGaps()
        {
            Random rng = new Random();
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = TimeSpan.FromSeconds(1); // Data point resolution
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(duration, stepDuration);
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");

            // Generate two random values
            double result1 = rng.NextDouble() * 1000; // Some large value
            double result2 = rng.NextDouble() * 1000; // Some large value

            // Add first value
            timebasedAverage.AddValue(result1, baseTime);

            // Add second value after a large gap (exceeds TotalTime)
            timebasedAverage.AddValue(result2, baseTime.AddSeconds(15));

            // The moving average should be close to result2 after the large gap
            double expectedValue = result2;
            double actualValue = timebasedAverage.Value;

            // Assert that the actual value is close to the expected value
            Assert.True(Math.Abs(actualValue - expectedValue) < 0.01, $"Expected value close to {expectedValue}, but got {actualValue}.");
        }
        [Fact]
        public void LargeDataGapWithInterpolationTest()
        {
            MovingAverage_Double timeBasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");

            // Adding initial value
            double initialValue = 100;
            timeBasedAverage.AddValue(initialValue, baseTime);

            // Simulating a large data gap
            DateTime newTime = baseTime.AddSeconds(20); // Large gap beyond TotalTime
            double newValue = 200;
            timeBasedAverage.AddValue(newValue, newTime, true); // With interpolation

            // Test to verify interpolated values
            double expectedAverage = 175; // Simplified expectation for the test
            Assert.Equal(expectedAverage, timeBasedAverage.GetCurrentMovingAverage(newTime.AddSeconds(1)), 2); // Precision of 2 decimal places
        }
        [Fact]
        public void SmallDataGapWithInterpolationTest()
        {
            MovingAverage_Double timeBasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");

            // Adding initial value
            double initialValue = 50;
            timeBasedAverage.AddValue(initialValue, baseTime);

            // Simulating a small data gap
            DateTime newTime = baseTime.AddSeconds(3); // Small gap within TotalTime
            double newValue = 70;
            timeBasedAverage.AddValue(newValue, newTime, true); // With interpolation

            // Test to verify interpolated values
            double expectedAverage = (initialValue + initialValue + ((initialValue + newValue) / 2) + newValue) / 4;

            double difference = Difference.Get(expectedAverage, timeBasedAverage.GetCurrentMovingAverage(newTime.AddSeconds(1)));
            double percetDifference = difference / expectedAverage;
            Assert.True(percetDifference <  0.05);
        }


        [Fact]
        public void PositiveValues_SmallDataGaps()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");
            for (uint i = 0; i < max; i += stepSize)
            {
                timebasedAverage.Clear();
                double result1 = rng.NextDouble() * i;
                double result2 = rng.NextDouble() * i;
                double control = Math.Round((result1 + result2) / 2, 6);

                timebasedAverage.AddValue(result1, baseTime); 
                timebasedAverage.AddValue(result1, baseTime.AddSeconds(4));
                timebasedAverage.AddValue(result2, baseTime.AddSeconds(5));
                timebasedAverage.AddValue(result2, baseTime.AddSeconds(6));
                timebasedAverage.AddValue(result2, baseTime.AddSeconds(7));
                timebasedAverage.AddValue(result2, baseTime.AddSeconds(8));
                timebasedAverage.AddValue(result2, baseTime.AddSeconds(9));
                double divergence = Math.Abs(timebasedAverage.Value - control);
                double divergencePercent = divergence / control;
                if (divergence == 0) divergencePercent = 0;
                if (divergencePercent > 0.15)
                {
                    { }
                }
                double divergencePercentAsPercentage = divergencePercent * 100;
                Assert.True(divergencePercent <= 0.15, $"Expected divergence percent to be less than 15%, but it was {Math.Round(divergencePercentAsPercentage)}%.");
            }
        }
        [Fact]
        public void StaticValues_SmallDataGaps()
        {
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");
            double result1 = 0;
            double result2 = 10;
            double control = Math.Round((result1 + result2) / 2, 6);

            timebasedAverage.AddValue(result1, baseTime);
            timebasedAverage.AddValue(result1, baseTime.AddSeconds(4));
            timebasedAverage.AddValue(result2, baseTime.AddSeconds(5));
            timebasedAverage.AddValue(result2, baseTime.AddSeconds(6));
            timebasedAverage.AddValue(result2, baseTime.AddSeconds(7));
            timebasedAverage.AddValue(result2, baseTime.AddSeconds(8));
            timebasedAverage.AddValue(result2, baseTime.AddSeconds(9));
            Assert.Equal(4.444444, Math.Round(timebasedAverage.Value, 6));
            Assert.Equal(5, Math.Round(timebasedAverage.GetCurrentMovingAverage(baseTime.AddSeconds(10)), 6));
        }
        [Fact]
        public void GenerateDataGap()
        {
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");
            double result1 = 0;
            double result2 = 10;
            double control = Math.Round((result1 + result2) / 2, 6);

            timebasedAverage.AddValue(result1, baseTime);
            timebasedAverage.AddValue(result1, baseTime.AddSeconds(4));
            Assert.Equal(result1, timebasedAverage.Value);
        }
        [Fact]
        public void ConstantValues()
        {
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime currentTime = DateTime.Now;

            for (int i = 0; i < 15; i++)
            {
                timebasedAverage.AddValue(5, currentTime);
                currentTime = currentTime.AddSeconds(1);
            }

            Assert.Equal(5, timebasedAverage.Value, 1); // Using precision of 1 due to potential floating point errors.
        }
        [Fact]
        public void LinearIncrease()
        {
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime currentTime = DateTime.Now;

            for (int i = 0; i < 20; i++)
            {
                timebasedAverage.AddValue(i, currentTime);
                currentTime = currentTime.AddSeconds(1);
            }

            Assert.True(timebasedAverage.Value > 10 && timebasedAverage.Value < 20);
        }
        [Fact]
        public void LinearDecrease()
        {
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime currentTime = DateTime.Now;

            for (int i = 20; i >= 0; i--)
            {
                timebasedAverage.AddValue(i, currentTime);
                currentTime = currentTime.AddSeconds(1);
            }

            Assert.True(timebasedAverage.Value > 0 && timebasedAverage.Value < 10);
        }
        [Fact]
        public void SineWaveValues()
        {
            int points = 120;
            TimeSpan totalTime = TimeSpan.FromSeconds(points);
            TimeSpan stepDuration = totalTime / points;

            MovingAverage_Double timebasedAverage = new MovingAverage_Double(totalTime, stepDuration);
            DateTime baseTime = DateTime.Now;

            double sumValues = 0;

            for (int i = 0; i < points; i++)
            {
                double value = Math.Sin(i * 2 * Math.PI / 60);
                sumValues += value;
                DateTime currentTime = baseTime + TimeSpan.FromSeconds(i);
                timebasedAverage.AddValue(value, currentTime);
            }

            double expectedValue = sumValues / points;
            Assert.Equal(expectedValue, timebasedAverage.Value, 2); // Using precision of 2 due to potential floating point errors.
        }

        [Fact]
        public void SineWaveSteppiness()
        {
            int points = 60;
            TimeSpan totalTime = TimeSpan.FromSeconds(points);
            int stepcount = 10;
            TimeSpan stepDuration = totalTime / (points/stepcount);

            MovingAverage_Double timebasedAverage = new MovingAverage_Double(totalTime, stepDuration);
            DateTime baseTime = DateTime.Now;

            double sumAbsoluteDifferences = 0;

            // Create a StringBuilder to store the CSV data.
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("Step,Sine Wave Value,Expected Average,Moving Average Value");

            for (int i = 0; i < points; i++)
            {
                double value = Math.Sin(i * 2 * Math.PI / points);
                DateTime currentTime = baseTime + TimeSpan.FromSeconds(i);
                timebasedAverage.AddValue(value, currentTime);

                double expectedValue = 0;
                //int windowSize = Math.Min(i + 1, points / 6);
                int windowSize = Math.Min(i + 1, points);

                for (int j = 0; j < windowSize; j++)
                {
                    expectedValue += Math.Sin((i - j) * 2 * Math.PI / points);
                }
                expectedValue /= windowSize;

                double actualValue = timebasedAverage.Value;

                // Add the values to the CSV data.
                csvData.AppendLine($"{i + 1},{value},{expectedValue:F3},{actualValue:F3}");

                sumAbsoluteDifferences += Math.Abs(expectedValue - actualValue);
            }

            // Save the CSV data to a file.
            string csvFilePath = "SineWaveSteppiness.csv";
            File.WriteAllText(csvFilePath, csvData.ToString());

            double averageAbsoluteDifference = sumAbsoluteDifferences / points;
            Assert.True(averageAbsoluteDifference < 0.1, $"The average absolute difference was {averageAbsoluteDifference}, which is not less than 0.015.");
        }
    }
}
