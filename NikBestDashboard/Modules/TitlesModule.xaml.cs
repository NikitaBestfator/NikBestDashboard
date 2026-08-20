using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        UpdateStats();
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
        else
        {
            variants.Add($"⚡ {topic} | Эпичная битва");
            variants.Add($"❓ {topic} — Кто выживет?");
            variants.Add($"🔥 {topic} | Полный разбор");
            variants.Add($"📌 {topic} — Смотри до конца!");
            variants.Add($"🎯 {topic} | Что произойдёт?");
        }

        return variants;
    }

    // ===== ШАБЛОНЫ =====

    private void ApplyTemplate_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string templateType)
        {
            var topic = TopicTextBox.Text.Trim();
            if (string.IsNullOrEmpty(topic) || topic == "Введите тему...")
            {
                MessageBox.Show("Сначала введите тему!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var templates = GetTemplates(templateType, topic);
            _generated = templates;
            GeneratedListBox.ItemsSource = _generated;
            NoResultsText.Visibility = _generated.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    private List<string> GetTemplates(string type, string topic)
    {
        return type switch
        {
            "Битва" => new List<string>
            {
                $"⚔️ {topic} — Кто выживет?",
                $"{topic}: Эпичное противостояние",
                $"БИТВА МАШИН: {topic}",
                $"{topic} — ЧЕЛЛЕНДЖ",
                $"Кто сильнее? {topic}"
            },
            "Челлендж" => new List<string>
            {
                $"🔥 {topic} — СМОГУ ЛИ Я?",
                $"{topic} за 5 минут",
                $"ЧЕЛЛЕНДЖ: {topic}",
                $"{topic} — Проверка на прочность",
                $"Смогу ли я пройти {topic}?"
            },
            "ТОП" => new List<string>
            {
                $"🏆 ТОП-5 {topic} в Майнкрафте",
                $"ТОП-10 {topic}, которые вас удивят",
                $"ТОП-5 лучших {topic}",
                $"ТОП-5 худших {topic}",
                $"ТОП-10 {topic} за всю историю"
            },
            "Эксперимент" => new List<string>
            {
                $"🧪 {topic} — Что будет?",
                $"Эксперимент: {topic}",
                $"Я проверил {topic}",
                $"Что если {topic}?",
                $"{topic} — НЕОЖИДАННЫЙ РЕЗУЛЬТАТ"
            },
            "Обзор" => new List<string>
            {
                $"📝 {topic} — Полный обзор",
                $"Что такое {topic}?",
                $"{topic} — Стоит ли пробовать?",
                $"Обзор {topic} за 5 минут",
                $"{topic} — ВСЯ ПРАВДА"
            },
            "Игры" => new List<string>
            {
                $"🎮 {topic} — Лучшие моменты",
                $"{topic} — Прохождение",
                $"ИГРАЕМ В {topic}",
                $"{topic} — Смешные моменты",
                $"НЕЛЕПЫЕ СМЕРТИ В {topic}"
            },
            "Код" => new List<string>
            {
                $"💻 {topic} — Как это работает?",
                $"Пишем {topic} на C#",
                $"Сделал {topic} за 10 минут",
                $"{topic} — Разбор кода",
                $"Как я сделал {topic}"
            },
            _ => new List<string> { $"📌 {topic} — Идея для видео" }
        };
    }

    private void AnalyzeTitle_Click(object sender, RoutedEventArgs e)
    {
        var topic = TopicTextBox.Text.Trim();
        if (string.IsNullOrEmpty(topic) || topic == "Введите тему...")
        {
            MessageBox.Show("Введите тему для анализа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var analysis = AnalyzeTitle(topic);
        MessageBox.Show(analysis, "📊 Анализ заголовка", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private string AnalyzeTitle(string title)
    {
        var result = new StringBuilder();
        result.AppendLine($"📊 Анализ заголовка:\n");
        result.AppendLine($"📝 \"{title}\"\n");

        int length = title.Length;
        string lengthStatus = length < 30 ? "❌ Слишком короткий (менее 30 символов)" :
                              length > 60 ? "❌ Слишком длинный (более 60 символов)" :
                              "✅ Оптимальная длина (30-60 символов)";
        result.AppendLine($"📏 Длина: {length} символов — {lengthStatus}");

        int emojiCount = title.Count(c => c >= 0x1F600 && c <= 0x1F64F || // смайлики
                                          c >= 0x1F300 && c <= 0x1F5FF || // символы
                                          c >= 0x2600 && c <= 0x27BF);   // прочие
        result.AppendLine($"😊 Эмодзи: {emojiCount} шт. {(emojiCount > 0 ? "✅ Отлично" : "❌ Добавьте эмодзи")}");

        int exclamationCount = title.Count(c => c == '!' || c == '!');
        result.AppendLine($"❗ Восклицательные знаки: {exclamationCount} шт. {(exclamationCount >= 1 ? "✅ Хорошо" : "❌ Добавьте эмоций")}");

        int questionCount = title.Count(c => c == '?' || c == '?');
        result.AppendLine($"❓ Вопросительные знаки: {questionCount} шт. {(questionCount >= 1 ? "✅ Интригует" : "❌ Добавьте интригу")}");

        string[] clickbaitWords = { "секрет", "шок", "невероятно", "топ", "лучший", "первый", "самый", "безумный", "эпичный", "ужас" };
        int clickbaitCount = clickbaitWords.Count(w => title.ToLower().Contains(w));
        result.AppendLine($"🔥 Кликбейтность: {clickbaitCount} кликбейтных слов {(clickbaitCount >= 2 ? "✅ Отлично для просмотров" : "❌ Добавьте кликбейтные слова")}");

        int score = 0;
        if (length >= 30 && length <= 60) score += 25;
        if (emojiCount > 0) score += 25;
        if (exclamationCount > 0) score += 20;
        if (questionCount > 0) score += 15;
        if (clickbaitCount >= 2) score += 15;

        result.AppendLine($"\n{new string('=', 30)}");
        result.AppendLine($"📊 Общая оценка: {score}/100");
        result.AppendLine(score >= 80 ? "🏆 Отличный заголовок!" :
                          score >= 60 ? "👍 Хороший заголовок, можно улучшить" :
                          "📝 Заголовок требует доработки");

        return result.ToString();
    }

    private void SafeCopyToClipboard(string text)
    {
        for (int attempt = 0; attempt < 3; attempt++)
        {
            try
            {
                Clipboard.SetText(text);
                MessageBox.Show("Скопировано!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                if (attempt == 2) throw;
                System.Threading.Thread.Sleep(100);
            }
        }
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
            SafeCopyToClipboard(text);
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
            SafeCopyToClipboard(item.Text);
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
    
    private void UpdateStats()
    {
        var total = _library.Count;
        var titles = _library.Count(i => i.Type == "Название");
        var covers = _library.Count(i => i.Type == "Обложка");
        var favorites = _library.Count(i => i.IsFavorite);

        TextTotal.Text = total.ToString();
        TextTitles.Text = titles.ToString();
        TextCovers.Text = covers.ToString();
        TextFavorites.Text = favorites.ToString();
    }
}