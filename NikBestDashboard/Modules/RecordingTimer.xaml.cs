using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace NikBestDashboard.Modules;

public partial class RecordingTimer : Window
{
    private DispatcherTimer _timer;
    private int _totalSeconds;
    private int _remainingSeconds;
    private bool _isPaused = false;
    private bool _isRunning = false;

    public RecordingTimer()
    {
        InitializeComponent();
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        UpdateDisplay(0);
    }

    private void StartTimer_Click(object sender, RoutedEventArgs e)
    {
        if (_isRunning && !_isPaused) return;

        if (TimeComboBox.SelectedItem is ComboBoxItem item && item.Tag != null)
        {
            _totalSeconds = Convert.ToInt32(item.Tag);
        }
        else
        {
            _totalSeconds = 60;
        }

        if (_isPaused)
        {
            _isPaused = false;
            _isRunning = true;
            _timer.Start();
            StatusText.Text = "⏳ Идёт запись...";
            StartButton.Content = "⏹️ Стоп";
            PauseButton.IsEnabled = true;
            PauseButton.Content = "⏸️ Пауза";
            return;
        }

        _remainingSeconds = _totalSeconds;
        _isRunning = true;
        _isPaused = false;
        _timer.Start();
        StartButton.Content = "⏹️ Стоп";
        PauseButton.IsEnabled = true;
        StatusText.Text = "⏳ Идёт запись...";
        TimeComboBox.IsEnabled = false;

        UpdateDisplay(_remainingSeconds);
    }

    private void PauseTimer_Click(object sender, RoutedEventArgs e)
    {
        if (!_isRunning) return;

        if (_isPaused)
        {
            _isPaused = false;
            _timer.Start();
            PauseButton.Content = "⏸️ Пауза";
            StatusText.Text = "⏳ Идёт запись...";
            StartButton.IsEnabled = true;
        }
        else
        {
            _isPaused = true;
            _timer.Stop();
            PauseButton.Content = "▶️ Продолжить";
            StatusText.Text = "⏸️ На паузе";
            StartButton.IsEnabled = false;
        }
    }

    private void ResetTimer_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        _isRunning = false;
        _isPaused = false;
        _remainingSeconds = 0;
        StartButton.Content = "▶️ Старт";
        StartButton.IsEnabled = true;
        PauseButton.IsEnabled = false;
        PauseButton.Content = "⏸️ Пауза";
        TimeComboBox.IsEnabled = true;
        StatusText.Text = "🔄 Сброшено";
        UpdateDisplay(0);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (_isPaused) return;

        _remainingSeconds--;

        if (_remainingSeconds <= 0)
        {
            _timer.Stop();
            _isRunning = false;
            StatusText.Text = "✅ Запись завершена!";
            StartButton.Content = "▶️ Старт";
            PauseButton.IsEnabled = false;
            TimeComboBox.IsEnabled = true;
            UpdateDisplay(0);

            PlayBeep(1000, 500);
            System.Threading.Thread.Sleep(200);
            PlayBeep(1000, 500);
            return;
        }

        UpdateDisplay(_remainingSeconds);

        if (_remainingSeconds <= 3 && _remainingSeconds > 0)
        {
            PlayBeep(800, 150);
        }
    }

    private void UpdateDisplay(int seconds)
    {
        var minutes = seconds / 60;
        var secs = seconds % 60;
        MinutesDisplay.Text = minutes.ToString("D2");
        SecondsDisplay.Text = secs.ToString("D2");

        if (seconds == 0)
        {
            MinutesDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEFF"));
            SecondsDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEFF"));
        }
        else if (seconds <= 10)
        {
            MinutesDisplay.Foreground = new SolidColorBrush(Colors.Red);
            SecondsDisplay.Foreground = new SolidColorBrush(Colors.Red);
        }
        else if (seconds <= 30)
        {
            MinutesDisplay.Foreground = new SolidColorBrush(Colors.Orange);
            SecondsDisplay.Foreground = new SolidColorBrush(Colors.Orange);
        }
        else
        {
            MinutesDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
            SecondsDisplay.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
        }
    }

    private void PlayBeep(int frequency = 800, int duration = 300)
    {
        try
        {
            Console.Beep(frequency, duration);
        }
        catch
        {
            // Игнорируем ошибки звука
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        Close();
    }
}