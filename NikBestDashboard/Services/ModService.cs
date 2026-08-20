using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class ModService
{
    private readonly string _filePath;

    public ModService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "mods.json");
    }

    public List<ModItem> Load()
    {
        if (!File.Exists(_filePath)) return new List<ModItem>();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ModItem>>(json) ?? new List<ModItem>();
        }
        catch
        {
            return new List<ModItem>();
        }
    }

    public void Save(List<ModItem> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}