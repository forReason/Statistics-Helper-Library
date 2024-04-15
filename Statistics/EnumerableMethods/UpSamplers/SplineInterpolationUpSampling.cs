
namespace QuickStatistics.Net.EnumerableMethods.UpSamplers;

public static partial class UpSampler
{
    /// <summary>
    /// Up-samples an array to a larger array using spline interpolation.
    /// </summary>
    /// <param name="source">The array to up-sample.</param>
    /// <param name="targetLength">The desired target length.</param>
    /// <returns>An up-sampled array using spline interpolation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the target length is invalid.</exception>
    public static double[] UpSampleSplineInterpolation(IEnumerable<double> source, int targetLength)
    {
        // Precondition checks
        if (targetLength < 1)
            throw new ArgumentOutOfRangeException(nameof(targetLength), "Target length must be greater than 1!");
        IList<double> sourceArray = source as IList<double> ?? source.ToArray();
        int sourceLength = sourceArray.Count;
        if (sourceLength == targetLength)
            return sourceArray.ToArray();
        if (sourceLength >= targetLength)
            throw new ArgumentOutOfRangeException(nameof(targetLength),
                "Target length must be greater than the source length.");

        double[] result = new double[targetLength];
        // Calculate the step size between each sample in the target array
        double stepSize = (double)(sourceLength - 1) / (targetLength - 1);

        // Generate the x and y coordinates for the spline interpolation
        double[] x = Enumerable.Range(0, sourceLength).Select(i => (double)i).ToArray();
        double[] y = sourceArray.ToArray();

        // Calculate the spline coefficients
        double[] coefficients = CubicSplineCoefficients(x, y);

        // Interpolate the values for the up-sampled array
        for (int targetIndex = 0; targetIndex < targetLength; targetIndex++)
        {
            double sourceIndex = targetIndex * stepSize;
            int lowerIndex = (int)Math.Floor(sourceIndex);
            int upperIndex = (int)Math.Ceiling(sourceIndex);

            if (lowerIndex == upperIndex)
            {
                result[targetIndex] = sourceArray[lowerIndex];
            }
            else
            {
                // Perform spline interpolation
                double interpolatedValue = CubicSplineInterpolation(x, y, coefficients, sourceIndex);
                result[targetIndex] = interpolatedValue;
            }
        }

        return result;
    }

    // Helper method to calculate the cubic spline coefficients
    private static double[] CubicSplineCoefficients(double[] x, double[] y)
    {
        int n = x.Length;
        double[] h = new double[n - 1];
        double[] alpha = new double[n - 1];
        double[] l = new double[n];
        double[] mu = new double[n];
        double[] z = new double[n];
        double[] c = new double[n];
        double[] b = new double[n - 1];
        double[] d = new double[n - 1];
        double[] coefficients = new double[3 * (n - 1)];

        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
            alpha[i] = 3 * (y[i + 1] - y[i]) / h[i];
        }

        l[0] = 1;
        mu[0] = 0;
        z[0] = 0;

        for (int i = 1; i < n - 1; i++)
        {
            l[i] = 2 * (x[i + 1] - x[i - 1]) - h[i - 1] * mu[i - 1];
            mu[i] = h[i] / l[i];
            z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
        }

        l[n - 1] = 1;
        z[n - 1] = 0;
        c[n - 1] = 0;

        for (int j = n - 2; j >= 0; j--)
        {
            c[j] = z[j] - mu[j] * c[j + 1];
            b[j] = (y[j + 1] - y[j]) / h[j] - h[j] * (c[j + 1] + 2 * c[j]) / 3;
            d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
        }

        for (int i = 0; i < n - 1; i++)
        {
            coefficients[3 * i] = y[i];
            coefficients[3 * i + 1] = b[i];
            coefficients[3 * i + 2] = c[i];
        }

        return coefficients;
    }

    // Helper method to perform cubic spline interpolation
    private static double CubicSplineInterpolation(double[] x, double[] y, double[] coefficients, double xi)
    {
        int n = x.Length;

        int j = Array.BinarySearch(x, xi);
        if (j < 0)
        {
            j = ~j - 1;
        }

        j = Math.Max(0, Math.Min(n - 2, j));

        double h = x[j + 1] - x[j];
        double t = (xi - x[j]) / h;

        double a = coefficients[3 * j + 2] * h - (y[j + 1] - y[j]);
        double b = (j + 1 < n - 1) ? -coefficients[3 * (j + 1) + 1] * h + (y[j + 1] - y[j]) : 0;

        double interpolatedValue = (1 - t) * y[j] + t * y[j + 1] + t * (1 - t) * (a * (1 - t) + b * t);
        return interpolatedValue;
    }


}