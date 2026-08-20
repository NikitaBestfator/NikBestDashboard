using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class SettingsModule : UserControl
{
    private readonly SettingsService _service = new SettingsService();
    private Settings _settings;

    public SettingsModule()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        _settings = _service.Load();

        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dataPath = System.IO.Path.Combine(appData, "NikBestDashboard");
        DataPathText.Text = dataPath;
        VersionText.Text = _settings.AppVersion;
    }

    private void OpenDataFolder_Click(object sender, RoutedEventArgs e)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dataPath = System.IO.Path.Combine(appData, "NikBestDashboard");

        try
        {
            Process.Start("explorer.exe", dataPath);
        }
        catch
        {
            MessageBox.Show("Не удалось открыть папку с данными.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
        catch
        {
            MessageBox.Show("Не удалось открыть ссылку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RestartApp_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Перезапустить приложение?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Process.GetCurrentProcess().MainModule?.FileName,
                UseShellExecute = true
            };
            Process.Start(startInfo);
            Application.Current.Shutdown();
        }
    }
}