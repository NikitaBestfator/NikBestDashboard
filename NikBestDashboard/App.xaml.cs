using System;
using System.Windows;
using NikBestDashboard.Services;

namespace NikBestDashboard;

public partial class App : Application
{
    public static string CurrentTheme { get; private set; } = "Тёмная";

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            var settingsService = new SettingsService();
            var settings = settingsService.Load();
            CurrentTheme = settings.Theme;
            ApplyTheme(CurrentTheme);
        }
        catch
        {
            ApplyTheme("Тёмная");
        }
    }

    public static void ApplyTheme(string theme)
    {
        CurrentTheme = theme;
        
        // Очищаем старые ресурсы темы
        var oldDictionaries = Application.Current.Resources.MergedDictionaries;
        oldDictionaries.Clear();

        // Загружаем новую тему
        var themeDict = new ResourceDictionary();
        var themeName = theme == "Светлая" ? "LightTheme" : "DarkTheme";
        var uri = new Uri($"/NikBestDashboard;component/Themes/{themeName}.xaml", UriKind.RelativeOrAbsolute);
        themeDict.Source = uri;
        
        Application.Current.Resources.MergedDictionaries.Add(themeDict);
    }
}