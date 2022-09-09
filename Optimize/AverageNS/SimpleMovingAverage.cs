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
        
        internal double Optimize(double startValue)
        {
            /// Configuration
            double targetPrecision = 0.001;
            ///Working variables
            // 0 = min value; 1 = current best testresult; 2 = max value
            BenchmarkResult[] optimizeRestrainBracket = new BenchmarkResult[3];
            double currentSearchValue = startValue;

            /// Optimisation init
            // generate firstBench
            Console.WriteLine("running initialization benchmark");
            optimizeRestrainBracket[1] = BenchmarkValueDivergence(startValue, targetPrecision);
            // start looking for lower bound
            Console.WriteLine("\nStart Looking for Minimum boundary");
            optimizeRestrainBracket[0] = optimizeRestrainBracket[1];
            while(true)
            {
                currentSearchValue /= 2;
                BenchmarkResult result = BenchmarkValueDivergence(currentSearchValue, targetPrecision);
                bool minFound = result.Score >= optimizeRestrainBracket[0].Score;
                optimizeRestrainBracket[0] = result;
                if (minFound)
                { // minimum bound was found ( optimizing for minimum loss)
                    Console.WriteLine("minimum boundary search complete!");
                    Console.WriteLine($"current boundaries: (" +
                        $"{optimizeRestrainBracket[0].Value} - {optimizeRestrainBracket[0].Score};" +
                        $"{optimizeRestrainBracket[1].Value} - {optimizeRestrainBracket[1].Score};" +
                        $"{optimizeRestrainBracket[2].Value} - {optimizeRestrainBracket[2].Score})");
                    break;
                }
                else
                { // maximum was found
                    Console.WriteLine("a better minimum was found. Pull down Max!");
                    Console.WriteLine($"current boundaries: (" +
                        $"{optimizeRestrainBracket[0].Value} - {optimizeRestrainBracket[0].Score};" +
                        $"{optimizeRestrainBracket[1].Value} - {optimizeRestrainBracket[1].Score};" +
                        $"{optimizeRestrainBracket[2].Value} - {optimizeRestrainBracket[2].Score})");
                    // move current middleground to maximum
                    optimizeRestrainBracket[2] = optimizeRestrainBracket[1];
                    // BenchResult is new middleground
                    optimizeRestrainBracket[1] = result;
                }
            }
            // start looking for upperbound
            if (optimizeRestrainBracket[2].Value <= optimizeRestrainBracket[1].Value)
            {
                currentSearchValue = optimizeRestrainBracket[1].Value;
                if (optimizeRestrainBracket[2].Value != null)
                {
                    currentSearchValue = optimizeRestrainBracket[2].Value;
                }
                while (true)
                {
                    currentSearchValue *= 2;
                    BenchmarkResult result = BenchmarkValueDivergence(currentSearchValue, targetPrecision);
                    bool maxFound = result.Score >= optimizeRestrainBracket[2].Score;
                    optimizeRestrainBracket[2] = result;
                    if (maxFound)
                    { // maximum bound was found ( can't find a better max )
                        break;
                    }
                    else
                    { // a better maximum was found, meaning the min can be trailed up
                      // move current middleground to minimum
                        optimizeRestrainBracket[0] = optimizeRestrainBracket[1];
                        // BenchResult is new middleground
                        optimizeRestrainBracket[1] = result;
                    }
                }
            }
            // min max bound should be found now. Narrowing the band
            while (true)
            {
                { }
            }

        }
        private BenchmarkResult BenchmarkValueDivergence(double optimizeValue, double targetPrecision)
        {
            Progressing_Average_Double results = new Progressing_Average_Double();
            double oldDivergence = double.MaxValue;
            int correctEpochs = 0;
            while (true)
            {
                double result = RunEpoch(100, optimizeValue);
                results.AddValue(result);
                double epochChange = Math.Abs(1 - (oldDivergence / results.Value));
                oldDivergence = results.Value;
                double epochPrecision = Math.Round(epochChange * 100, 2);
                Console.WriteLine($"\rbenchmarking value:{optimizeValue} accuracy:{100-epochPrecision}% Loss: {Math.Round(results.Value, 2)}");
                if (epochChange < targetPrecision)
                { // precision reached
                    correctEpochs ++;
                    if (correctEpochs >= 10)
                    {
                        Console.WriteLine($"final result for {optimizeValue}: {Math.Round(results.Value, 2)}");
                        break;
                    }
                }
                else
                {
                    correctEpochs = 0;
                }
            }
            return new BenchmarkResult(optimizeValue, results.Value);
        }
        private double RunEpoch(int epochDuration, double optimizeValue)
        {
            Progressing_Average_Double results = new Progressing_Average_Double();
            for (int i = 0; i < epochDuration; i++)
            {
                double progress = i / (double)epochDuration;
                Console.Write($"\rEpoch progress: {(progress * 100).ToString("0.00")}%");
                results.AddValue(SingleRun(optimizeValue, progress, epochDuration));
            }
            return results.Value;
        }
        private double SingleRun(double optimizeValue, double epochProgress, int epochSteps)
        {
            uint numberSizeSteps = 20;
            uint max = int.MaxValue;
            uint stepSize = max / numberSizeSteps;
            Progressing_Average_Double avg_divergence = new Progressing_Average_Double();
            //int threads = Environment.ProcessorCount;
            Task<Task<double>>[] runPackages = new Task<Task<double>>[numberSizeSteps];
            for (uint i = 0; i < numberSizeSteps; i ++)
            {
                runPackages[i] = Task.Factory.StartNew(() => MicroRun(optimizeValue, i));
            }
            var runTasks = Task.WhenAll(runPackages);
            runTasks.Wait();
            // merge runs
            foreach (Task<double> task in runTasks.Result)
            {
                avg_divergence.AddValue(task.Result);
            }
            if (double.IsNaN(avg_divergence.Value))
            {
                Console.WriteLine("ERROR!");
            }
            return avg_divergence.Value;
        }
        private async Task<double> MicroRun(double optimizeValue, uint stepSize)
        {
            Random rng = new Random();
            uint length = 50000;
            uint lengthSteps = length / 20;
            uint increase = lengthSteps;
            Simple_Moving_Average_Double simpleAverage = new Simple_Moving_Average_Double(length);
            Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
            Statistics.Variance_NS.StandardDeviation divergence_avg = new Statistics.Variance_NS.StandardDeviation();
            for (uint dataLength = 1; dataLength < length; dataLength += increase)
            {
                simpleAverage.Clear();
                progressingAverage.Clear();
                double value = 0;
                for (uint b = 0; b < dataLength*stepSize; b++)
                {
                    if (rng.NextDouble() > 0.5) value++;
                    else value--;
                    simpleAverage.AddPoint(value);
                }
                for (uint b = 0; b < dataLength; b++)
                {
                    if (rng.NextDouble() > 0.5) value++;
                    else value--;
                    simpleAverage.AddPoint(value);
                    progressingAverage.AddValue(value);
                }

                double absoluteDivergence = Math.Abs(simpleAverage.Value-progressingAverage.Value);
                //if ()
                //double percentualDivergence = (absoluteDivergence / Math.Abs(progressingAverage.Value))*100;
                //if (double.IsInfinity(percentualDivergence))
                //{
                //    Console.WriteLine("ERROR! result is infinity!");
                //    Console.WriteLine($"value: {value}");
                //    Console.WriteLine($"simpleAverage: {simpleAverage.Value}");
                //    Console.WriteLine($"benchmark: {progressingAverage.Value}");
                //    Console.WriteLine($"absolute divergence: {absoluteDivergence}");
                //    Console.WriteLine($"percentualDivergence: {percentualDivergence}");
                //
                //    Console.WriteLine("ERROR!!!");
                //}
                //if (double.IsNaN(percentualDivergence))
                //{
                //    percentualDivergence = 0;
                //}
                divergence_avg.AddValue(absoluteDivergence);
                
            }
            if (double.IsNaN(divergence_avg.Value))
            {
                Console.WriteLine("ERROR!");
            }
            return divergence_avg.Value;
        }
    }
}
