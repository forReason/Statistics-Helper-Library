﻿using Statistics.Average_NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Statistics_unit_tests.Average_NS
{
    public class SimpleMovingAverage
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
                Simple_Moving_Average_Double progressingAverage = new Simple_Moving_Average_Double(10);
                double result = rng.NextDouble() * i;
                for (uint b = 0; b < max; b += stepSize)
                {
                    progressingAverage.AddPoint(result);
                    if (progressingAverage.Value != result)
                    {
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
        [Fact]
        public void CompareWithProgressingAverage()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                Simple_Moving_Average_Double simpleAverage = new Simple_Moving_Average_Double(1000);
                Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
                
                for (uint b = 0; b < 1000; b ++)
                {
                    double result = (rng.NextDouble()-0.5) * i;
                    progressingAverage.AddValue(result);
                    simpleAverage.AddPoint(result);
                    if (progressingAverage.Value != simpleAverage.Value)
                    {
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
        [Fact]
        public void MoveTowardsValue()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                for(uint dataLength = 1; dataLength < 50000; dataLength ++)
                {
                    Simple_Moving_Average_Double simpleAverage = new Simple_Moving_Average_Double(1000);
                    double startValue = (rng.NextDouble() - 0.5) * i;
                    double targetValue = (rng.NextDouble() - 0.5) * i;

                    for (uint b = 0; b < dataLength; b++)
                    {
                        simpleAverage.AddPoint(startValue);
                    }
                    for (uint b = 0; b < dataLength; b++)
                    {
                        simpleAverage.AddPoint(targetValue);
                    }
                    double divergence = Math.Abs(1 - (simpleAverage.Value / targetValue));
                    if (divergence != double.NaN && (divergence > 0.01))
                    {
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
    }
}