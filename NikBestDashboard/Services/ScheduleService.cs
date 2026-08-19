using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class ScheduleService
{
    private readonly string _filePath;

    public ScheduleService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "schedule.json");
    }

    public List<ScheduleItem> Load()
    {
        if (!File.Exists(_filePath)) return new List<ScheduleItem>();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ScheduleItem>>(json) ?? new List<ScheduleItem>();
        }
        catch
        {
            return new List<ScheduleItem>();
        }
    }

    public void Save(List<ScheduleItem> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}