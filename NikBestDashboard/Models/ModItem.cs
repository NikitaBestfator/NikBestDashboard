using System;

namespace NikBestDashboard.Models;

public class ModItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Библиотека, Мобы, Оружие, Стройка, Техника, Магия, Карты, Утилиты
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsLibrary { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}