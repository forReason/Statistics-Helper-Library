using System.Security.Cryptography;
using System.Text;

namespace QuickStatistics.Net.AiHelpers;

/// <summary>
/// Provides functionality to normalize decimal values into separate components for neural network processing or other applications.
/// </summary>
public static partial class Normalize
{
    /// <summary>
    /// Hashes a string using SHA256 and returns two normalized floats in the range [0, 1].
    /// </summary>
    /// <remarks>
    /// can normalize up to 70 trillion unique combinations. Good for normalizing Identifiers (names/id's)
    /// </remarks>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>
    /// An array containing two floats, both normalized to the range [0, 1].
    /// The floats are derived from the first 8 bytes of the SHA256 hash.
    /// </returns>
    public static float[] NormalizeToFloat(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            // Take the first 4 bytes for the first float
            int int1 = BitConverter.ToInt32(hashBytes, 0) & 0x7FFFFFFF; // Ensure non-negative
            // Take the next 4 bytes for the second float
            int int2 = BitConverter.ToInt32(hashBytes, 4) & 0x7FFFFFFF; // Ensure non-negative

            // Normalize to range [0, 1]
            float float1 = (float)int1 / int.MaxValue;
            float float2 = (float)int2 / int.MaxValue;

            return [float1, float2];
        }
    }
    /// <summary>
    /// Hashes a string using SHA256 and returns a double in the range [0, 1].
    /// </summary>
    /// <remarks>
    /// can normalize up to 4.5 quadrillion unique combinations. Good for normalizing Identifiers (names/id's/Tags)
    /// </remarks>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>
    /// a double normalized to the range [0, 1].
    /// </returns>
    public static double NormalizeToDouble(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Take the first 8 bytes for the double
            long longValue = BitConverter.ToInt64(hashBytes, 0) & 0x7FFFFFFFFFFFFFFF; // Ensure non-negative

            // Normalize to range [0, 1]
            return (double)longValue / long.MaxValue;
        }
    }
}

/// <summary>
/// Provides extension methods for the <see cref="string"/> class to normalize strings into floats and doubles.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Hashes a string using SHA256 and returns a double in the range [0, 1].
    /// </summary>
    /// <remarks>
    /// Can normalize up to 4.5 quadrillion unique combinations. Good for normalizing identifiers (names, IDs, tags).
    /// </remarks>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>A double normalized to the range [0, 1].</returns>
    public static double ToNormalizedDouble(this string input)
    {
        return Normalize.NormalizeToDouble(input);
    }
    /// <summary>
    /// Hashes a string using SHA256 and returns a double in the range [0, 1].
    /// </summary>
    /// <remarks>
    /// Can normalize up to 4.5 quadrillion unique combinations. Good for normalizing identifiers (names, IDs, tags).
    /// </remarks>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>A double normalized to the range [0, 1].</returns>
    public static float[] ToNormalizedFloat(this string input)
    {
        return Normalize.NormalizeToFloat(input);
    }
}
