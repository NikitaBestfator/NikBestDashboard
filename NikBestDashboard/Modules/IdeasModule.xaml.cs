using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NikBestDashboard.Models;
using NikBestDashboard.Services;
using System.Windows.Media;

namespace NikBestDashboard.Modules;

public partial class IdeasModule : UserControl
{
    private List<Idea> _ideas;
    private readonly DataService _dataService = new DataService();

    public IdeasModule()
    {
        InitializeComponent();
        LoadIdeas();
    }

    private void LoadIdeas()
    {
        _ideas = _dataService.LoadIdeas();
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        if (_ideas == null) return;

        var filtered = _ideas.AsEnumerable();

        // Фильтр по категории
        if (CategoryFilterComboBox.SelectedItem is ComboBoxItem categoryItem && 
            categoryItem.Content.ToString() != "Все категории")
        {
            filtered = filtered.Where(i => i.Category == categoryItem.Content.ToString());
        }

        // Фильтр по статусу
        if (StatusFilterComboBox.SelectedItem is ComboBoxItem statusItem && 
            statusItem.Content.ToString() != "Все статусы")
        {
            filtered = filtered.Where(i => i.Status == statusItem.Content.ToString());
        }

        IdeasListBox.ItemsSource = filtered.ToList();
    }

    private void FilterChanged(object sender, SelectionChangedEventArgs e)
    {
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
        _dataService.SaveIdeas(_ideas);
        LoadIdeas();
        TitleTextBox.Text = "Новая идея...";
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
                    _dataService.SaveIdeas(_ideas);
                    LoadIdeas();
                }
            }
        }
    }
    private void OnFilterChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
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
            TitleTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136)); // #888888
        }
    }

    private void EditIdea_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Редактирование идей пока в разработке!", "Скоро будет", MessageBoxButton.OK, MessageBoxImage.Information);
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
}