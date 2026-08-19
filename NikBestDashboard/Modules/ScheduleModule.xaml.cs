using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class ScheduleModule : UserControl
{
    private List<ScheduleItem> _items = new List<ScheduleItem>();
    private readonly ScheduleService _service = new ScheduleService();
    private bool _isLoaded = false;

    public ScheduleModule()
    {
        InitializeComponent();
        LoadItems();
        _isLoaded = true;
    }

    private void LoadItems()
    {
        _items = _service.Load();
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        // Если список ещё не загружен — выходим
        if (_items == null) return;

        var filtered = _items.AsEnumerable();

        if (StatusFilterComboBox.SelectedItem is ComboBoxItem statusItem && 
            statusItem.Content.ToString() != "Все статусы")
        {
            filtered = filtered.Where(i => i.Status == statusItem.Content.ToString());
        }

        var order = new Dictionary<string, int>
        {
            { "В планах", 0 },
            { "В работе", 1 },
            { "Готово", 2 },
            { "Опубликовано", 3 }
        };

        ScheduleListBox.ItemsSource = filtered
            .OrderBy(i => order.ContainsKey(i.Status) ? order[i.Status] : 999)
            .ToList();
    }

    private void FilterChanged(object sender, SelectionChangedEventArgs e)
    {
        // Игнорируем вызов, если модуль ещё не загружен
        if (!_isLoaded) return;
        ApplyFilter();
    }

    private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (TitleTextBox.Text == "Название видео...")
        {
            TitleTextBox.Text = "";
            TitleTextBox.Foreground = Brushes.White;
        }
    }

    private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
        {
            TitleTextBox.Text = "Название видео...";
            TitleTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        var title = TitleTextBox.Text.Trim();
        if (string.IsNullOrEmpty(title) || title == "Название видео...")
        {
            MessageBox.Show("Введите название видео!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var item = new ScheduleItem
        {
            Title = title,
            Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "В планах",
            Date = DateTime.Now
        };

        _items.Add(item);
        _service.Save(_items);
        LoadItems();
        TitleTextBox.Text = "Название видео...";
        TitleTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
    }

    private void DeleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
        {
            var result = MessageBox.Show("Удалить это видео из плана?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var item = _items.FirstOrDefault(i => i.Id == id);
                if (item != null)
                {
                    _items.Remove(item);
                    _service.Save(_items);
                    LoadItems();
                }
            }
        }
    }

    private void ChangeStatus_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null) return;

            var statuses = new[] { "В планах", "В работе", "Готово", "Опубликовано" };
            var currentIndex = Array.IndexOf(statuses, item.Status);
            var nextIndex = (currentIndex + 1) % statuses.Length;
            item.Status = statuses[nextIndex];

            _service.Save(_items);
            LoadItems();
        }
    }
}