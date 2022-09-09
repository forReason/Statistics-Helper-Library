using System;
using Xunit;
using Statistics.MinMax_NS;

namespace Statistics_unit_tests.Minmax
{
    public class SlidingMinimum
    {
        [Fact]
        public void TestMinimumWithWrapAround()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Minimum sliding_Minimum = new Sliding_Minimum(10);
            sliding_Minimum.AddPoint(10);
            if (sliding_Minimum.Value != 10.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(12);
            sliding_Minimum.AddPoint(12);
            sliding_Minimum.AddPoint(14);
            sliding_Minimum.AddPoint(12);
            sliding_Minimum.AddPoint(13);
            sliding_Minimum.AddPoint(14);
            sliding_Minimum.AddPoint(17);
            sliding_Minimum.AddPoint(19);
            sliding_Minimum.AddPoint(15);
            if (sliding_Minimum.Value != 10.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(13);
            sliding_Minimum.AddPoint(13);
            if (sliding_Minimum.Value != 12.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(4);
            if (sliding_Minimum.Value != 4.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
        }
        [Fact]
        public void TestMinimumForNegativeNumbers()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Minimum sliding_Minimum = new Sliding_Minimum(10);
            sliding_Minimum.AddPoint(-8);
            if (sliding_Minimum.Value != -8.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(-3);
            sliding_Minimum.AddPoint(-5);
            sliding_Minimum.AddPoint(-4);
            sliding_Minimum.AddPoint(-7);
            sliding_Minimum.AddPoint(-3);
            sliding_Minimum.AddPoint(-4);
            sliding_Minimum.AddPoint(-7);
            sliding_Minimum.AddPoint(-6);
            sliding_Minimum.AddPoint(-5);
            if (sliding_Minimum.Value != -8.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(-6);
            sliding_Minimum.AddPoint(-6);
            if (sliding_Minimum.Value != -7.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
        }
        [Fact]
        public void TestClearing()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Minimum sliding_Minimum = new Sliding_Minimum(10);
            sliding_Minimum.AddPoint(10);
            if (sliding_Minimum.Value != 10)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(2);
            sliding_Minimum.AddPoint(8);
            sliding_Minimum.Clear();
            if (sliding_Minimum.Value != double.MaxValue)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
            sliding_Minimum.AddPoint(-8);
            if (sliding_Minimum.Value != -8.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
        }
        [Fact]
        public void TestPositiveNegativeNumbers()
        {
            // positive tests
            Random rng = new Random();
            Sliding_Minimum sliding_Minimum = new Sliding_Minimum(10);
            sliding_Minimum.AddPoint(10);
            sliding_Minimum.AddPoint(-22);
            sliding_Minimum.AddPoint(-8);
            sliding_Minimum.AddPoint(-8);
            if (sliding_Minimum.Value != -22.0)
            {
                throw new Exception($"value {sliding_Minimum.Value} is incorrect!");
            }
        }
    }
}
