using System;
using System.Collections.Generic;

namespace NikBestDashboard.Models;

public class Idea
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = "Битвы";
    public string Priority { get; set; } = "Средний";
    public string Status { get; set; } = "В планах";
    public bool IsArchived { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}