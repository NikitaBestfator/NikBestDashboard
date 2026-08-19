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
        CopyDefaultUnitsIfNeeded();
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
    
    private void CopyDefaultUnitsIfNeeded()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        var targetPath = Path.Combine(appFolder, "units.json");

        if (File.Exists(targetPath)) return;

        // Путь к исходному файлу в папке проекта
        var sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "units.json");

        if (!File.Exists(sourcePath))
        {
            // Если файла нет в папке сборки, пробуем найти в папке проекта
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "units.json");
            if (File.Exists(projectPath))
            {
                sourcePath = Path.GetFullPath(projectPath);
            }
        }

        if (File.Exists(sourcePath))
        {
            Directory.CreateDirectory(appFolder);
            File.Copy(sourcePath, targetPath);
        }
    }
}