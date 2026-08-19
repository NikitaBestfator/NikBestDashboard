using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class DataService
{
    private readonly string _dataPath;
    
    public DataService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _dataPath = Path.Combine(appFolder, "ideas.json");
    }

    public List<Idea> LoadIdeas()
    {
        if (!File.Exists(_dataPath))
            return new List<Idea>();

        try
        {
            var json = File.ReadAllText(_dataPath);
            return JsonSerializer.Deserialize<List<Idea>>(json) ?? new List<Idea>();
        }
        catch
        {
            return new List<Idea>();
        }
    }

    public void SaveIdeas(List<Idea> ideas)
    {
        var json = JsonSerializer.Serialize(ideas, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_dataPath, json);
    }
}