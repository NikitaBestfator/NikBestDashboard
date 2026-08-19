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
            "Titles" => new TitlesModule(),  // <-- ЗАМЕНИЛИ ЗАГЛУШКУ
            "Units" => new UnitsModule(),
            "Schedule" => new ScheduleModule(),
            "Settings" => new SettingsModule(),
            _ => new TextBlock { Text = "Модуль не найден", FontSize = 24, Foreground = System.Windows.Media.Brushes.Red }
        };

        ModuleContent.Content = module;
    }
}