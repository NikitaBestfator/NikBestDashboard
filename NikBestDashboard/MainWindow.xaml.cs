using System.Windows;
using System.Windows.Controls;
using NikBestDashboard.Modules;

namespace NikBestDashboard;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadModule("Ideas");
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string moduleName)
        {
            LoadModule(moduleName);
        }
    }

    private void LoadModule(string moduleName)
    {
        object? module = moduleName switch
        {
            "Ideas" => new IdeasModule(),
            "Titles" => new TextBlock { Text = "📋 Генератор текста (в разработке)", FontSize = 24, Foreground = System.Windows.Media.Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
            "Mobs" => new TextBlock { Text = "📊 Управление мобами (в разработке)", FontSize = 24, Foreground = System.Windows.Media.Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
            "Schedule" => new TextBlock { Text = "📅 Планировщик видео (в разработке)", FontSize = 24, Foreground = System.Windows.Media.Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
            "Settings" => new TextBlock { Text = "⚙️ Настройки (в разработке)", FontSize = 24, Foreground = System.Windows.Media.Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
            _ => new TextBlock { Text = "Модуль не найден", FontSize = 24, Foreground = System.Windows.Media.Brushes.Red }
        };

        ModuleContent.Content = module;
    }
}