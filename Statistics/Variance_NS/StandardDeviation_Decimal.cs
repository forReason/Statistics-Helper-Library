using QuickStatistics.Net.Math_NS;

namespace QuickStatistics.Net.Variance_NS
{
    public class StandardDeviation_Decimal
    {
        public StandardDeviation_Decimal()
        {
            Clear();
        }
        private decimal M { get; set; }
        private decimal S { get; set; }
        public uint Count { get; private set; }
        public decimal Value
        {
            get
            {
                if (Count <= 2)
                {
                    return 0;
                }
                return DecimalMath.Sqrt(S / (Count - 2));
            }
        }
        public void AddValue(decimal value)
        {
            decimal tmpM = M;
            M += (value - tmpM) / Count;
            S += (value - tmpM) * (value - M);
            Count++;
        }

        public static decimal CalculateStandardDeviation(IEnumerable<decimal> input)
        {
            decimal M = 0;
            decimal S = 0;
            uint Count = 1;
            foreach (decimal value in input)
            {
                decimal tmpM = M;
                M += (value - tmpM) / Count;
                S += (value - tmpM) * (value - M);
                Count++;
            }
            if (Count <= 2)
            {
                return 0;
            }
            return DecimalMath.Sqrt(S / (Count - 2));
        }
        public void Clear()
        {
            M = 0.0m;
            S = 0.0m;
            Count = 1;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
