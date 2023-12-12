using System.Numerics;
using System.Runtime.CompilerServices;

namespace QuickStatistics.Net.Math_NS
{
    /// <summary>
    /// Provides methods to calculate the absolute difference between two numeric values.
    /// </summary>
    /// <remarks>
    /// This can circumvent certain limitations with Math.Abs for unsigned types (eg. Math.Abs(ulong-ulong) <br/>
    /// </remarks>
    public static class Difference
    {
        /// <summary>
        /// Calculates the absolute difference between two byte values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Get(byte value1, byte value2)
        {
            return value1 >= value2 ? (byte)(value1 - value2) : (byte)(value2 - value1);
        }

        /// <summary>
        /// Calculates the absolute difference between two ushort values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static ushort Get(ushort value1, ushort value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return (ushort)(value1 - value2);
            else
                return (ushort)(value2 - value1);
        }
        /// <summary>
        /// Calculates the absolute difference between two short values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static short Get(short value1, short value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return (short)(value1 - value2);
            else
                return (short)(value2 - value1);
        }
        /// <summary>
        /// Calculates the absolute difference between two uint values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static uint Get(uint value1, uint value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return (uint)(value1 - value2);
            else
                return (uint)(value2 - value1);
        }
        /// <summary>
        /// Calculates the absolute difference between two int values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static int Get(int value1, int value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
        /// <summary>
        /// Calculates the absolute difference between two ulong values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static ulong Get(ulong value1, ulong value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
        /// <summary>
        /// Calculates the absolute difference between two long values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static long Get(long value1, long value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
        /// <summary>
        /// Calculates the absolute difference between two BigInteger values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static BigInteger Get(BigInteger value1, BigInteger value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
        /// <summary>
        /// Calculates the absolute difference between two float values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static float Get(float value1, float value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
        /// <summary>
        /// Calculates the absolute difference between two double values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static double Get(double value1, double value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
        /// <summary>
        /// Calculates the absolute difference between two decimal values.
        /// </summary>
        /// <remarks>
        /// the order of elements is not relevant and gets adjusted for automatically
        /// </remarks>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>The absolute difference between the two values.</returns>
        public static decimal Get(decimal value1, decimal value2)
        {
            if (value1 == value2)
                return 0;
            else if (value1 > value2)
                return value1 - value2;
            else
                return value2 - value1;
        }
    }
}
