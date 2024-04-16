using System.Numerics;

namespace QuickStatistics.Net.Variance_NS;
#if NET7_0_OR_GREATER
public class StandardDeviation<T> where T : INumber<T>
{
    public StandardDeviation()
    {
        Clear();
    }

    private StandardDeviation_Double std = new StandardDeviation_Double();
    public uint Count => std.Count;

    public double Value => std.Value;

    public void AddValue(T value)
    {
        std.AddValue(Convert.ToDouble(value));
    }

    public static double CalculateStandardDeviation<T>(IEnumerable<T> input) where T : INumber<T>
    {
        double M = 0;
        double S = 0;
        uint Count = 1;
        foreach (T value in input)
        {
            double convertedValue = Convert.ToDouble(value);
            if (double.IsNaN(convertedValue) || double.IsInfinity(convertedValue))
            {
                throw new ArgumentException("value IS nan!");
            }

            double tmpM = M;
            M += (convertedValue - tmpM) / Count;
            S += (convertedValue - tmpM) * (convertedValue - M);
            Count++;
            if (double.IsNaN(M))
            {
                Console.WriteLine($"\nNAN EXCEPTION!!! divide by: {Count}");
                Console.WriteLine($"\nNAN EXCEPTION!!! tmpM: {tmpM}");
                throw new ArgumentException("m IS nan!");
            }
        }

        if (Count <= 2)
        {
            return 0;
        }

        return Math.Sqrt(S / (Count - 2));
    }

    public void Clear()
    {
        std.Clear();
    }

    public override string ToString()
    {
        return this.Value.ToString();
    }
}
#endif
