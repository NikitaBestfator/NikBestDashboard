using System;

namespace NikBestDashboard.Models;

public class ScheduleItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "В планах"; // В планах, В работе, Готово, Опубликовано
    public DateTime? Date { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}