using System;
using System.Windows;

namespace NikBestDashboard.Modules;

public partial class AddToScheduleDialog : Window
{
    public string Title { get; private set; }
    public DateTime? SelectedDate { get; private set; }

    public AddToScheduleDialog(string ideaTitle)
    {
        InitializeComponent();
        Title = ideaTitle;
        TitleText.Text = ideaTitle;
        SelectedDate = DateTime.Now;
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedDate = DateTime.Now;
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}