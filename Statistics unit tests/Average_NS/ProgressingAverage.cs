using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistics.Average_NS;
using Xunit;

namespace Statistics_unit_tests.Average_NS
{
    public class ProgressingAverage
    {
        [Fact]
        public void StaticPositiveValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for(uint i = 0; i < max; i+= stepSize)
            {
                Progressing_Average_Double progressingAverage = new Progressing_Average_Double();
                double result = rng.NextDouble() * i;
                for (uint b = 0 ; b < max; b+= stepSize)
                {
                    progressingAverage.AddValue(result);
                }
                if (progressingAverage.Value != result)
                {
                    throw new Exception("Value does not add up!");
                }
            }
        }
    }
}
