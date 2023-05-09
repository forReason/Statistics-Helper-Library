using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickStatistics.Net.Average_NS;
using Xunit;

namespace Statistics_unit_tests.Average_NS
{
    public class ProgressingAverage
    {
        [Fact]
        public void StaticPositiveValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
                double result = rng.NextDouble() * i;
                for (uint b = 0; b < max; b += stepSize)
                {
                    progressingAverage.AddValue(result);
                }
                if (progressingAverage.Value != result)
                {
                    throw new Exception("Value does not add up!");
                }
            }
        }
        [Fact]
        public void StaticNegativeValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
                double result = rng.NextDouble() * i;
                result = -result;
                for (uint b = 0; b < max; b += stepSize)
                {
                    progressingAverage.AddValue(result);
                }
                if (progressingAverage.Value != result)
                {
                    throw new Exception("Value does not add up!");
                }
            }
        }
        [Fact]
        public void PositiveValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue / 50;
            uint stepSize = max / 50;
            for (uint i = 50; i < max; i += stepSize)
            {
                Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
                uint result = 0;
                uint steps = 0;
                uint stepsizeb = i / 50;
                for (uint b = 0; b < i; b += stepSize)
                {
                    result += b;
                    steps++;
                    progressingAverage.AddValue(b);
                }
                double endResult = result / (double)steps;
                if (progressingAverage.Value != endResult)
                {
                    throw new Exception("Value does not add up!");
                }
            }
        }
        /// <summary>
        /// tests a generating the average from a range of random doubles (-0.5 to 0.5)
        /// </summary>
        /// <exception cref="Exception"></exception>
        [Fact]
        public void RandomValue()
        {
            // positive tests
            Random rng = new Random();
            Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
            double result = 0;
            uint steps = 0;
            for (uint b = 0; b < 2000; b ++)
            {
                double random = rng.NextDouble() - 0.5;
                result += random;
                steps++;
                progressingAverage.AddValue(random);
            }
            double endResult = result / (double)steps;
            if (Math.Round(progressingAverage.Value,6) != Math.Round(endResult,6))
            {
                throw new Exception("Value does not add up!");
            }
        }
    }
}