using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class TitlesModule : UserControl
{
    private readonly TextIdeaService _service = new TextIdeaService();
    private List<TextIdea> _library = new List<TextIdea>();
    private List<string> _generated = new List<string>();
    private bool _isLoaded = false;

    public TitlesModule()
    {
        InitializeComponent();
        LoadLibrary();
        _isLoaded = true;
    }

    private void LoadLibrary()
    {
        _library = _service.Load();
        ApplyLibraryFilter();
    }

    private void ApplyLibraryFilter()
    {
        var filtered = _library.AsEnumerable();

        var searchText = SearchTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(searchText) && searchText != "Поиск...")
        {
            filtered = filtered.Where(i => i.Text.ToLower().Contains(searchText.ToLower()) ||
                                           i.Topic.ToLower().Contains(searchText.ToLower()));
        }

        LibraryListBox.ItemsSource = filtered.OrderByDescending(i => i.IsFavorite).ThenByDescending(i => i.CreatedAt).ToList();
    }

    private void Generate_Click(object sender, RoutedEventArgs e)
    {
        var topic = TopicTextBox.Text.Trim();
        if (string.IsNullOrEmpty(topic) || topic == "Введите тему...")
        {
            MessageBox.Show("Введите тему!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Название";

        _generated = GenerateVariants(topic, type);
        GeneratedListBox.ItemsSource = _generated;

        NoResultsText.Visibility = _generated.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
    }

    private List<string> GenerateVariants(string topic, string type)
    {
        var variants = new List<string>();

        if (type == "Название")
        {
            variants.Add($"🔥 {topic} — ЭТО НУЖНО ВИДЕТЬ!");
            variants.Add($"❓ {topic}: А ТЫ ЗНАЛ?");
            variants.Add($"⚡ {topic} ЗА 5 МИНУТ!");
            variants.Add($"📌 {topic} — ПОЛНЫЙ РАЗБОР");
            variants.Add($"🎯 {topic}: ЧТО БУДЕТ, ЕСЛИ...");
        }
        else // Обложка
        {
            variants.Add($"⚡ {topic} | Эпичная битва");
            variants.Add($"❓ {topic} — Кто выживет?");
            variants.Add($"🔥 {topic} | Полный разбор");
            variants.Add($"📌 {topic} — Смотри до конца!");
            variants.Add($"🎯 {topic} | Что произойдёт?");
        }

        return variants;
    }

    private void SaveGenerated_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string text)
        {
            var type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Название";
            var topic = TopicTextBox.Text.Trim();

            var item = new TextIdea
            {
                Text = text,
                Type = type,
                Topic = topic,
                CreatedAt = DateTime.Now
            };

            _library.Add(item);
            _service.Save(_library);
            LoadLibrary();
            MessageBox.Show("Сохранено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void CopyGenerated_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string text)
        {
            try
            {
                Clipboard.SetText(text);
                MessageBox.Show("Скопировано!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось скопировать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void ToggleFavorite_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is TextIdea item)
        {
            var libraryItem = _library.FirstOrDefault(i => i.Id == item.Id);
            if (libraryItem != null)
            {
                libraryItem.IsFavorite = !libraryItem.IsFavorite;
                _service.Save(_library);
                LoadLibrary();
            }
        }
    }

    private void CopyLibrary_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is TextIdea item)
        {
            try
            {
                Clipboard.SetText(item.Text);
                MessageBox.Show("Скопировано!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось скопировать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void DeleteLibrary_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is TextIdea item)
        {
            var result = MessageBox.Show("Удалить этот текст?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var libraryItem = _library.FirstOrDefault(i => i.Id == item.Id);
                if (libraryItem != null)
                {
                    _library.Remove(libraryItem);
                    _service.Save(_library);
                    LoadLibrary();
                }
            }
        }
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyLibraryFilter();
    }

    private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (SearchTextBox.Text == "Поиск...")
        {
            SearchTextBox.Text = "";
            SearchTextBox.Foreground = Brushes.White;
        }
    }

    private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            SearchTextBox.Text = "Поиск...";
            SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void TopicTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (TopicTextBox.Text == "Введите тему...")
        {
            TopicTextBox.Text = "";
            TopicTextBox.Foreground = Brushes.White;
        }
    }

    private void TopicTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TopicTextBox.Text))
        {
            TopicTextBox.Text = "Введите тему...";
            TopicTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }
}