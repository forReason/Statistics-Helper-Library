using QuickStatistics.Net.Average_NS;
using System;
using System.IO;
using Xunit;

namespace Statistics_unit_tests.Average_NS
{
    public class SimpleMovingAverageTests
    {
        [Fact]
        public void TestDataLength_NewInstance()
        {
            int maxSize = 50000;
            for (int targetLength = 0; targetLength < maxSize; targetLength += maxSize/20)
            {
                SimpleMovingAverage_Double sma = new SimpleMovingAverage_Double(targetLength);
                if (sma.MaxDataLength != targetLength)
                {
                    throw new Exception($"target length ({targetLength}) and max data length ({sma.MaxDataLength}) does not match up!");
                }
                for(int i = 0; i < targetLength * 2; i++)
                {
                    sma.AddValue(1);
                }
                if (sma.CurrentDataLength != targetLength)
                {
                    { }
                    throw new Exception($"Current Data Length ({sma.CurrentDataLength}) does not match target data length ({targetLength})!");
                }
            }
        }
        //[Fact]
        //public void TestDataLength_ReusedInstance_Upsize()
        //{
        //    Simple_Moving_Average_Double sma = new Simple_Moving_Average_Double(0);
        //    uint maxSize = 50000;
        //    for (uint targetLength = 0; targetLength < maxSize; targetLength += maxSize/20)
        //    {
        //        sma.MaxDataLength = targetLength;
        //        if (sma.MaxDataLength != targetLength)
        //        {
        //            throw new Exception($"target length ({targetLength}) and max data length ({sma.MaxDataLength}) does not match up!");
        //        }
        //        for (int i = 0; i < targetLength * 2; i++)
        //        {
        //            sma.AddPoint(1);
        //        }
        //        if (sma.CurrentDataLength != targetLength)
        //        {
        //            { }
        //            throw new Exception($"Current Data Length ({sma.CurrentDataLength}) does not match target data length ({targetLength})!");
        //        }
        //    }
        //}
        //[Fact]
        //public void TestDataLength_ReusedInstance_Downsize()
        //{
        //    Simple_Moving_Average_Double sma = new Simple_Moving_Average_Double(0);
        //    uint maxSize = 50000;
        //    for (uint targetLength = maxSize; targetLength > 0; targetLength -= maxSize / 20)
        //    {
        //        sma.MaxDataLength = targetLength;
        //        if (sma.MaxDataLength != targetLength)
        //        {
        //            throw new Exception($"target length ({targetLength}) and max data length ({sma.MaxDataLength}) does not match up!");
        //        }
        //        for (int i = 0; i < targetLength * 2; i++)
        //        {
        //            sma.AddPoint(1);
        //        }
        //        if (sma.CurrentDataLength != targetLength)
        //        {
        //            { }
        //            throw new Exception($"Current Data Length ({sma.CurrentDataLength}) does not match target data length ({targetLength})!");
        //        }
        //    }
        //}
        [Fact]
        public void StaticPositiveValues()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                SimpleMovingAverage_Double sma = new SimpleMovingAverage_Double(10);
                double result = rng.NextDouble() * i;
                for (uint b = 0; b < max; b += stepSize)
                {
                    sma.AddValue(result);
                    double diff = Math.Abs(result - sma.Value);
                    double diffPercent = diff / result;
                    if (diffPercent > 0.00001)
                    {
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
        [Fact]
        public void CompareWithProgressingAverage()
        {
            // positive tests
            Random rng = new Random();
            uint max = int.MaxValue;
            uint stepSize = max / 20;
            for (uint i = 0; i < max; i += stepSize)
            {
                SimpleMovingAverage_Double simpleAverage = new SimpleMovingAverage_Double(1000);
                ProgressingAverage_Double progressingAverage = new ProgressingAverage_Double();
                for (uint b = 0; b < 1000; b ++)
                {
                    double result = (rng.NextDouble()-0.5) * i;
                    progressingAverage.AddValue(result);
                    simpleAverage.AddValue(result);
                    double difference = Math.Abs(progressingAverage.Value - simpleAverage.Value);
                    double percentDifference = difference / Math.Abs(progressingAverage.Value);
                    if (percentDifference > 0.5)
                    {
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
        [Fact]
        public void MoveTowardsValue()
        {
            // positive tests
            Random rng = new Random();
            int max = 50000;
            int stepSize = max / 20;
            for (int i = 0; i < max; i += stepSize)
            {
                for(int dataLength = 1; dataLength < 50000; dataLength += 50000/20)
                {
                    SimpleMovingAverage_Double simpleAverage = new SimpleMovingAverage_Double(dataLength);
                    double startValue = (rng.NextDouble() - 0.5) * i;
                    double targetValue = (rng.NextDouble() - 0.5) * i;

                    for (uint b = 0; b <= dataLength; b++)
                    {
                        simpleAverage.AddValue(startValue);
                    }
                    for (uint b = 0; b <= dataLength; b++)
                    {
                        simpleAverage.AddValue(targetValue);
                    }
                    double divergence = Math.Abs(1 - (simpleAverage.Value / targetValue));
                    if (divergence != double.NaN && (divergence > 0.01))
                    {
                        throw new Exception("Value does not add up!");
                    }
                }
            }
        }
        [Fact]
        public void AddValue_ShouldUpdateAverageCorrectly()
        {
            // Arrange
            var sma = new SimpleMovingAverage_Decimal(3);
            sma.AddValue(1);
            sma.AddValue(2);
            sma.AddValue(3);

            // Act
            var result = sma.Value;

            // Assert
            Assert.Equal(2, result); // (1 + 2 + 3) / 3 = 2
        }

        [Fact]
        public void Backup_ShouldStoreValuesCorrectly()
        {
            // Arrange
            string backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            var sma = new SimpleMovingAverage_Decimal(3, backupPath);
            sma.AddValue(1);
            sma.AddValue(2);
            sma.AddValue(3);

            // Act
            var lines = File.ReadAllLines(backupPath);

            // Assert
            Assert.Equal(3, lines.Length);

            // Clean up
            File.Delete(backupPath);
        }

        [Fact]
        public void RestoreBackup_ShouldLoadValuesCorrectly()
        {
            // Arrange
            string backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            File.WriteAllLines(backupPath, new[] { "0.3333333333333333333333333333", "0.6666666666666666666666666667", "1" });
            var sma = new SimpleMovingAverage_Decimal(3, backupPath);

            // Act
            var result = Math.Round(sma.Value,3);

            // Assert
            Assert.Equal(0.667m, result); // (1 + 2 + 3) / 3 = 2

            // Clean up
            File.Delete(backupPath);
        }

        [Fact]
        public void AddValue_ShouldNotThrow_WhenBackupPathIsEmpty()
        {
            // Arrange
            var sma = new SimpleMovingAverage_Decimal(3, "");

            // Act & Assert
            var exception = Record.Exception(() => sma.AddValue(1));
            Assert.Null(exception); // Ensure no exception is thrown
        }

        [Fact]
        public void Backup_ShouldBeDisabled_WhenBackupPathIsNull()
        {
            // Arrange
            var sma = new SimpleMovingAverage_Decimal(3, null);

            // Act
            sma.AddValue(1);
            sma.AddValue(2);
            sma.AddValue(3);

            // BackupFile should be null and no backup should be stored
            Assert.Null(sma.BackupFile);
        }
    }
}
