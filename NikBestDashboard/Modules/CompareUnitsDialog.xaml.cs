using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NikBestDashboard.Models;

namespace NikBestDashboard.Modules;

public partial class CompareUnitsDialog : Window
{
    public CompareUnitsDialog(List<Unit> units)
    {
        InitializeComponent();
        LoadComparison(units);
    }

    private void LoadComparison(List<Unit> units)
    {
        var data = new List<Dictionary<string, object>>();

        // Собираем характеристики
        var properties = new[]
        {
            new { Key = "Имя", Value = units.Select(u => u.Name) },
            new { Key = "Игра", Value = units.Select(u => u.Game) },
            new { Key = "Категория", Value = units.Select(u => u.Category) },
            new { Key = "❤ Здоровье", Value = units.Select(u => u.Health.ToString()) },
            new { Key = "⚔ Атака", Value = units.Select(u => u.Attack.ToString()) },
            new { Key = "🛡 Защита", Value = units.Select(u => u.Defense.ToString()) },
            new { Key = "🏃 Скорость", Value = units.Select(u => u.Speed.ToString("0.0")) },
            new { Key = "🎯 Тип атаки", Value = units.Select(u => u.AttackType) },
            new { Key = "📝 Описание", Value = units.Select(u => u.Description) }
        };

        // Заполняем данные для отображения
        var items = new List<ComparisonRow>();
        foreach (var prop in properties)
        {
            var values = prop.Value.ToList();
            // Добавляем пробелы, чтобы было видно, что юнитов меньше
            while (values.Count < units.Count)
            {
                values.Add("—");
            }
            items.Add(new ComparisonRow { Key = prop.Key, Values = values });
        }

        ComparisonGrid.ItemsSource = items;
    }

    public class ComparisonRow
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
    }
}