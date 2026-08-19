using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class UnitService
{
    private readonly string _filePath;

    public UnitService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "units.json");
    }

    public List<Unit> Load()
    {
        if (!File.Exists(_filePath)) return new List<Unit>();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Unit>>(json) ?? new List<Unit>();
        }
        catch
        {
            return new List<Unit>();
        }
    }

    public void Save(List<Unit> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}