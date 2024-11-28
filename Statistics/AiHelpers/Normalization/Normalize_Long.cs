namespace QuickStatistics.Net.AiHelpers;

public static partial class Normalize
{
    /// <summary>
    /// Normalizes a 64-bit integer into a float[4] array without losing precision.
    /// </summary>
    public static float[] NormalizeToFloat(long value)
    {
        ulong unsignedValue = (ulong)value;
        byte[] bytes = BitConverter.GetBytes(unsignedValue);

        float[] normalizedArray = new float[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            normalizedArray[i] = bytes[i] / 255f;
        }
        return normalizedArray;
    }


    /// <summary>
    /// Denormalizes a float[4] array back into a 64-bit integer.
    /// </summary>
    public static long DenormalizeLong(float[] normalizedArray)
    {
        if (normalizedArray == null || normalizedArray.Length != 8)
            throw new ArgumentException("Array must have exactly eight elements.");

        byte[] bytes = new byte[8];
        for (int i = 0; i < 8; i++)
        {
            if (normalizedArray[i] < 0.0f || normalizedArray[i] > 1.0f)
                throw new ArgumentOutOfRangeException(nameof(normalizedArray), "All elements must be in the range [0, 1].");

            bytes[i] = (byte)(normalizedArray[i] * 255f + 0.5f); // Add 0.5f for rounding
        }

        ulong unsignedValue = BitConverter.ToUInt64(bytes, 0);
        return (long)unsignedValue;
    }
    
    /// <summary>
    /// Normalizes a 64-bit integer into a double in the range [0, 1].
    /// </summary>
    public static double[] NormalizeToDouble(long value)
    {
        ulong unsignedValue = (ulong)value; // Treat as unsigned for bit manipulation
        uint high = (uint)(unsignedValue >> 32); // Most significant 32 bits
        uint low = (uint)unsignedValue; // Least significant 32 bits

        double normalizedHigh = high / (double)uint.MaxValue;
        double normalizedLow = low / (double)uint.MaxValue;

        return new[] { normalizedHigh, normalizedLow };
    }



    /// <summary>
    /// Denormalizes a double back into a 64-bit integer.
    /// </summary>
    public static long DenormalizeLong(double[] normalizedArray)
    {
        if (normalizedArray == null || normalizedArray.Length != 2)
            throw new ArgumentException("Array must have exactly two elements.");

        if (normalizedArray[0] < 0.0 || normalizedArray[0] > 1.0 ||
            normalizedArray[1] < 0.0 || normalizedArray[1] > 1.0)
            throw new ArgumentOutOfRangeException(nameof(normalizedArray), "All elements must be in the range [0, 1].");

        uint high = (uint)(normalizedArray[0] * uint.MaxValue + 0.5); // Add 0.5 for rounding
        uint low = (uint)(normalizedArray[1] * uint.MaxValue + 0.5);

        ulong combined = ((ulong)high << 32) | low;
        return (long)combined; // Convert back to signed
    }
}

/// <summary>
/// Provides extension methods for normalizing and denormalizing integers.
/// </summary>
public static class LongExtensions
{
    /// <summary>
    /// Normalizes a long number into a float[4] array without losing precision.
    /// </summary>
    public static float[] NormalizeToFloatArray(this long value)
    {
        return Normalize.NormalizeToFloat(value);
    }

    /// <summary>
    /// converts a normalized float[4] array back to long.
    /// </summary>
    public static long LongFromNormalized(this float[] normalizedArray)
    {
        return Normalize.DenormalizeLong(normalizedArray);
    }

    /// <summary>
    /// Normalizes a long number into a double[2] array without losing precision.
    /// </summary>
    public static double[] NormalizeToDouble(this long value)
    {
        return Normalize.NormalizeToDouble(value);
    }

    /// <summary>
    /// converts a normalized double[2] array back to long.
    /// </summary>
    public static long LongFromNormalized(this double[] normalizedValue)
    {
        return Normalize.DenormalizeLong(normalizedValue);
    }
}