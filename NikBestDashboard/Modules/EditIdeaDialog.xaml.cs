using System;
using System.Windows;
using NikBestDashboard.Models;

namespace NikBestDashboard.Modules;

public partial class EditIdeaDialog : Window
{
    public Idea Idea { get; private set; }

    public EditIdeaDialog(Idea idea)
    {
        InitializeComponent();
        Idea = idea;
        TitleTextBox.Text = idea.Title;

        // ===== ВСЕ КАТЕГОРИИ (КАК В ФИЛЬТРЕ) =====
        CategoryComboBox.ItemsSource = new[] 
        { 
            "Битвы", 
            "Постройки", 
            "Топы и списки", 
            "Эксперименты", 
            "Игры", 
            "Моды и код", 
            "Челленджи", 
            "Викторина", 
            "Код", 
            "Бравл Старс", 
            "TABS", 
            "Roblox" 
        };
        CategoryComboBox.SelectedItem = idea.Category;

        PriorityComboBox.ItemsSource = new[] { "Высокий", "Средний", "Низкий" };
        PriorityComboBox.SelectedItem = idea.Priority;

        StatusComboBox.ItemsSource = new[] { "В планах", "В работе", "Готово", "Опубликовано" };
        StatusComboBox.SelectedItem = idea.Status;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Idea.Title = TitleTextBox.Text;
        Idea.Category = CategoryComboBox.SelectedItem?.ToString() ?? Idea.Category;
        Idea.Priority = PriorityComboBox.SelectedItem?.ToString() ?? Idea.Priority;
        Idea.Status = StatusComboBox.SelectedItem?.ToString() ?? Idea.Status;

        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}