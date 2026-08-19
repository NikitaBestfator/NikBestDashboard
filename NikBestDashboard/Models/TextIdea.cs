using System;

namespace NikBestDashboard.Models;

public class TextIdea
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = "Название"; // Название / Обложка
    public string Topic { get; set; } = string.Empty; // Тема, к которой относится
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsFavorite { get; set; } = false;
}