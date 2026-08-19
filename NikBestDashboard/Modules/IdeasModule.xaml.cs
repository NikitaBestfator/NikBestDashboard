using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class IdeasModule : UserControl
{
    private List<Idea> _ideas = new List<Idea>();
    private readonly IdeaService _service = new IdeaService();
    private bool _isLoaded = false;

    public IdeasModule()
    {
        InitializeComponent();
        LoadIdeas();
        _isLoaded = true;
    }

    private void LoadIdeas()
    {
        _ideas = _service.Load();
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        if (_ideas == null) return;

        var filtered = _ideas.AsEnumerable();

        if (CategoryFilterComboBox.SelectedItem is ComboBoxItem categoryItem && 
            categoryItem.Content.ToString() != "Все категории")
        {
            filtered = filtered.Where(i => i.Category == categoryItem.Content.ToString());
        }

        if (StatusFilterComboBox.SelectedItem is ComboBoxItem statusItem && 
            statusItem.Content.ToString() != "Все статусы")
        {
            filtered = filtered.Where(i => i.Status == statusItem.Content.ToString());
        }

        IdeasListBox.ItemsSource = filtered.ToList();
    }

    private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyFilters();
    }

    private void AddIdea_Click(object sender, RoutedEventArgs e)
    {
        var title = TitleTextBox.Text.Trim();
        if (string.IsNullOrEmpty(title) || title == "Новая идея...")
        {
            MessageBox.Show("Введите название идеи!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var idea = new Idea
        {
            Title = title,
            Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Битвы",
            Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Средний",
            Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "В планах",
            CreatedAt = DateTime.Now
        };

        _ideas.Add(idea);
        _service.Save(_ideas);
        LoadIdeas();
        TitleTextBox.Text = "Новая идея...";
        TitleTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
    }

    private void DeleteIdea_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
        {
            var result = MessageBox.Show("Удалить эту идею?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var idea = _ideas.FirstOrDefault(i => i.Id == id);
                if (idea != null)
                {
                    _ideas.Remove(idea);
                    _service.Save(_ideas);
                    LoadIdeas();
                }
            }
        }
    }

    private void EditIdea_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
        {
            var idea = _ideas.FirstOrDefault(i => i.Id == id);
            if (idea == null) return;

            // Создаём диалог редактирования
            var dialog = new EditIdeaDialog(idea);
            dialog.Owner = Window.GetWindow(this);
        
            if (dialog.ShowDialog() == true)
            {
                // Сохраняем изменения
                _service.Save(_ideas);
                LoadIdeas();
            }
        }
    }

    private void RandomIdea_Click(object sender, RoutedEventArgs e)
    {
        if (_ideas.Count == 0)
        {
            MessageBox.Show("Нет идей! Добавь хотя бы одну.", "Нет идей", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var random = new Random();
        var randomIdea = _ideas[random.Next(_ideas.Count)];
        MessageBox.Show($"🎲 Случайная идея:\n\n{randomIdea.Title}\n\nКатегория: {randomIdea.Category}\nПриоритет: {randomIdea.Priority}\nСтатус: {randomIdea.Status}", "Идея дня!", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (TitleTextBox.Text == "Новая идея...")
        {
            TitleTextBox.Text = "";
            TitleTextBox.Foreground = Brushes.White;
        }
    }

    private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
        {
            TitleTextBox.Text = "Новая идея...";
            TitleTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }
}