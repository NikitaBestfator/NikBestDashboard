using System;
using System.Collections.Generic;

namespace NikBestDashboard.Models;

public class Idea
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = "Битвы"; // Битвы, Лаборатория, Код, Другое
    public string Subcategory { get; set; } = string.Empty; // Обычные, Мутанты и т.д.
    public string Priority { get; set; } = "Средний"; // Высокий, Средний, Низкий
    public string Status { get; set; } = "В планах"; // В планах, В работе, Готово, Опубликовано
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<string> Tags { get; set; } = new List<string>();
}