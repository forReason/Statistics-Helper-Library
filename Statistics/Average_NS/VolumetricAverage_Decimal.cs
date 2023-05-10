
namespace QuickStatistics.Net.Average_NS
{
    public class VolumetricAverage_Decimal
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
        public static decimal VolumeBasedAverage(decimal value1, decimal volume1, decimal value2, decimal volume2)
        {
            /// condition prechecks
            if (volume1 < 0 || volume2 < 0)
            {
                throw new NotImplementedException("negative volumes are not implemented!");
            }
            /// easy calculation checks
            // check if input values are equal (then no calculation required
            if (value1 == value2)
            {
                return value1;
            }
            // check if input volumes are equal (can take simple avg then)
            if (volume1 == volume2)
            {
                return (value1 / 2) + (value2 / 2);
            }
            // check if ine volume is 0
            if (volume1 == 0)
            {
                return value2;
            }
            else if (volume2 == 0)
            {
                return value1;
            }
            /// prepare variables
            // return variable
            decimal returnAverage = 0;
            // required for calculation
            decimal totalVolume = volume1 + volume2;
            /// attempt highest precision calculation
            // add value1 to average
            returnAverage += value1 * volume1;
            // add value2 to average
            returnAverage += value2 * volume2;
            return returnAverage / totalVolume;
        }
        public static decimal VolumeBasedAverage(VolumetricValue_Decimal[] values)
        {
            decimal totalVolume = 0;
            foreach (VolumetricValue_Decimal item in values)
            {
                totalVolume += item.Volume;
            }
            if (totalVolume == 0) return 0;
            decimal result = 0;
            foreach(VolumetricValue_Decimal item in values)
            {
                decimal factor = (item.Volume / totalVolume);
                result += item.Value * factor;
            }
            return result;
        }
    }
    public struct VolumetricValue_Decimal
    {
        public VolumetricValue_Decimal(decimal value, decimal volume)
        {
            Value = value;
            Volume = volume;
        }
        public decimal Value { get; set; }
        public decimal Volume { get; set; }
    }
}
