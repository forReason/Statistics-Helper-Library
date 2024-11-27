namespace QuickStatistics.Net.Normalization_NS;

/// <summary>
/// Provides functionality to normalize decimal values into separate components for neural network processing or other applications.
/// </summary>
public static class Normalize
{
    /// <summary>
    /// Normalizes a decimal value into two components: 
    /// an integer part normalized to the range [0, 1] based on a specified maximum value, 
    /// and a fractional part normalized to [0, 1] within the same unit.
    /// </summary>
    /// <param name="value">The decimal value to normalize.</param>
    /// <param name="maxValue">The maximum value used for normalizing the integer part.</param>
    /// <returns>
    /// A tuple containing:
    /// - <c>normalizedInteger</c>: A double representing the integer part of the value, normalized to the range [0, 1].
    /// - <c>normalizedFraction</c>: A double representing the fractional part of the value, scaled to the range [0, 1].
    /// </returns>
    /// <remarks>
    /// The function assumes that <paramref name="value"/> is non-negative and less than or equal to <paramref name="maxValue"/>.
    /// The fractional part retains the precision of the input decimal, making it suitable for high-precision tasks.
    /// </remarks>
    public static (double normalizedInteger, double normalizedFraction) NormalizeDecimal(decimal value, decimal maxValue)
    {
        // Extract integer and fractional parts
        decimal integerPart = Math.Floor(value);
        decimal fractionalPart = value - integerPart;

        // Normalize integer part to the range [0, 1]
        double normalizedInteger = (double)(integerPart / maxValue);

        // Normalize fractional part (scaled to 0 to 1 within a single unit)
        double normalizedFraction = (double)fractionalPart;

        return (normalizedInteger, normalizedFraction);
    }
}
