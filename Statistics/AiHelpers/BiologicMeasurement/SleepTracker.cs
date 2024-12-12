namespace QuickStatistics.Net.AiHelpers.BiologicMeasurement;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class SleepTracker
{
    public SleepTracker(string backupFilePath = null)
    {
        BackupFilePath = backupFilePath;
        if (!string.IsNullOrWhiteSpace(BackupFilePath))
            LoadData();
    }
    private readonly Dictionary<DateTime, List<DateTime>> dailyMessageTimes 
        = new Dictionary<DateTime, List<DateTime>>();
    private const int DayBoundaryHour = 4;

    /// <summary>
    /// Optional backup file path. When set, calls to SaveData() and LoadData() will use this path.
    /// </summary>
    public string BackupFilePath { get; set; }

    /// <summary>
    /// Saves the current dailyMessageTimes to the optional backup path if set.
    /// </summary>
    private void SaveData()
    {
        if (string.IsNullOrWhiteSpace(BackupFilePath))
            return;

        var json = JsonSerializer.Serialize(dailyMessageTimes);
        var tempFile = BackupFilePath + ".tmp";
        File.WriteAllText(tempFile, json);
        File.Move(tempFile, BackupFilePath, true);
    }


    /// <summary>
    /// Loads the dailyMessageTimes from the optional backup path if set and file exists.
    /// </summary>
    private void LoadData()
    {
        if (string.IsNullOrWhiteSpace(BackupFilePath) || !File.Exists(BackupFilePath))
            return;

        var json = File.ReadAllText(BackupFilePath);
        var data = JsonSerializer.Deserialize<Dictionary<DateTime, List<DateTime>>>(json);
        if (data != null)
        {
            dailyMessageTimes.Clear();
            foreach (var kvp in data)
                dailyMessageTimes[kvp.Key] = kvp.Value;
        }
    }

    public void CleanupOldData(int daysToKeep)
    {
        var cutoff = DateTime.Today.AddDays(-daysToKeep);
        var keysToRemove = dailyMessageTimes.Keys.Where(k => k < cutoff).ToList();
        foreach (var key in keysToRemove)
            dailyMessageTimes.Remove(key);
    }

    public void GatherAwakeTime(DateTime messageTime)
    {
        var adjustedDateKey = GetAdjustedDateKey(messageTime);
        if (!dailyMessageTimes.ContainsKey(adjustedDateKey))
            dailyMessageTimes[adjustedDateKey] = new List<DateTime>();

        dailyMessageTimes[adjustedDateKey].Add(messageTime);
        dailyMessageTimes[adjustedDateKey].Sort();
        if (!string.IsNullOrEmpty(BackupFilePath)) SaveData();
    }

    public (double wakeupTime, double bedTime) GetOverallAwakeStats()
    {
        return GetAwakeStatsForDates(dailyMessageTimes.Keys.ToList());
    }

    public (double wakeupTime, double bedTime) GetTodayStats()
    {
        var todayKey = GetAdjustedDateKey(DateTime.Now);
        return GetAwakeStatsForDates(new List<DateTime> { todayKey });
    }

    public (double wakeupTime, double bedTime) GetYesterdayStats()
    {
        var yesterdayKey = GetAdjustedDateKey(DateTime.Now.AddDays(-1));
        return GetAwakeStatsForDates(new List<DateTime> { yesterdayKey });
    }

    public (double wakeupTime, double bedTime) GetLastFiveDaysStats()
    {
        var days = Enumerable.Range(0, 5)
            .Select(i => GetAdjustedDateKey(DateTime.Now.AddDays(-i)))
            .Distinct()
            .ToList();
        return GetAwakeStatsForDates(days);
    }

    private (double wakeupTime, double bedTime) GetAwakeStatsForDates(List<DateTime> dates)
    {
        var minHours = new List<double>();
        var maxHours = new List<double>();

        foreach (var date in dates)
        {
            if (!dailyMessageTimes.TryGetValue(date, out var dayMessages) || dayMessages.Count == 0)
                continue;

            double earliest = dayMessages[0].Hour + (dayMessages[0].Minute / 60.0);
            double latest = dayMessages[dayMessages.Count - 1].Hour + (dayMessages[dayMessages.Count - 1].Minute / 60.0);
            minHours.Add(earliest);
            maxHours.Add(latest);
        }

        double avgMin = minHours.Count > 0 ? minHours.Average() : 0.0;
        double avgMax = maxHours.Count > 0 ? maxHours.Average() : 0.0;
        return (avgMin, avgMax);
    }

    private DateTime GetAdjustedDateKey(DateTime messageTime)
    {
        return messageTime.Hour < DayBoundaryHour
            ? messageTime.Date.AddDays(-1)
            : messageTime.Date;
    }
}
