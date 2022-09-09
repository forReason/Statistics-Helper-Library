using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimize.AverageNS
{
    public struct BenchmarkResult
    {
        public BenchmarkResult(double value, double score)
        {
            Value = value;
            Score = score;
        }
        public double Value { get; private set; }
        public double Score { get; private set; }
    }
}
