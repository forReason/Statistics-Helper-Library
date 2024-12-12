using QuickStatistics.Net.AiHelpers.BiologicMeasurement;

namespace Statistics_unit_tests.AiHelpers.BiologicMeasurement;

using System;
using System.IO;
using Xunit;

public class SleepTrackerTests
{
    [Fact]
    public void GatherAwakeTime_WithoutBackupPath_DoesNotCreateFile()
    {
        var tracker = new SleepTracker();
        tracker.GatherAwakeTime(DateTime.Now);
        Assert.False(File.Exists(tracker.BackupFilePath ?? ""));
    }

    [Fact]
    public void GatherAwakeTime_WithBackupPath_CreatesBackupFile()
    {
        var tempPath = Path.GetTempFileName();
        File.Delete(tempPath);
        var tracker = new SleepTracker(tempPath);
        tracker.GatherAwakeTime(DateTime.Now);
        Assert.True(File.Exists(tempPath));
        File.Delete(tempPath);
    }

    [Fact]
    public void TodayStats_ReturnExpectedResults()
    {
        var tracker = new SleepTracker();
        var now = DateTime.Now;
        tracker.GatherAwakeTime(new DateTime(now.Year, now.Month, now.Day, 1, 30, 0)); // before 4 AM, counts as yesterday
        tracker.GatherAwakeTime(new DateTime(now.Year, now.Month, now.Day, 8, 0, 0)); // after 4 AM, counts as today
        (double wakeup, double bed) = tracker.GetTodayStats();
        Assert.InRange(wakeup, 7.9, 8.1);
    }

    [Fact]
    public void Autoload_ConstructorLoadsExistingData()
    {
        var tempPath = Path.GetTempFileName();
        File.Delete(tempPath);
        {
            var tracker = new SleepTracker(tempPath);
            tracker.GatherAwakeTime(DateTime.Now);
        }
        var loadedTracker = new SleepTracker(tempPath);
        var stats = loadedTracker.GetTodayStats();
        Assert.NotEqual(0.0, stats.wakeupTime);
        File.Delete(tempPath);
    }
    [Fact]
    public void GatherAwakeTime_MultipleDays_StatsReflectDayBoundaryAndMultipleEntries()
    {
        var tracker = new SleepTracker();

        // Times for "yesterday" before the 4 AM boundary (counts as the previous day)
        var yesterday = DateTime.Now.AddDays(-1);
        var yesterdayPreBoundary = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 4, 15, 0);
        var yesterdayPostBoundary = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 10, 0, 0);

        // Times for "today" straddling the boundary
        var today = DateTime.Now;
        var todayPreBoundary = new DateTime(today.Year, today.Month, today.Day, 2, 30, 0);  // Counts as yesterday
        var todayMorning = new DateTime(today.Year, today.Month, today.Day, 6, 15, 0);
        var todayEvening = new DateTime(today.Year, today.Month, today.Day, 22, 45, 0);

        // Gather times
        tracker.GatherAwakeTime(yesterdayPreBoundary);
        tracker.GatherAwakeTime(yesterdayPostBoundary);
        tracker.GatherAwakeTime(todayPreBoundary);
        tracker.GatherAwakeTime(todayMorning);
        tracker.GatherAwakeTime(todayEvening);

        // Yesterday stats (should include yesterdayPreBoundary, yesterdayPostBoundary, and todayPreBoundary)
        // These should yield earliest around 2:30 AM (converted to previous day) and latest around 10:00 AM.
        var yesterdayStats = tracker.GetYesterdayStats();
        Assert.InRange(yesterdayStats.wakeupTime, 4, 4.5); // 2:30 AM is 2.5 hours, roughly.
        Assert.InRange(yesterdayStats.bedTime, 2.5, 2.6);

        // Today stats (should include just todayMorning and todayEvening)
        var todayStats = tracker.GetTodayStats();
        Assert.InRange(todayStats.wakeupTime, 6.2, 6.3); // 6:15 AM ~ 6.25 hours
        Assert.InRange(todayStats.bedTime, 22.7, 22.8); // 10:45 PM ~ 22.75 hours
    }

}
