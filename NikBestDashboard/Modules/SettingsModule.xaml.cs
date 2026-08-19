using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        
        if (!System.IO.Directory.Exists(dataPath))
        {
            System.IO.Directory.CreateDirectory(dataPath);
        }
        
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

    private void BackupData_Click(object sender, RoutedEventArgs e)
{
    try
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var sourceFolder = System.IO.Path.Combine(appData, "NikBestDashboard");
        
        if (!System.IO.Directory.Exists(sourceFolder))
        {
            MessageBox.Show("Папка с данными не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var backupFolder = System.IO.Path.Combine(appData, "NikBestDashboard", "Backups");
        System.IO.Directory.CreateDirectory(backupFolder);
        
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
        var guid = Guid.NewGuid().ToString().Substring(0, 8);
        var backupPath = System.IO.Path.Combine(backupFolder, $"backup_{timestamp}_{guid}.zip");
        
        // ПОВТОРНЫЕ ПОПЫТКИ С ЗАДЕРЖКОЙ (до 5 раз)
        bool success = false;
        for (int attempt = 0; attempt < 5; attempt++)
        {
            try
            {
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
                
                System.Threading.Thread.Sleep(100 * (attempt + 1)); // 100, 200, 300, 400, 500 мс
                
                ZipFile.CreateFromDirectory(sourceFolder, backupPath);
                success = true;
                break;
            }
            catch (IOException)
            {
                if (attempt == 4) throw; // Если 5-я попытка не удалась
                System.Threading.Thread.Sleep(500); // Ждём полсекунды перед повторной попыткой
            }
        }
        
        if (success)
        {
            MessageBox.Show($"✅ Бэкап создан!\n\n📁 {backupPath}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"❌ Ошибка создания бэкапа:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
}