using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    public class Average
    {
        public static double VolumeBasedAverage(double value1, double volume1, double value2, double volume2)
        {
            /// condition prechecks
            if (volume1 < 0 || volume2 <0)
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
            /// prepare variables
            // return variable
            double returnAverage = 0;
            // required for calculation
            double totalVolume = volume1 + volume2;
            if (double.IsNaN(totalVolume))
            { // volume1 + volume2 > double.max -> reduce precision to calculate
                volume1 /= 2;
                volume2 /= 2;
                totalVolume = volume1 + volume2;
            }
            /// attempt highest precision calculation
            // add value1 to average
            returnAverage += value1 * volume1;
            // add value2 to average
            returnAverage += value2 * volume2;
            if (!double.IsNaN(returnAverage))
            {
                return returnAverage/totalVolume;
            }
            else
            {
                returnAverage = 0;
                // reduce precision by one digit if numbers get too large
                // add value1 to average
                returnAverage += value1 * (volume1/totalVolume);
                // add value2 to average
                returnAverage += value2 * (volume2/totalVolume);
                return returnAverage;
            }
            
        }
    }
}
