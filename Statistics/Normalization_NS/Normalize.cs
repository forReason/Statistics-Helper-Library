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
        // Extract integer (123.456 -> 123)
        decimal integerPart = Math.Floor(value);

        // 21 000 000 -> 1.0
        double normalizedInteger = (double)(integerPart / maxValue);

        // Normalize fractional part (0.xxxx)
        double normalizedFraction = (double)(value - integerPart);

        return (normalizedInteger, normalizedFraction);
    }
    /// <summary>
    /// Normalizes a DateTime value to the range [0, 1] based on an optional start date and an end date.
    /// </summary>
    /// <param name="value">The DateTime value to normalize.</param>
    /// <param name="startDate">The start DateTime for normalization. Defaults to Unix epoch (1970-01-01T00:00:00Z).</param>
    /// <param name="endDate">The end DateTime for normalization. Defaults to the year 4000.</param>
    /// <returns>
    /// A double representing the normalized DateTime value in the range [0, 1].
    /// </returns>
    /// <remarks>
    /// The function assumes that <paramref name="value"/> is within the range of [<paramref name="startDate"/>, <paramref name="endDate"/>].
    /// </remarks>
    public static double NormalizeDateTime(DateTime value, DateTime? startDate = null, DateTime? endDate = null)
    {
        // Default start date to Unix epoch (1970-01-01T00:00:00Z)
        DateTime start = startDate ?? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Default end date to the year 4000
        DateTime end = endDate ?? new DateTime(4000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        if (value < start || value > end)
            throw new ArgumentOutOfRangeException(nameof(value), "The DateTime value must be within the specified range.");

        // Calculate the total range in milliseconds
        double totalRange = (end - start).TotalMilliseconds;

        // Calculate the value's position in milliseconds from the start
        double valuePosition = (value - start).TotalMilliseconds;

        // Normalize to the range [0, 1]
        return valuePosition / totalRange;
    }

}
