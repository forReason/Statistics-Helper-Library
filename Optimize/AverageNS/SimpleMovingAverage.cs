using Statistics.Average_NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimize.AverageNS
{
    internal class SimpleMovingAverage
    {
        Random rng = new Random();
        private double Optimize()
        {
            double bestInput = 5;
            double bestInputDivergence = double.MaxValue;
            while ()
            {

            }
        }
        private double BenchmarkValueDivergence(double optimizeValue, double targetPrecision)
        {
            Progressing_Average_Double results = new Progressing_Average_Double();
            double oldDivergence = double.MaxValue;
            while (true)
            {
                results.AddValue(RunEpoch(10, optimizeValue));
                double epochChange = Math.Abs(1 - (oldDivergence / results.Value));
                oldDivergence = results.Value;
                double epochPrecision = Math.Round(epochChange * 100, 2);
                Console.Write($"\rbenchmarking value:{optimizeValue} Epoch precision:{epochPrecision}% Benchmark Result: {Math.Round(results.Value, 2)}");
                if (epochChange < targetPrecision)
                { // precision reached
                    Console.WriteLine($"final result for {optimizeValue}: {Math.Round(results.Value, 2)}");
                    break;
                }
            }
            return results.Value;
        }
        private double RunEpoch(int epochDuration, double optimizeValue)
        {
            Progressing_Average_Double results = new Progressing_Average_Double();
            uint numberSizeSteps = 20;
            uint max = int.MaxValue;
            uint stepSize = max / numberSizeSteps;
            for (int year = 0; year < epochDuration; year++)
            { // run one epoch of tests
                for (uint i = 0; i < max; i += stepSize)
                {
                    for (uint dataLength = 1; dataLength < 50000; dataLength += 97)
                    {
                        Simple_Moving_Average_Double simpleAverage = new Simple_Moving_Average_Double(dataLength);
                        Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
                        for (uint b = 0; b < dataLength; b++)
                        {
                            simpleAverage.AddPoint((rng.NextDouble() - 0.5) * i);
                        }
                        for (uint b = 0; b < dataLength; b++)
                        {
                            double dataPoint = (rng.NextDouble() - 0.5) * i;
                            simpleAverage.AddPoint(dataPoint);
                            progressingAverage.AddValue(dataPoint);
                        }
                        double divergence = Math.Abs(1 - (simpleAverage.Value / progressingAverage.Value));
                        if (double.IsNaN(divergence))
                        {
                            divergence = 0;
                        }
                        results.AddValue(divergence);
                    }
                }
            }
            return results.Value;
        }
    }
}
