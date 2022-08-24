using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics.Average_NS
{
    public class Progressing_Average_Double
    {
        public Progressing_Average_Double()
        {
            _Count = 0;
            Value = 0;
        }
        public double Value { get; private set; }
        private int _Count { get; set; }
        public void AddValue(double input)
        {
            if (_Count == int.MaxValue)
            {
                throw new IndexOutOfRangeException("max amount has been reached! use preciseaverage or moving avg instead!");
            }
            _Count++;
            Value += (input - Value) / _Count;
        }
    }
    public class Progressing_Average_Decimal
    {
        public Progressing_Average_Decimal()
        {
            _Count = 0;
            Value = 0;
        }
        public decimal Value { get; private set; }
        private decimal _Count { get; set; }
        public void AddValue(decimal input)
        {
            Value += (input - Value) / _Count;
        }
    }
}
