using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Variance_NS
{
    public class StandardDeviation
    {
        public StandardDeviation()
        {
            Clear();
        }
        private double M { get; set; }
        private double S { get; set; }
        private uint Iteration { get; set; }
        public double Value
        {
            get
            {
                return Math.Sqrt(S / (Iteration - 2));
            }
        }
        public void AddValue(double value)
        {
            if (value != 0 && !double.IsNormal(value))
            {
                throw new ArgumentException($"input {value} is abnormal!");
            }
            if (double.IsNaN(value))
            {
                throw new ArgumentException("value IS nan!");
            }
            double tmpM = M;
            M += (value - tmpM) / Iteration;
            S += (value - tmpM) * (value - M);
            Iteration++;
            if (double.IsNaN(M))
            {
                Console.WriteLine($"\nNAN EXCEPTION!!! divide by: {Iteration}");
                Console.WriteLine($"\nNAN EXCEPTION!!! tmpM: {tmpM}");
                throw new ArgumentException("m IS nan!");
            }
        }
        public void Clear()
        {
            M = 0.0;
            S = 0.0;
            Iteration = 1;
        }
    }
}
