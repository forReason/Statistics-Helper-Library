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
    internal class Simple_Moving_Average_Double
    {
        public Simple_Moving_Average_Double(uint data_Length)
        {
            Clear();
        }
        public uint DataLength { get; set; }
        private uint _AddedDatapoints { get; set; }
        private bool _SeriesLengthReached { get; set; }
        public double Value { get; set; }
        public void Clear()
        {
            _AddedDatapoints = 0;
            Value = 0;
            _SeriesLengthReached = false;
        }
        public void AddPoint(double input)
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
                    _SeriesLengthReached=true;
                }
            }
        }
    }
    /// <summary>
    /// this is an extremely lightweight and fast class in order to receive the simple moving average
    /// </summary>
    internal class Simple_Moving_Average_Decimal
    {
        public Simple_Moving_Average_Decimal(uint data_Length)
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
