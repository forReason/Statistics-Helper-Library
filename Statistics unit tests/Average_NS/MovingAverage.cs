using System;
using QuickStatistics.Net.Average_NS;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using System.IO;

namespace Statistics_unit_tests.Average_NS
{
    public class MovingAverage
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
        public void PositiveValues_LargeDataGaps()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = duration / 20;
            TimeSpan sleep = stepDuration / 20;
            int microStep = (int)(duration.TotalSeconds / sleep.TotalSeconds);
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");
            for (uint i = 0; i < max; i += stepSize)
            {
                timebasedAverage.Clear();
                double result1 = rng.NextDouble() * i;
                double result2 = rng.NextDouble() * i;
                double control = Math.Round((result1 + result2) / 2,6);

                timebasedAverage.AddValue(result1, baseTime);
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
                Assert.True(divergencePercent < 0.15);
            }
        }
        [Fact]
        public void PositiveValues_SmallDataGaps()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = duration;
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
                Assert.True(divergencePercent < 0.15);
            }
        }
        [Fact]
        public void StaticValues_SmallDataGaps()
        {
            MovingAverage_Double timebasedAverage = new MovingAverage_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
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
            Assert.Equal(control, Math.Round(timebasedAverage.Value, 6));
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
            Assert.True(averageAbsoluteDifference < 0.015, $"The average absolute difference was {averageAbsoluteDifference}, which is not less than 0.015.");
        }
    }
}
