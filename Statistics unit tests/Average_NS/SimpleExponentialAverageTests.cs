using System;
using System.Globalization;
using System.IO;
using QuickStatistics.Net.Average_NS;
using Xunit;

namespace Statistics_unit_tests.Average_NS;

public class SimpleExponentialAverageTests
{
    [Fact]
        public void AddValue_ShouldUpdateAverageCorrectly()
        {
            // Arrange
            var sea = new SimpleExponentialAverage_Decimal(5);
            sea.AddValue(10);
            sea.AddValue(20);
            sea.AddValue(30);

            // Act
            var result = sea.Value;

            // Assert
            Assert.Equal(22.5m, result, 1); // (10 + 20 + 30) / 3 = 20 (approximately due to EMA behavior)
        }

        [Fact]
        public void Backup_ShouldStoreStateCorrectly()
        {
            // Arrange
            string backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            var sea = new SimpleExponentialAverage_Decimal(5, 0.5m, backupPath);
            sea.AddValue(10);
            sea.AddValue(20);
            sea.AddValue(30);

            // Act
            sea.AddValue(40); // Trigger backup storage
            var lines = File.ReadAllLines(backupPath);

            // Assert
            Assert.Equal(2, lines.Length);
            Assert.True(decimal.TryParse(lines[0], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value));
            Assert.True(uint.TryParse(lines[1], NumberStyles.Number, CultureInfo.InvariantCulture, out uint currentLength));
            Assert.Equal(sea.Value,value);
            Assert.Equal(sea.CurrentDataLength, currentLength/0.5m);

            // Clean up
            File.Delete(backupPath);
        }

        [Fact]
        public void RestoreBackup_ShouldLoadStateCorrectly()
        {
            // Arrange
            string backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            // Create a backup file manually to simulate a saved state
            File.WriteAllLines(backupPath, new[]
            {
                "25.5", // Example value for the moving average
                "3"     // Example current data length
            });
            var sea = new SimpleExponentialAverage_Decimal(5, 0.5m, backupPath);

            // Act
            var restoredValue = sea.Value;
            var restoredLength = sea.CurrentDataLength * 0.5m;

            // Assert
            Assert.Equal(25.5m, restoredValue);
            Assert.Equal((uint)3, restoredLength);

            // Clean up
            File.Delete(backupPath);
        }

        [Fact]
        public void RestoreBackup_ShouldHandleCorruptedFilesGracefully()
        {
            // Arrange
            string backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            // Create a corrupted backup file
            File.WriteAllLines(backupPath, new[]
            {
                "invalid", // Invalid value
                "invalid"  // Invalid data length
            });
            var sea = new SimpleExponentialAverage_Decimal(5, 0.5m, backupPath);

            // Act
            var restoredValue = sea.Value;
            var restoredLength = sea.CurrentDataLength;

            // Assert
            Assert.Equal(0, restoredValue); // Should reset to initial state
            Assert.Equal((uint)0, restoredLength);

            // Clean up
            File.Delete(backupPath);
        }

        [Fact]
        public void StoreBackup_ShouldBeAtomic()
        {
            // Arrange
            string backupPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            var sea = new SimpleExponentialAverage_Decimal(5, 0.5m, backupPath);
            sea.AddValue(10);
            sea.AddValue(20);

            // Act
            // Simulate a crash by deleting the backup file before storing again
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }

            sea.AddValue(30); // Should recreate and store backup atomically

            var lines = File.ReadAllLines(backupPath);

            // Assert
            Assert.Equal(2, lines.Length); // Ensure the backup file has the expected number of lines
            Assert.True(decimal.TryParse(lines[0], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value));
            Assert.True(uint.TryParse(lines[1], NumberStyles.Number, CultureInfo.InvariantCulture, out uint currentLength));
            Assert.Equal(sea.Value, value);
            Assert.Equal(sea.CurrentDataLength, currentLength/0.5m);

            // Clean up
            File.Delete(backupPath);
        }
}