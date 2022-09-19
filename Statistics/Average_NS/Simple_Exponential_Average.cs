using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Average_NS
{
    /// <summary>
    /// this is an extremely lightweight and fast class in order to receive the simple moving average
    /// </summary>
    public class Simple_Exponential_Average_Double
    {
        public Simple_Exponential_Average_Double(uint maxDataLength, double divergenceCorrection = 0.29296875)
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
        public void AddPoint(double input)
        {
            _CurrentDataLength++;
            Value += (input - Value) / (_CurrentDataLength);// * DivergenceCorrection);
            _CurrentDataLength -= _CurrentDataLength / (_CorrectedDataLength + 1);
        }
    }
    /// <summary>
    /// this is an extremely lightweight and fast class in order to receive the simple moving average
    /// </summary>
    internal class Simple_Exponential_Average_Decimal
    {
        public Simple_Exponential_Average_Decimal(uint data_Length)
        {
            Clear();
        }
        public uint DataLength { get; set; }
        private uint _AddedDatapoints { get; set; }
        private bool _SeriesLengthReached { get; set; }
        public decimal Value { get; set; }
        public void Clear()
        {
            _AddedDatapoints = 0;
            Value = 0;
            _SeriesLengthReached = false;
        }
        public void AddPoint(decimal input)
        {
            if (_SeriesLengthReached)
            {
                Value += (input - Value) / DataLength;
            }
            else
            {
                _AddedDatapoints++;
                Value += (input - Value) / _AddedDatapoints;
                if (_AddedDatapoints >= DataLength)
                {
                    _SeriesLengthReached = true;
                }
            }
        }
    }
}
