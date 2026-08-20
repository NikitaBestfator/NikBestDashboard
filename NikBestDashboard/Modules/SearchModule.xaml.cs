using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class SearchModule : UserControl
{
    private readonly IdeaService _ideaService = new IdeaService();
    private readonly ModService _modService = new ModService();
    private readonly UnitService _unitService = new UnitService();
    private readonly TextIdeaService _textService = new TextIdeaService();
    private readonly ScheduleService _scheduleService = new ScheduleService();

    private List<SearchResult> _allResults = new List<SearchResult>();
    private MainWindow _mainWindow;

    public SearchModule()
    {
        InitializeComponent();
        _mainWindow = Window.GetWindow(this) as MainWindow;
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        PerformSearch();
    }

    private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            PerformSearch();
        }
    }

    private void PerformSearch()
    {
        try
        {
            if (ResultsList == null) return;

            var query = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(query) || query == "Введите слово для поиска...")
            {
                ResultsList.ItemsSource = null;
                ResultsCountText.Text = "";
                return;
            }

            var results = new List<SearchResult>();

            // ===== ПОИСК В ИДЕЯХ =====
            var ideas = _ideaService.Load();
            foreach (var idea in ideas)
            {
                var title = idea.Title ?? "";
                var category = idea.Category ?? "";
                var tags = idea.Tags ?? "";

                if (title.ToLower().Contains(query.ToLower()) ||
                    category.ToLower().Contains(query.ToLower()) ||
                    tags.ToLower().Contains(query.ToLower()))
                {
                    results.Add(new SearchResult
                    {
                        ModuleName = "Идеи",
                        ModuleIcon = "📝",
                        Title = title,
                        Category = category,
                        Tags = tags,
                        ModuleTag = "Ideas",
                        Id = idea.Id ?? ""
                    });
                }
            }

            // ===== ПОИСК В МОДАХ =====
            var mods = _modService.Load();
            foreach (var mod in mods)
            {
                var name = mod.Name ?? "";
                var category = mod.Category ?? "";
                var description = mod.Description ?? "";

                if (name.ToLower().Contains(query.ToLower()) ||
                    category.ToLower().Contains(query.ToLower()) ||
                    description.ToLower().Contains(query.ToLower()))
                {
                    results.Add(new SearchResult
                    {
                        ModuleName = "Моды",
                        ModuleIcon = "📦",
                        Title = name,
                        Category = category,
                        Description = description,
                        ModuleTag = "Mods",
                        Id = mod.Id ?? ""
                    });
                }
            }

            // ===== ПОИСК В ЮНИТАХ =====
            var units = _unitService.Load();
            foreach (var unit in units)
            {
                var name = unit.Name ?? "";
                var game = unit.Game ?? "";
                var category = unit.Category ?? "";

                if (name.ToLower().Contains(query.ToLower()) ||
                    game.ToLower().Contains(query.ToLower()) ||
                    category.ToLower().Contains(query.ToLower()))
                {
                    results.Add(new SearchResult
                    {
                        ModuleName = "Юниты",
                        ModuleIcon = "📊",
                        Title = name,
                        Category = $"{game} • {category}",
                        Description = $"❤{unit.Health} ⚔{unit.Attack}",
                        ModuleTag = "Units",
                        Id = unit.Id ?? ""
                    });
                }
            }

            // ===== ПОИСК В ТЕКСТАХ =====
            var texts = _textService.Load();
            foreach (var text in texts)
            {
                var textContent = text.Text ?? "";
                var topic = text.Topic ?? "";
                var type = text.Type ?? "";

                if (textContent.ToLower().Contains(query.ToLower()) ||
                    topic.ToLower().Contains(query.ToLower()) ||
                    type.ToLower().Contains(query.ToLower()))
                {
                    var displayTitle = textContent.Length > 50 ? textContent.Substring(0, 50) + "..." : textContent;
                    results.Add(new SearchResult
                    {
                        ModuleName = "Тексты",
                        ModuleIcon = "📋",
                        Title = displayTitle,
                        Category = $"{type} • {topic}",
                        Description = textContent,
                        ModuleTag = "Titles",
                        Id = text.Id ?? ""
                    });
                }
            }

            // ===== ПОИСК В ПЛАНИРОВЩИКЕ =====
            var schedules = _scheduleService.Load();
            foreach (var item in schedules)
            {
                var title = item.Title ?? "";

                if (title.ToLower().Contains(query.ToLower()))
                {
                    results.Add(new SearchResult
                    {
                        ModuleName = "План",
                        ModuleIcon = "📅",
                        Title = title,
                        Category = item.Status ?? "",
                        Description = item.Date?.ToString("dd.MM.yyyy") ?? "",
                        ModuleTag = "Schedule",
                        Id = item.Id ?? ""
                    });
                }
            }

            _allResults = results;
            ResultsList.ItemsSource = results;
            ResultsCountText.Text = $"🔍 Найдено: {results.Count} результатов";
        }
        catch (Exception ex)
        {
            if (ResultsCountText != null)
                ResultsCountText.Text = $"❌ Ошибка: {ex.Message}";
        }
    }

    private void NavigateToModule_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is SearchResult result)
        {
            if (_mainWindow == null)
            {
                _mainWindow = Window.GetWindow(this) as MainWindow;
                if (_mainWindow == null) return;
            }

            _mainWindow.LoadModule(result.ModuleTag);
        }
    }

    private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (SearchTextBox.Text == "Введите слово для поиска...")
        {
            SearchTextBox.Text = "";
            SearchTextBox.Foreground = Brushes.White;
        }
    }

    private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            SearchTextBox.Text = "Введите слово для поиска...";
            SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }
}