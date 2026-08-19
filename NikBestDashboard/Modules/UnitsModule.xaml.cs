using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class UnitsModule : UserControl
{
    private List<Unit> _items = new List<Unit>();
    private readonly UnitService _service = new UnitService();
    private bool _isLoaded = false;

    public UnitsModule()
    {
        InitializeComponent();
        LoadItems();
        _isLoaded = true;
    }

    private void LoadItems()
    {
        _items = _service.Load();
        UpdateGameFilter();
        ApplyFilter();
    }

    private void UpdateGameFilter()
    {
        var games = _items.Select(i => i.Game).Distinct().ToList();
        GameFilterComboBox.Items.Clear();
        GameFilterComboBox.Items.Add(new ComboBoxItem { Content = "Все игры", IsSelected = true });
        foreach (var game in games.OrderBy(g => g))
        {
            GameFilterComboBox.Items.Add(new ComboBoxItem { Content = game });
        }
    }

    private void ApplyFilter()
    {
        if (_items == null) return;

        var filtered = _items.AsEnumerable();

        if (GameFilterComboBox.SelectedItem is ComboBoxItem gameItem && gameItem.Content.ToString() != "Все игры")
        {
            filtered = filtered.Where(i => i.Game == gameItem.Content.ToString());
        }

        if (CategoryFilterComboBox.SelectedItem is ComboBoxItem categoryItem && categoryItem.Content.ToString() != "Все категории")
        {
            filtered = filtered.Where(i => i.Category == categoryItem.Content.ToString());
        }

        var searchText = SearchTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(searchText) && searchText != "Поиск...")
        {
            filtered = filtered.Where(i => i.Name.ToLower().Contains(searchText.ToLower()));
        }

        UnitsListBox.ItemsSource = filtered.OrderBy(i => i.Name).ToList();
    }

    private void FilterChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyFilter();
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyFilter();
    }

    // ===== МЕТОД СРАВНЕНИЯ =====

    private void CompareUnits_Click(object sender, RoutedEventArgs e)
    {
        var selectedUnits = UnitsListBox.SelectedItems.Cast<Unit>().ToList();
        
        if (selectedUnits.Count < 2)
        {
            MessageBox.Show("Выберите хотя бы 2 юнита для сравнения (зажмите Ctrl или Shift)!", 
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new CompareUnitsDialog(selectedUnits);
        dialog.Owner = Window.GetWindow(this);
        dialog.ShowDialog();
        
        UnitsListBox.UnselectAll();
    }

    // ===== ОСТАЛЬНЫЕ МЕТОДЫ =====

    private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (SearchTextBox.Text == "Поиск...")
        {
            SearchTextBox.Text = "";
            SearchTextBox.Foreground = Brushes.White;
        }
    }

    private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            SearchTextBox.Text = "Поиск...";
            SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (NameTextBox.Text == "Имя юнита...")
        {
            NameTextBox.Text = "";
            NameTextBox.Foreground = Brushes.White;
        }
    }

    private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            NameTextBox.Text = "Имя юнита...";
            NameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void GameTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (GameTextBox.Text == "Игра...")
        {
            GameTextBox.Text = "";
            GameTextBox.Foreground = Brushes.White;
        }
    }

    private void GameTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(GameTextBox.Text))
        {
            GameTextBox.Text = "Игра...";
            GameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void HealthTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (HealthTextBox.Text == "HP")
        {
            HealthTextBox.Text = "";
            HealthTextBox.Foreground = Brushes.White;
        }
    }

    private void HealthTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HealthTextBox.Text))
        {
            HealthTextBox.Text = "HP";
            HealthTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void AttackTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (AttackTextBox.Text == "ATK")
        {
            AttackTextBox.Text = "";
            AttackTextBox.Foreground = Brushes.White;
        }
    }

    private void AttackTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AttackTextBox.Text))
        {
            AttackTextBox.Text = "ATK";
            AttackTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        var name = NameTextBox.Text.Trim();
        if (string.IsNullOrEmpty(name) || name == "Имя юнита...")
        {
            MessageBox.Show("Введите имя юнита!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var game = GameTextBox.Text.Trim();
        if (string.IsNullOrEmpty(game) || game == "Игра...")
        {
            MessageBox.Show("Введите название игры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        int health = 0;
        int attack = 0;
        int.TryParse(HealthTextBox.Text, out health);
        int.TryParse(AttackTextBox.Text, out attack);

        var unit = new Unit
        {
            Name = name,
            Game = game,
            Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Моб",
            Health = health,
            Attack = attack
        };

        _items.Add(unit);
        _service.Save(_items);
        LoadItems();
        ClearForm();
    }

    private void ClearForm()
    {
        NameTextBox.Text = "Имя юнита...";
        NameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        GameTextBox.Text = "Игра...";
        GameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        HealthTextBox.Text = "HP";
        HealthTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        AttackTextBox.Text = "ATK";
        AttackTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
    }

    private void DeleteItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
        {
            var result = MessageBox.Show("Удалить этого юнита?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var item = _items.FirstOrDefault(i => i.Id == id);
                if (item != null)
                {
                    _items.Remove(item);
                    _service.Save(_items);
                    LoadItems();
                }
            }
        }
    }
}