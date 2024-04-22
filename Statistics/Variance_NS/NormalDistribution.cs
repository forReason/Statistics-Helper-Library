namespace QuickStatistics.Net.Variance_NS;

public class NormalDistribution
{
    /// <summary>
    /// The output of the function is the density at the point x
    /// </summary>
    /// <remarks>
    /// This density value is not a probability (as probabilities in continuous distributions are measured over an interval),
    /// but a density, meaning it describes the relative likelihood of the random variable falling near x
    /// </remarks>
    /// <param name="x">The data point at which you want to evaluate the density function.</param>
    /// <param name="mean">The mean (average) of the normal distribution.</param>
    /// <param name="stdDev">The standard deviation of the normal distribution.</param>
    /// <returns>the relative likelihood of the next random variable falling near x</returns>
    public static double ProbabilityDensityFunction(double x, double mean, double stdDev)
    {
        double expComponent = Math.Exp(-0.5 * Math.Pow((x - mean) / stdDev, 2));
        return (1 / (stdDev * Math.Sqrt(2 * Math.PI))) * expComponent;
    }
    
    /// <summary>
    /// Calculates an approximation of the probability that a single random data point, 
    /// drawn from a normal distribution defined by specified mean and standard deviation, 
    /// will fall within a given range [lower, upper].
    /// </summary>
    /// <param name="lower">The lower bound of the requested value range.</param>
    /// <param name="upper">The upper bound of the requested value range.</param>
    /// <param name="mean">The mean of the normal distribution.</param>
    /// <param name="stdDev">The standard deviation of the normal distribution.</param>
    /// <param name="steps">The number of intervals to divide the range [lower, upper] for numerical integration,
    /// affecting the accuracy of the approximation.
    /// More Steps lead to a more accurate result at the cost of computation time.</param>
    /// <returns>
    /// The estimated probability (0.0-1.0) that a random value from the distribution falls within the specified range.
    /// </returns>
    public static double ProbabilityFromRange(double lower, double upper, double mean, double stdDev, int steps = 10000)
    {
        double stepSize = (upper - lower) / steps;
        double sum = 0;
        double x = lower + stepSize / 2;  // Midpoint of each interval for better accuracy

        for (int i = 0; i < steps; i++)
        {
            sum += ProbabilityDensityFunction(x, mean, stdDev) * stepSize;  // f(x) * width
            x += stepSize;
        }

        return sum;
    }

    
    /// <summary>
    /// returns the expected value from a given sigma level
    /// </summary>
    /// <remarks>usual Sigma is -3 to +3</remarks>
    /// <param name="mean">the mean which to calculate the value for</param>
    /// <param name="stdDev">the standard deviation required for the calculation</param>
    /// <param name="sigmaLevel">the sigma Level</param>
    /// <returns></returns>
    public static double GetXForSigma(double mean, double stdDev, double sigmaLevel)
    {
        return mean + sigmaLevel * stdDev;
    }

}