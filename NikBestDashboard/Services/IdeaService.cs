using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class IdeaService
{
    private readonly string _filePath;

    public IdeaService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "ideas.json");
        
        // Копируем базовые данные при первом запуске
        CopyDefaultIdeasIfNeeded();
    }

    private void CopyDefaultIdeasIfNeeded()
    {
        if (File.Exists(_filePath)) return;

        var sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ideas.json");
        
        if (!File.Exists(sourcePath))
        {
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "ideas.json");
            if (File.Exists(projectPath))
            {
                sourcePath = Path.GetFullPath(projectPath);
            }
        }

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, _filePath);
        }
    }

    public List<Idea> Load()
    {
        if (!File.Exists(_filePath)) return new List<Idea>();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Idea>>(json) ?? new List<Idea>();
        }
        catch
        {
            return new List<Idea>();
        }
    }

    public void Save(List<Idea> ideas)
    {
        var json = JsonSerializer.Serialize(ideas, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}