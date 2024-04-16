namespace QuickStatistics.Net.Variance_NS;

public class StandardDeviation_Double
{
    public StandardDeviation_Double()
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

    public static double CalculateStandardDeviation(IEnumerable<double> input)
    {
        double M = 0;
        double S = 0;
        uint Count = 1;
        foreach (double value in input)
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

        if (Count <= 2)
        {
            return 0;
        }

        return Math.Sqrt(S / (Count - 2));
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
