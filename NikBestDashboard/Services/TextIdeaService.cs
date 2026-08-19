using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class TextIdeaService
{
    private readonly string _filePath;

    public TextIdeaService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "text_ideas.json");
    }

    public List<TextIdea> Load()
    {
        if (!File.Exists(_filePath)) return new List<TextIdea>();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TextIdea>>(json) ?? new List<TextIdea>();
        }
        catch
        {
            return new List<TextIdea>();
        }
    }

    public void Save(List<TextIdea> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}