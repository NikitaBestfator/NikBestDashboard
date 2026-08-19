using System;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class SettingsService
{
    private readonly string _filePath;

    public SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "settings.json");
    }

    public Settings Load()
    {
        if (!File.Exists(_filePath)) return new Settings();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        catch
        {
            return new Settings();
        }
    }

    public void Save(Settings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}