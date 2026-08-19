using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class IdeasModule : UserControl
{
    private List<Idea> _ideas;
    private readonly DataService _dataService = new DataService();

    public IdeasModule()
    {
        InitializeComponent();
        LoadIdeas();
    }

    private void LoadIdeas()
    {
        _ideas = _dataService.LoadIdeas();
        IdeasListBox.ItemsSource = _ideas;
    }

    private void AddIdea_Click(object sender, RoutedEventArgs e)
    {
        var title = TitleTextBox.Text.Trim();
        if (string.IsNullOrEmpty(title) || title == "Новая идея...")
        {
            MessageBox.Show("Введите название идеи!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var idea = new Idea
        {
            Title = title,
            Status = "В планах",
            CreatedAt = DateTime.Now
        };

        _ideas.Add(idea);
        _dataService.SaveIdeas(_ideas);
        LoadIdeas();
        TitleTextBox.Text = "Новая идея...";
    }
}