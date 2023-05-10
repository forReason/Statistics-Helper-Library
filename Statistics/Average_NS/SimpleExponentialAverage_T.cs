using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuickStatistics.Net.Average_NS
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// this is an extremely lightweight and fast class in order to receive the simple moving average
    /// </summary>
    /// <remarks>has an internal conversion to Double for larger numbers, use <see cref="SimpleExponentialAverage_Decimal"/></remarks>
    public class SimpleExponentialAverage <T> where T : INumber<T>
    {
        public SimpleExponentialAverage(uint maxDataLength, double divergenceCorrection = 0.29296875)
        {
            _DivergenceCorrection = divergenceCorrection;
            MaxDataLength = maxDataLength;
            Clear();
        }
        /// <summary>
        /// the target maximum data length to keep track of
        /// </summary>
        public uint MaxDataLength 
        { 
            get
            {
                return _MaxDataLength;
            }
            set
            {
                _MaxDataLength = value;
                SetMax();
            } 
        }
        private uint _MaxDataLength;
        private uint _CorrectedDataLength;
        /// <summary>
        /// the actual number of currently tracked points (should be <= maxDataLength)
        /// </summary>
        public uint CurrentDataLength { get { return (uint)(_CurrentDataLength / DivergenceCorrection); } }
        private uint _CurrentDataLength { get; set; }
        /// <summary>
        /// The current moving average value
        /// </summary>
        public double Value { get; private set; }
        /// <summary>
        /// This is important for time series! 
        /// Lets say, the value goes up to 10000 and then turns back to 5. 
        /// The value lags behind as opposed to a true average.
        /// </summary>
        public double DivergenceCorrection
        {
            get { return _DivergenceCorrection; }
            set { 
                _DivergenceCorrection = value;
                SetMax();
            }
        }
        private double _DivergenceCorrection { get; set; }
        private void SetMax()
        {
            _CorrectedDataLength = (uint)(_MaxDataLength * _DivergenceCorrection);
            // reset size (in case of downsize)
            uint overFlow = _CurrentDataLength - _CorrectedDataLength;
            overFlow = Math.Max(overFlow, 0);
            _CurrentDataLength -= overFlow;
        }
        public void Clear()
        {
            _CurrentDataLength = 0;
            Value = 0;
        }
        public void AddValue(T input)
        {
            _CurrentDataLength++;
            Value += (Convert.ToDouble(input) - Value) / (_CurrentDataLength);// * DivergenceCorrection);
            _CurrentDataLength -= _CurrentDataLength / (_CorrectedDataLength + 1);
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
#endif
}
