
using System.Numerics;
using System.Windows.Markup;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// calculates the average based on two amounts.
    /// </summary>
    /// <remarks>has an internal conversion to double. for larger numbers, use <see cref="VolumetricAverage_Decimal"/></remarks>
    public class VolumetricAverage<T> where T : INumber<T>
    {
        /// <summary>
        /// Builds an average based on two values based on their amount
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="volume1"></param>
        /// <param name="value2"></param>
        /// <param name="volume2"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">negative volumes not allowed</exception>
        public static double VolumeBasedAverage(T value1, T volume1, T value2, T volume2)
        {
            return VolumetricAverage_Double.VolumeBasedAverage(
                Convert.ToDouble(value1), Convert.ToDouble(volume1),
                Convert.ToDouble(value2), Convert.ToDouble(volume2));
        }
        public static double VolumeBasedAverage(VolumetricValue<T>[] values)
        {
            VolumetricValue_Double[] doubleValues = new VolumetricValue_Double[values.Length];
            for(int i = 0; i < values.Length; i++)
            {
                doubleValues[i] = new VolumetricValue_Double(Convert.ToDouble(values[i].Value), Convert.ToDouble(values[i].Volume));
            }
            return VolumetricAverage_Double.VolumeBasedAverage(doubleValues);
        }
    }
    public struct VolumetricValue<T> where T : INumber<T>
    {
        public VolumetricValue(T value, T volume)
        {
            Value = value;
            Volume = volume;
        }
        public T Value { get; set; }
        public T Volume { get; set; }
    }
#endif
}
