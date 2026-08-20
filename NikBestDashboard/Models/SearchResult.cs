namespace NikBestDashboard.Models;

public class SearchResult
{
    public string ModuleName { get; set; } = string.Empty;   // "Идеи", "Моды", "Юниты"...
    public string ModuleIcon { get; set; } = string.Empty;   // "📝", "📦", "📊"...
    public string Title { get; set; } = string.Empty;        // Название идеи/мода/юнита
    public string Category { get; set; } = string.Empty;     // Категория
    public string Tags { get; set; } = string.Empty;         // Теги (если есть)
    public string Description { get; set; } = string.Empty;  // Описание
    public string ModuleTag { get; set; } = string.Empty;    // "Ideas", "Mods", "Units"...
    public string Id { get; set; } = string.Empty;           // ID объекта
    public string NavigateCommand { get; set; } = string.Empty; // Для перехода
}