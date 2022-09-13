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
            Clear();
        }
        public double Value { get; private set; }
        public int Count { get; private set; }
        public void AddValue(double input)
        {
            Count++;
            if (Count == int.MaxValue)
            {
                throw new IndexOutOfRangeException("max amount has been reached! use preciseaverage or moving avg instead!");
            }
            Value += (input - Value) / Count;
            { }
        }
        public void Clear()
        {
            Count = 0;
            Value = 0;
        }
    }
    public class Progressing_Average_Decimal
    {
        public Progressing_Average_Decimal()
        {
            Clear();
        }
        public decimal Value { get; private set; }
        private decimal _Count { get; set; }
        public void AddValue(decimal input)
        {
            Value += (input - Value) / _Count;
        }
        public void Clear()
        {
            _Count = 0;
            Value = 0;
        }
    }
}
