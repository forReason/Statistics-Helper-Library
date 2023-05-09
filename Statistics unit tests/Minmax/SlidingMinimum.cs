using System;
using Xunit;
using QuickStatistics.Net.MinMax_NS;

namespace Statistics_unit_tests.Minmax
{
    public class SlidingMaximum
    {
        [Fact]
        public void TestMaximumWithWrapAround()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Maximum sliding_Maximum = new Sliding_Maximum(10);
            sliding_Maximum.AddPoint(10);
            if (sliding_Maximum.Value != 10.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
            sliding_Maximum.AddPoint(3);
            sliding_Maximum.AddPoint(2);
            sliding_Maximum.AddPoint(4);
            sliding_Maximum.AddPoint(2);
            sliding_Maximum.AddPoint(3);
            sliding_Maximum.AddPoint(4);
            sliding_Maximum.AddPoint(7);
            sliding_Maximum.AddPoint(9);
            sliding_Maximum.AddPoint(5);
            if (sliding_Maximum.Value != 10.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
            sliding_Maximum.AddPoint(2);
            sliding_Maximum.AddPoint(2);
            if (sliding_Maximum.Value != 9.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
        }
        [Fact]
        public void TestMaximumForNegativeNumbers()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Maximum sliding_Maximum = new Sliding_Maximum(10);
            sliding_Maximum.AddPoint(-2);
            if (sliding_Maximum.Value != -2.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
            sliding_Maximum.AddPoint(-3);
            sliding_Maximum.AddPoint(-5);
            sliding_Maximum.AddPoint(-4);
            sliding_Maximum.AddPoint(-7);
            sliding_Maximum.AddPoint(-3);
            sliding_Maximum.AddPoint(-4);
            sliding_Maximum.AddPoint(-7);
            sliding_Maximum.AddPoint(-9);
            sliding_Maximum.AddPoint(-5);
            if (sliding_Maximum.Value != -2.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
            sliding_Maximum.AddPoint(-8);
            sliding_Maximum.AddPoint(-9);
            if (sliding_Maximum.Value != -3.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
        }
        [Fact]
        public void TestClearing()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Maximum sliding_Maximum = new Sliding_Maximum(10);
            sliding_Maximum.AddPoint(10);
            if (sliding_Maximum.Value != 10)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
            sliding_Maximum.AddPoint(2);
            sliding_Maximum.AddPoint(8);
            sliding_Maximum.Clear();
            if (sliding_Maximum.Value != double.MinValue)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
            sliding_Maximum.AddPoint(-8);
            if (sliding_Maximum.Value != -8.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
        }
        [Fact]
        public void TestPositiveNegativeNumbers()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Maximum sliding_Maximum = new Sliding_Maximum(10);
            sliding_Maximum.AddPoint(10);
            sliding_Maximum.AddPoint(-22);
            sliding_Maximum.AddPoint(-8);
            sliding_Maximum.AddPoint(-8);
            if (sliding_Maximum.Value != 10.0)
            {
                throw new Exception($"value {sliding_Maximum.Value} is incorrect!");
            }
        }
    }
}
