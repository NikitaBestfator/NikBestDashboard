using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NikBestDashboard.Models;

namespace NikBestDashboard.Services;

public class ModService
{
    private readonly string _filePath;

    public ModService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "NikBestDashboard");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "mods.json");
        
        // Копируем базовые моды при первом запуске
        CopyDefaultModsIfNeeded();
    }

    private void CopyDefaultModsIfNeeded()
    {
        if (File.Exists(_filePath)) return;

        // Путь к исходному файлу в папке проекта
        var sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "mods.json");

        if (!File.Exists(sourcePath))
        {
            // Пробуем найти в папке проекта (для отладки)
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "mods.json");
            if (File.Exists(projectPath))
            {
                sourcePath = Path.GetFullPath(projectPath);
            }
            else
            {
                // Если файла нет — создаём базовый список
                var defaultMods = GetDefaultMods();
                var json = JsonSerializer.Serialize(defaultMods, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                return;
            }
        }

        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, _filePath);
        }
    }

    private List<ModItem> GetDefaultMods()
    {
        return new List<ModItem>
        {
            new ModItem { Name = "Super TNT", Category = "Оружие", Description = "Добавляет мощные виды TNT с различными эффектами", IsLibrary = false },
            new ModItem { Name = "TL Skin and Cape", Category = "Утилиты", Description = "Позволяет установить скин и плащ через TLauncher", IsLibrary = false },
            new ModItem { Name = "Carpenter's Blocks", Category = "Стройка", Description = "Позволяет создавать блоки с текстурой других блоков", IsLibrary = false },
            new ModItem { Name = "Custom NPC", Category = "Мобы", Description = "Создание собственных NPC с диалогами и квестами", IsLibrary = false },
            new ModItem { Name = "JourneyMap", Category = "Утилиты", Description = "Мини-карта с возможностью просмотра в реальном времени", IsLibrary = false },
            new ModItem { Name = "OptiFine", Category = "Утилиты", Description = "Улучшает производительность и графику игры", IsLibrary = false },
            new ModItem { Name = "Just Enough Items (JEI)", Category = "Утилиты", Description = "Показывает все рецепты крафта", IsLibrary = false }
        };
    }

    public List<ModItem> Load()
    {
        if (!File.Exists(_filePath)) return new List<ModItem>();

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ModItem>>(json) ?? new List<ModItem>();
        }
        catch
        {
            return new List<ModItem>();
        }
    }

    public void Save(List<ModItem> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}