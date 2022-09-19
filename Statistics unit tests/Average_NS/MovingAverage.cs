using System;
using Statistics.Average_NS;
using System.Threading.Tasks;
using Xunit;

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
            Moving_Average_Double timebasedAverage = new Moving_Average_Double(duration,stepDuration);
            for (uint i = 0; i < max; i += stepSize)
            {
                timebasedAverage.Clear();
                double result = rng.NextDouble() * i;
                for (uint b = 0; b < microStep; b ++)
                {
                    timebasedAverage.AddValue(result);
                    Task.Delay(sleep).Wait();
                    if (timebasedAverage.Value != result)
                    { // proof result
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
        [Fact]
        public void PositiveValues_DataGaps()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = duration / 20;
            TimeSpan sleep = stepDuration / 20;
            int microStep = (int)(duration.TotalSeconds / sleep.TotalSeconds);
            Moving_Average_Double timebasedAverage = new Moving_Average_Double(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
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
                if (Math.Round(timebasedAverage.Value,6) != control)
                { // proof result
                    throw new Exception("Value does not add up!");
                }
            }
        }
        [Fact]
        public void RandomValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            TimeSpan duration = TimeSpan.FromSeconds(10);
            TimeSpan stepDuration = duration / 20;
            double stepAmount = duration / stepDuration;
            Moving_Average_Double timebasedAverage = new Moving_Average_Double(duration, stepDuration);
            Progressing_Average_Double controlAverage = new Progressing_Average_Double();
            DateTime baseTime = DateTime.Parse("2022/08/09 13:22:00");
            for (uint i = 0; i < max; i += stepSize)
            {
                // clear
                timebasedAverage.Clear();
                controlAverage.Clear();
                for(int b = 0; b < stepAmount; b++)
                {
                    double rand = (rng.NextDouble() - 0.5) * i;
                    DateTime targetTime = baseTime + (b * (stepDuration + TimeSpan.FromMilliseconds(1)));
                    timebasedAverage.AddValue(rand, targetTime);
                    controlAverage.AddValue(rand);
                    double divergence = Math.Abs(timebasedAverage.Value - controlAverage.Value);
                    double divergencePercent = divergence / controlAverage.Value;
                    if (divergencePercent > 0.001)
                    { // proof result
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
    }
}
