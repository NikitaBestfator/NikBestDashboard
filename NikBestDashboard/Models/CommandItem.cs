namespace NikBestDashboard.Models;

public class CommandItem
{
    public string Command { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Базовые, Характеристики, Серверные
    public string Example { get; set; } = string.Empty;
}