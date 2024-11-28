namespace QuickStatistics.Net.AiHelpers;

/// <summary>
/// Provides functionality to normalize decimal values into separate components for neural network processing or other applications.
/// </summary>
public static partial class Normalize
{
    /// <summary>
    /// Normalizes a DateTime value to a double array for high precision.
    /// </summary>
    /// <param name="value">The DateTime value to normalize.</param>
    /// <returns>A double array containing the high and low normalized parts of the ticks.</returns>
    public static double[] NormalizeToDouble(DateTime value)
    {
        // Ensure UTC for consistent ticks
        value = value.ToUniversalTime();
        long ticks = value.Ticks;

        // Split ticks into high and low parts
        long high = ticks >> 32; // Most significant 32 bits
        long low = ticks & 0xFFFFFFFF; // Least significant 32 bits

        // Normalize both parts separately
        double normalizedHigh = (double)high / int.MaxValue;
        double normalizedLow = (double)low / uint.MaxValue;

        return new[] { normalizedHigh, normalizedLow };
    }
    /// <summary>
    /// Normalizes a DateTime value to a float[4] array for maximum precision using smaller components.
    /// </summary>
    /// <param name="value">The DateTime value to normalize.</param>
    /// <returns>A float[4] array containing the split normalized parts of the ticks.</returns>
    public static float[] NormalizeToFloat(DateTime value)
    {
        // Ensure UTC for consistent ticks
        value = value.ToUniversalTime();
        long ticks = value.Ticks;

        // Split ticks into 4 parts (16-bit segments)
        ushort part1 = (ushort)((ticks >> 48) & 0xFFFF); // Most significant 16 bits
        ushort part2 = (ushort)((ticks >> 32) & 0xFFFF); // Next 16 bits
        ushort part3 = (ushort)((ticks >> 16) & 0xFFFF); // Next 16 bits
        ushort part4 = (ushort)(ticks & 0xFFFF);         // Least significant 16 bits

        // Normalize each part to [0, 1]
        float normalizedPart1 = (float)part1 / ushort.MaxValue;
        float normalizedPart2 = (float)part2 / ushort.MaxValue;
        float normalizedPart3 = (float)part3 / ushort.MaxValue;
        float normalizedPart4 = (float)part4 / ushort.MaxValue;

        return new[] { normalizedPart1, normalizedPart2, normalizedPart3, normalizedPart4 };
    }

    
    /// <summary>
    /// Denormalizes a float[4] array back into a DateTime value with high precision.
    /// </summary>
    /// <param name="normalizedArray">The float[4] array containing the split normalized parts of the ticks.</param>
    /// <returns>The denormalized DateTime value.</returns>
    public static DateTime DenormalizeDateTime(float[] normalizedArray)
    {
        if (normalizedArray == null || normalizedArray.Length != 4)
            throw new ArgumentException("Array must have exactly four elements.");

        // Convert each normalized part back to its 16-bit integer representation
        ushort part1 = (ushort)(normalizedArray[0] * ushort.MaxValue);
        ushort part2 = (ushort)(normalizedArray[1] * ushort.MaxValue);
        ushort part3 = (ushort)(normalizedArray[2] * ushort.MaxValue);
        ushort part4 = (ushort)(normalizedArray[3] * ushort.MaxValue);

        // Recombine the parts into the original 64-bit ticks value
        long ticks = ((long)part1 << 48) | ((long)part2 << 32) | ((long)part3 << 16) | part4;

        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Denormalizes a double array back into a DateTime value with high precision.
    /// </summary>
    /// <param name="normalizedArray">The double array containing the high and low normalized parts.</param>
    /// <returns>The denormalized DateTime value.</returns>
    public static DateTime DenormalizeDateTime(double[] normalizedArray)
    {
        if (normalizedArray == null || normalizedArray.Length != 2)
            throw new ArgumentException("Array must have exactly two elements.");

        double normalizedHigh = normalizedArray[0];
        double normalizedLow = normalizedArray[1];

        // Convert normalized values back to high and low parts
        long high = (long)(normalizedHigh * int.MaxValue);
        long low = (long)(normalizedLow * uint.MaxValue);

        // Combine high and low parts into ticks
        long ticks = (high << 32) | (low & 0xFFFFFFFF);

        return new DateTime(ticks, DateTimeKind.Utc);
    }

}
public static class DateTimeExtensions
{
    /// <summary>
    /// Normalizes the DateTime value to a double array for high precision.
    /// </summary>
    /// <param name="value">The DateTime value to normalize.</param>
    /// <returns>A double array containing the high and low normalized parts of the ticks.</returns>
    public static double[] ToNormalizedDoubleArray(this DateTime value)
    {
        return Normalize.NormalizeToDouble(value);
    }

    /// <summary>
    /// Normalizes the DateTime value to a float array for maximum precision using smaller components.
    /// </summary>
    /// <param name="value">The DateTime value to normalize.</param>
    /// <returns>A float array containing the split normalized parts of the ticks.</returns>
    public static float[] ToNormalizedFloatArray(this DateTime value)
    {
        return Normalize.NormalizeToFloat(value);
    }

    /// <summary>
    /// Converts a normalized double array back into a DateTime.
    /// </summary>
    /// <param name="normalizedArray">The double array containing the normalized parts.</param>
    /// <returns>The denormalized DateTime value.</returns>
    public static DateTime DateTimeFromNormalized(this double[] normalizedArray)
    {
        return Normalize.DenormalizeDateTime(normalizedArray);
    }

    /// <summary>
    /// Converts a normalized float array back into a DateTime.
    /// </summary>
    /// <param name="normalizedArray">The float array containing the normalized parts.</param>
    /// <returns>The denormalized DateTime value.</returns>
    public static DateTime DateTimeFromNormalized(this float[] normalizedArray)
    {
        return Normalize.DenormalizeDateTime(normalizedArray);
    }
}

