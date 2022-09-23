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
        public uint Count { get; private set; }
        public double Value
        {
            get
            {
                if (Count <= 2)
                {
                    return 0;
                }
                return Math.Sqrt(S / (Count - 2));
            }
        }
        public void AddValue(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("value IS nan!");
            }
            double tmpM = M;
            M += (value - tmpM) / Count;
            S += (value - tmpM) * (value - M);
            Count++;
            if (double.IsNaN(M))
            {
                Console.WriteLine($"\nNAN EXCEPTION!!! divide by: {Count}");
                Console.WriteLine($"\nNAN EXCEPTION!!! tmpM: {tmpM}");
                throw new ArgumentException("m IS nan!");
            }
        }
        public void Clear()
        {
            M = 0.0;
            S = 0.0;
            Count = 1;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
