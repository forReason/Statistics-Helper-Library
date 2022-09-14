using Statistics.Average_NS;
using Statistics.MinMax_NS;
using Statistics.Variance_NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimize.AverageNS
{
    internal class SimpleMovingAverage
    {
        public SimpleMovingAverage()
        {
            Console.WriteLine("parsing training data into memory");
            DirectoryInfo trainingData = new DirectoryInfo("Training_Data");
            List<double> output = new List<double>();
            foreach (FileInfo csvTable in trainingData.GetFiles())
            {
                CSV_Helper_Project.Table table = new CSV_Helper_Project.Table();
                table.LoadFromFile(csvTable.FullName,hasHeaders:true,delimiter:',');
                foreach (string col in table.GetHeaders())
                {
                    string[] values = table.GetColumnOfAllRecords(col);
                    foreach (string cell in values)
                    {
                        if (cell == "NA" || cell == "") continue;
                        try
                        {
                            output.Add(double.Parse(cell));
                        }
                        catch
                        {
                            { }
                        }
                    }
                    TestFiles.Add(output.ToArray());
                    output.Clear();
                }
            }
            { }
        }
        List<double[]> TestFiles = new List<double[]>();

        internal double Optimize(double startValue)
        {
            /// Configuration
            double targetPrecision = 0.005;
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
            Sliding_Maximum max = new Sliding_Maximum(10);
            Sliding_Minimum min = new Sliding_Minimum(10);
            double precision = double.MaxValue;
            Console.WriteLine($"starting benchmark for value {optimizeValue} with target divergence < {targetPrecision*100}%");
            max.AddPoint(double.PositiveInfinity);
            // start real data benchmark
            Task<Task<double>> benchResults = Task.Factory.StartNew(() => BenchSavedFiles(optimizeValue));
            Task.WaitAll(benchResults);
            double realDataResults = benchResults.Result.Result;
            // start random data simulation
            while (precision > targetPrecision)
            {
                double result = RunEpoch(64, optimizeValue);
                results.AddValue(result);
                // calculate early stopping
                min.AddPoint(results.Value);
                max.AddPoint(results.Value);
                precision = ((max.Value - min.Value) / Math.Abs(results.Value));
                Console.WriteLine($"\rbenchmarking value:{optimizeValue} divergence:{((precision)*100).ToString("0.00")}% ({(max.Value-min.Value).ToString("0.00")}) Loss: {results.Value.ToString("0.00")}");
            }
            Console.WriteLine($"final result for {optimizeValue}: {Math.Round(results.Value, 2)}");
            double mergedResult = Volumetric_Average.VolumeBasedAverage(value1: results.Value, volume1: 0.3, value2: realDataResults, volume2: 0.7);
            return new BenchmarkResult(optimizeValue, mergedResult);
        }
        
        private async Task<double> BenchSavedFiles(double optimizeValue)
        {
            uint numberSizeSteps = 20;
            Simple_Moving_Average_Double simpleAverage = new Simple_Moving_Average_Double(numberSizeSteps, optimizeValue);
            Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
            StandardDeviation divergence_sdv = new StandardDeviation();
            Progressing_Average_Double divergence_avg = new Progressing_Average_Double();
            foreach(double[] file in TestFiles)
            { // go trough each test file
                uint fileSizeDivider = (uint)(file.Length / numberSizeSteps); // a lot of 20 is this long
                for (int stepDivider = 1; stepDivider <numberSizeSteps && stepDivider < fileSizeDivider/2; stepDivider++)
                { // smaller substeps
                    uint dataLength = (uint)(fileSizeDivider / stepDivider);
                    simpleAverage.MaxDataLength = dataLength;
                    simpleAverage.Clear();
                    for (int i = 0; i < file.Length/dataLength; i++)
                    { // check the different parts of the file
                        progressingAverage.Clear();
                        int baseIndex =(int)(i * dataLength);
                        for (int stepIndex = 0; stepIndex < dataLength; stepIndex++)
                        {
                            simpleAverage.AddPoint(file[baseIndex+stepIndex]);
                            progressingAverage.AddValue(file[baseIndex + stepIndex]);
                        }
                        // get metric
                        double absoluteDivergence = Math.Abs(simpleAverage.Value - progressingAverage.Value);
                        double divergenceFactor = (absoluteDivergence / (Math.Abs(progressingAverage.Value) + 1)) * 100;
                        divergence_avg.AddValue(divergenceFactor);
                        divergence_sdv.AddValue(divergenceFactor);
                        if (divergenceFactor > 300)
                        {
                            { }
                        }
                    }
                }
            }
            double loss = divergence_avg.Value * divergence_sdv.Value;
            return loss;
        }
        private double RunEpoch(int epochDuration, double optimizeValue)
        {
            int threadLimit = Environment.ProcessorCount;
            Progressing_Average_Double results = new Progressing_Average_Double();
            List<Task<Task<double>>> benchResults = new List<Task<Task<double>>>();
            for (int i = 0; i < threadLimit; i++)
            {
                benchResults.Add(Task.Factory.StartNew(() => SingleRun(optimizeValue)));
            }
            while (results.Count < epochDuration)
            {
                int index = Task.WaitAny(benchResults.ToArray());
                results.AddValue(benchResults[index].Result.Result);
                benchResults.RemoveAt(index);
                int queue = results.Count + benchResults.Count;
                if (queue < epochDuration)
                {
                    benchResults.Add(Task.Factory.StartNew(() => SingleRun(optimizeValue)));
                }
                double progress = ((double)results.Count/epochDuration) *100;
                Console.Write($"\rEpoch progress: {progress.ToString("00.0")} %");
            }
            return results.Value;
        }
        private async Task<double> SingleRun(double optimizeValue)
        {
            Random rng = new Random();
            uint iterations = 250;
            uint lengthSteps = 15;
            // generate simple average queues
            List<Simple_Moving_Average_Double> simpleAverages = new List<Simple_Moving_Average_Double>();
            List<StandardDeviation> divergence_sdv = new List<StandardDeviation>();
            List<Progressing_Average_Double> divergence_avg = new List<Progressing_Average_Double>();
            for(int i = 2; i <= lengthSteps; i++)
            {
                simpleAverages.Add(new Simple_Moving_Average_Double((uint)Math.Pow(i, 4),optimizeValue));
                divergence_sdv.Add(new StandardDeviation());
                divergence_avg.Add(new Progressing_Average_Double());
            }
            Progressing_Average_Double comparisonBenchmark = new Progressing_Average_Double();
            double value = 0;
            for (int step = 0; step < iterations; step++)
            { // generate test data
                comparisonBenchmark.Clear();
                for (uint stepIteration = 1; stepIteration <= simpleAverages[simpleAverages.Count-1].MaxDataLength; stepIteration++)
                {
                    if (rng.NextDouble() > 0.5) value++;
                    else value--;
                    comparisonBenchmark.AddValue(value);
                    for(int avgIndex = 0; avgIndex < simpleAverages.Count; avgIndex++)
                    {
                        simpleAverages[avgIndex].AddPoint(value);
                        if (stepIteration == simpleAverages[avgIndex].MaxDataLength)
                        {
                            double absoluteDivergence = Math.Abs(simpleAverages[avgIndex].Value - comparisonBenchmark.Value);
                            double divergenceFactor = (absoluteDivergence / (Math.Abs(comparisonBenchmark.Value) + 1)) * 100;
                            divergence_avg[avgIndex].AddValue(divergenceFactor);
                            divergence_sdv[avgIndex].AddValue(divergenceFactor);
                        }
                    }
                }
            }
            Progressing_Average_Double calculationResult = new Progressing_Average_Double();
            for (int avgIndex = 0; avgIndex < simpleAverages.Count; avgIndex++)
            {
                double checkvalve = divergence_avg[avgIndex].Value * divergence_sdv[avgIndex].Value;
                calculationResult.AddValue(checkvalve);
            }
            if (double.IsNaN(calculationResult.Value))
            {
                Console.WriteLine("ERROR!");
            }
            return calculationResult.Value;
        }
    }
}
