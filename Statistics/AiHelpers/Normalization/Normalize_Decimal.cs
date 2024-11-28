namespace QuickStatistics.Net.AiHelpers;

/// <summary>
/// Provides functionality to normalize decimal values into separate components for neural network processing or other applications.
/// </summary>
public static partial class Normalize
{
    /// <summary>
    /// Normalizes a decimal number into a double[5] array without losing precision.
    /// </summary>
    public static double[] NormalizeToDouble(decimal value)
    {
        int[] bits = decimal.GetBits(value);

        double[] normalizedArray = new double[5];

        // Normalize the 96-bit integer parts
        for (int i = 0; i < 3; i++)
        {
            uint part = (uint)bits[i];
            normalizedArray[i] = part / (double)UInt32.MaxValue;
        }

        // Extract and normalize the scale (0-28)
        int scale = (bits[3] >> 16) & 0xFF;
        normalizedArray[3] = scale / 28.0;

        // Extract and normalize the sign
        bool isNegative = (bits[3] & 0x80000000) != 0;
        normalizedArray[4] = isNegative ? 1.0 : 0.0;

        return normalizedArray;
    }

    /// <summary>
    /// Denormalizes a double[5] array back into a decimal number.
    /// </summary>
    public static decimal DenormalizeDecimal(double[] normalizedArray)
    {
        if (normalizedArray == null || normalizedArray.Length != 5)
            throw new ArgumentException("Array must have exactly five elements.");

        uint[] parts = new uint[3];
        for (int i = 0; i < 3; i++)
        {
            if (normalizedArray[i] < 0.0 || normalizedArray[i] > 1.0)
                throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Elements must be in [0, 1].");

            parts[i] = (uint)(normalizedArray[i] * UInt32.MaxValue + 0.5);
        }

        int scale = (int)(normalizedArray[3] * 28.0 + 0.5);

        if (scale < 0 || scale > 28)
            throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Scale element must correspond to a valid scale (0-28).");

        if (normalizedArray[4] < 0 || normalizedArray[4] > 1)
            throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Elements must be in [0, 1].");
        bool isNegative = normalizedArray[4] >= 0.5;

        int flags = (scale << 16);
        if (isNegative)
            flags |= unchecked((int)0x80000000);

        int[] bits = new int[4];
        bits[0] = (int)parts[0];
        bits[1] = (int)parts[1];
        bits[2] = (int)parts[2];
        bits[3] = flags;

        return new decimal(bits);
    }
    /// <summary>
    /// Normalizes a decimal number into a float[8] array without losing precision.
    /// </summary>
    public static float[] NormalizeToFloat(decimal value)
    {
        int[] bits = decimal.GetBits(value);

        // bits[0], bits[1], bits[2] contain the 96-bit integer part
        // We will split each 32-bit integer into two 16-bit parts
        float[] normalizedArray = new float[8];

        for (int i = 0; i < 3; i++)
        {
            uint part = (uint)bits[i];

            // Split into two 16-bit parts
            ushort low = (ushort)(part & 0xFFFF);
            ushort high = (ushort)(part >> 16);

            // Normalize each 16-bit part
            normalizedArray[i * 2] = low / (float)ushort.MaxValue;
            normalizedArray[i * 2 + 1] = high / (float)ushort.MaxValue;
        }

        // Extract and normalize the scale (0-28)
        int scale = (bits[3] >> 16) & 0xFF;
        normalizedArray[6] = scale / 28.0f;

        // Extract and normalize the sign
        bool isNegative = (bits[3] & unchecked((int)0x80000000)) != 0;
        normalizedArray[7] = isNegative ? 1.0f : 0.0f;

        return normalizedArray;
    }

    /// <summary>
    /// Denormalizes a float[8] array back into a decimal number.
    /// </summary>
    public static decimal DenormalizeDecimal(float[] normalizedArray)
    {
        if (normalizedArray == null || normalizedArray.Length != 8)
            throw new ArgumentException("Array must have exactly eight elements.");

        uint[] parts = new uint[3];

        for (int i = 0; i < 3; i++)
        {
            float lowNorm = normalizedArray[i * 2];
            float highNorm = normalizedArray[i * 2 + 1];

            if (lowNorm < 0.0f || lowNorm > 1.0f || highNorm < 0.0f || highNorm > 1.0f)
                throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Elements must be in [0, 1].");

            ushort low = (ushort)(lowNorm * ushort.MaxValue + 0.5f);
            ushort high = (ushort)(highNorm * ushort.MaxValue + 0.5f);

            parts[i] = ((uint)high << 16) | low;
        }

        // Denormalize the scale
        float scaleNorm = normalizedArray[6];
        if (scaleNorm < 0.0f || scaleNorm > 1.0f)
            throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Scale element must be in [0, 1].");

        int scale = (int)(scaleNorm * 28.0f + 0.5f);

        if (scale < 0 || scale > 28)
            throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Scale must be between 0 and 28.");

        // Denormalize the sign
        float signNorm = normalizedArray[7];
        if (signNorm < 0.0f || signNorm > 1.0f)
            throw new ArgumentOutOfRangeException(nameof(normalizedArray), "Sign element must be in [0, 1].");

        bool isNegative = signNorm >= 0.5f;

        int flags = scale << 16;
        if (isNegative)
            flags |= unchecked((int)0x80000000);

        int[] bits = new int[4];
        bits[0] = (int)parts[0];
        bits[1] = (int)parts[1];
        bits[2] = (int)parts[2];
        bits[3] = flags;

        return new decimal(bits);
    }
}
public static class DecimalExtensions
{
    /// <summary>
    /// Normalizes a decimal number into a float[8] array without losing precision.
    /// </summary>
    public static float[] ToNormalizedFloatArray(this decimal value)
    {
        return Normalize.NormalizeToFloat(value);
    }

    /// <summary>
    /// Denormalizes a float[8] array back into a decimal number.
    /// </summary>
    public static decimal DecimalFromNormalized(this float[] normalizedArray)
    {
        return Normalize.DenormalizeDecimal(normalizedArray);
    }

    /// <summary>
    /// Normalizes a decimal number into a double[5] array without losing precision.
    /// </summary>
    public static double[] ToNormalizedDoubleArray(this decimal value)
    {
        return Normalize.NormalizeToDouble(value);
    }

    /// <summary>
    /// Denormalizes a double[5] array back into a decimal number.
    /// </summary>
    public static decimal DecimalFromNormalized(this double[] normalizedArray)
    {
        return Normalize.DenormalizeDecimal(normalizedArray);
    }
}

