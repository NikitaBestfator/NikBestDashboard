using System;

namespace NikBestDashboard.Models;

public class ModItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsLibrary { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public string CategoryIcon
    {
        get
        {
            return Category switch
            {
                "Оружие" => "⚔️",
                "Стройка" => "🏗️",
                "Техника" => "🔧",
                "Утилиты" => "🛠️",
                "Мобы" => "👾",
                "Библиотека" => "📚",
                _ => "📦"
            };
        }
    }
}