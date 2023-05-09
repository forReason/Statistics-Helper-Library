using System;
using Xunit;
using QuickStatistics.Net.Average_NS;

namespace Statistics_unit_tests.Average_NS
{
    public class VolumetricAverage
    {
        [Fact]
        public void BasicVolumeAverage_PositiveValues()
        {
            // positive tests
            double testResult = Volumetric_Average.VolumeBasedAverage(value1: 0.1, volume1: 2, value2: 0.01, volume2: 0.1);
            if (testResult != 0.09571428571428571)
            {
                throw new System.Exception($"result was: {testResult} should be: 0.09571428571428571");
            }
            testResult = Volumetric_Average.VolumeBasedAverage(value1: 0.1, volume1: 2, value2: 0.9, volume2: 0.1);
            if (testResult != 0.1380952380952381)
            {
                throw new System.Exception($"result was: {testResult} should be:  0.1380952380952381");
            }
            // inversed positive tests
            testResult = Volumetric_Average.VolumeBasedAverage(value2: 0.1, volume2: 2, value1: 0.01, volume1: 0.1);
            if (testResult != 0.09571428571428571)
            {
                throw new System.Exception($"result was: {testResult} should be: 0.09571428571428571");
            }
            testResult = Volumetric_Average.VolumeBasedAverage(value2: 0.1, volume2: 2, value1: 0.9, volume1: 0.1);
            if (testResult != 0.1380952380952381)
            {
                throw new System.Exception($"result was: {testResult} should be:  0.1380952380952381");
            }
        }
        [Fact]
        public void BasicVolumeAverage_EqualValues()
        {
            Random rng = new Random();
            for (double i = double.MinValue; i <= double.MaxValue; i += double.MaxValue / 1049)
            { // equal volume and value
                double testResult = Volumetric_Average.VolumeBasedAverage(value1: i, volume1: Math.Abs(i), value2: i, volume2: Math.Abs(i));
                if (testResult != i)
                {
                    throw new System.Exception($"result was: {testResult} should be: {i}");
                }
            }
            for (double i = double.MinValue; i <= double.MaxValue; i += double.MaxValue / 1068)
            { // equal value
                double testResult = Volumetric_Average.VolumeBasedAverage(value1: i, volume1: rng.NextDouble(), value2: i, volume2: rng.NextDouble());
                if (testResult != i)
                {
                    throw new System.Exception($"result was: {testResult} should be: {i}");
                }
            }
        }
        [Fact]
        public void BasicVolumeAverage_PositiveNegativeValues()
        {
            double testResult = 0;
            // test null
            for (double i = 0; i <= double.MaxValue; i += double.MaxValue / 1049)
            {
                testResult = Volumetric_Average.VolumeBasedAverage(value1: i, volume1: i, value2: -i, volume2: i);
                if (testResult != 0)
                {
                    throw new System.Exception($"result was: {testResult} should be: {0}");
                }
            }
            // manual test cases
            testResult = Volumetric_Average.VolumeBasedAverage(value1: 0.1, volume1: 2, value2: -0.01, volume2: 0.1);
            if (testResult != 0.094761904761904769)
            {
                throw new System.Exception($"result was: {testResult} should be:  0.094761904761904769");
            }
            testResult = Volumetric_Average.VolumeBasedAverage(value1: 0.1, volume1: 2, value2: -0.9, volume2: 0.3);
            if (testResult != -0.030434782608695657)
            {
                throw new System.Exception($"result was: {testResult} should be:  -0.030434782608695657");
            }
            // inversed  tests
            testResult = Volumetric_Average.VolumeBasedAverage(value2: 0.1, volume2: 2, value1: -0.01, volume1: 0.1);
            if (testResult != 0.094761904761904769)
            {
                throw new System.Exception($"result was: {testResult} should be:  0.094761904761904769");
            }
            testResult = Volumetric_Average.VolumeBasedAverage(value2: 0.1, volume2: 2, value1: -0.9, volume1: 0.3);
            if (testResult != -0.030434782608695657)
            {
                throw new System.Exception($"result was: {testResult} should be:  -0.030434782608695657");
            }
        }
        [Fact]
        public void BasicVolumeAverage_PositiveNegativeValues_Edgecases()
        {
            // edge cases
            double testResult = Volumetric_Average.VolumeBasedAverage(value2: double.MaxValue, volume2: 5, value1: double.MinValue, volume1: 10);
            if (testResult != double.MinValue * 0.33333333333333333333)
            {
                throw new System.Exception($"result was: {testResult} should be: {double.MinValue - (double.MaxValue / 2)}");
            }
        }
        [Fact]
        public void BasicVolumeAverage_WithZero_Edgecases()
        {
            // edge cases
            double testResult = Volumetric_Average.VolumeBasedAverage(value2: 0, volume2: 0, value1: 10, volume1: 10);
            Assert.Equal(10, testResult);
        }
    }
}
