using System.Text.Json.Serialization;

namespace NikBestDashboard.Models;

public class Settings
{
    public string Theme { get; set; } = "Тёмная";
    public string DataPath { get; set; } = string.Empty;
    public string AppVersion { get; set; } = "1.0.0";
    public string Author { get; set; } = "NikBest";
}