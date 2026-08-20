using System.Windows;
using System.Windows.Controls;
using NikBestDashboard.Modules;
using System.Windows.Input;

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

    public void LoadModule(string moduleName)
    {
        object? module = moduleName switch
        {
            "Ideas" => new IdeasModule(),
            "Titles" => new TitlesModule(),
            "Units" => new UnitsModule(),
            "Schedule" => new ScheduleModule(),
            "Settings" => new SettingsModule(),
            "Mods" => new ModsModule(),
            "Search" => new SearchModule(),
            _ => new TextBlock { Text = "Модуль не найден", FontSize = 24, Foreground = System.Windows.Media.Brushes.Red }
        };

        ModuleContent.Content = module;
    }
    
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            switch (e.Key)
            {
                case Key.D1: LoadModule("Dashboard"); e.Handled = true; break;
                case Key.D2: LoadModule("Ideas"); e.Handled = true; break;
                case Key.D3: LoadModule("Titles"); e.Handled = true; break;
                case Key.D4: LoadModule("Units"); e.Handled = true; break;
                case Key.D5: LoadModule("Schedule"); e.Handled = true; break;
                case Key.D6: LoadModule("Mods"); e.Handled = true; break;
                case Key.D7: LoadModule("Settings"); e.Handled = true; break;
            }
        }
        base.OnKeyDown(e);
    }
    
    private void OpenTimer_Click(object sender, RoutedEventArgs e)
    {
        var timer = new RecordingTimer();
        timer.Owner = this;
        timer.ShowDialog();
    }
}