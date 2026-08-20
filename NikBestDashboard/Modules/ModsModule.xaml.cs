using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NikBestDashboard.Models;
using NikBestDashboard.Services;

namespace NikBestDashboard.Modules;

public partial class ModsModule : UserControl
{
    private List<ModItem> _items = new List<ModItem>();
    private readonly ModService _service = new ModService();
    private bool _isLoaded = false;
    private string _currentTab = "Mods";

    private List<CommandItem> _commands = new List<CommandItem>();

    public ModsModule()
    {
        InitializeComponent();
        LoadItems();
        LoadCommands();
        _isLoaded = true;
    }

    private void LoadItems()
    {
        _items = _service.Load();
        ApplyFilter();
    }

    private void LoadCommands()
    {
        _commands = new List<CommandItem>
        {
            new CommandItem { Command = "/tp", Description = "Телепортирует игрока к указанным координатам или другому игроку", Category = "Базовые", Example = "/tp @p 100 64 200" },
            new CommandItem { Command = "/give", Description = "Выдает указанное количество предметов игроку", Category = "Базовые", Example = "/give @s diamond 64" },
            new CommandItem { Command = "/time set", Description = "Устанавливает время суток", Category = "Базовые", Example = "/time set day" },
            new CommandItem { Command = "/gamemode", Description = "Устанавливает игровой режим", Category = "Базовые", Example = "/gamemode creative" },
            new CommandItem { Command = "/weather", Description = "Устанавливает погоду", Category = "Базовые", Example = "/weather clear" },
            new CommandItem { Command = "/clear", Description = "Очищает инвентарь игрока", Category = "Базовые", Example = "/clear @s" },
            new CommandItem { Command = "/fill", Description = "Заполняет область блоками", Category = "Базовые", Example = "/fill ~ ~ ~ ~10 ~10 ~10 stone" },
            new CommandItem { Command = "/spawn", Description = "Телепортирует на точку спавна", Category = "Базовые", Example = "/spawn" },
            new CommandItem { Command = "/spawnpoint", Description = "Устанавливает точку спавна в текущем месте", Category = "Базовые", Example = "/spawnpoint" },
            new CommandItem { Command = "/gamerule", Description = "Изменяет игровые правила", Category = "Базовые", Example = "/gamerule keepInventory true" },
            new CommandItem { Command = "/kill", Description = "Убивает игрока или сущность", Category = "Базовые", Example = "/kill @s" },
            new CommandItem { Command = "/setblock", Description = "Устанавливает блок в указанном месте", Category = "Базовые", Example = "/setblock ~ ~ ~ diamond_block" },
            new CommandItem { Command = "/help", Description = "Показывает справку по командам", Category = "Базовые", Example = "/help" },
            new CommandItem { Command = "/advancement", Description = "Управляет достижениями игрока", Category = "Характеристики", Example = "/advancement grant @s everything" },
            new CommandItem { Command = "/effect", Description = "Накладывает или убирает эффекты", Category = "Характеристики", Example = "/effect @s speed 60 2" },
            new CommandItem { Command = "/enchant", Description = "Зачаровывает предмет в руке", Category = "Характеристики", Example = "/enchant @s sharpness 5" },
            new CommandItem { Command = "/locate", Description = "Показывает координаты ближайшей структуры", Category = "Характеристики", Example = "/locate stronghold" },
            new CommandItem { Command = "/xp", Description = "Управляет опытом игрока", Category = "Характеристики", Example = "/xp add @s 100" },
            new CommandItem { Command = "/difficulty", Description = "Устанавливает уровень сложности", Category = "Характеристики", Example = "/difficulty hard" },
            new CommandItem { Command = "/summon", Description = "Призывает моба или предмет", Category = "Характеристики", Example = "/summon zombie ~ ~5 ~" },
            new CommandItem { Command = "/seed", Description = "Показывает сид (зерно) мира", Category = "Характеристики", Example = "/seed" },
            new CommandItem { Command = "/list", Description = "Показывает список игроков на сервере", Category = "Серверные", Example = "/list" },
            new CommandItem { Command = "/afk", Description = "Включает режим 'Нет на месте'", Category = "Серверные", Example = "/afk" },
            new CommandItem { Command = "/case", Description = "Показывает предметы в кейсе в руке", Category = "Серверные", Example = "/case" },
            new CommandItem { Command = "/compass", Description = "Показывает текущее направление", Category = "Серверные", Example = "/compass" },
            new CommandItem { Command = "/depth", Description = "Показывает положение относительно уровня моря", Category = "Серверные", Example = "/depth" },
            new CommandItem { Command = "/getpos", Description = "Показывает координаты персонажа", Category = "Серверные", Example = "/getpos" },
            new CommandItem { Command = "/ignore", Description = "Запрещает игроку писать вам сообщения", Category = "Серверные", Example = "/ignore PlayerName" },
            new CommandItem { Command = "/itemdb", Description = "Узнать ID предмета в руке", Category = "Серверные", Example = "/itemdb" },
            new CommandItem { Command = "/me", Description = "Отправить сообщение в чат от лица персонажа", Category = "Серверные", Example = "/me строит дом" },
            new CommandItem { Command = "/near", Description = "Показывает игроков рядом с вами", Category = "Серверные", Example = "/near" },
            new CommandItem { Command = "/pvp-on", Description = "Выключает защиту для новых игроков", Category = "Серверные", Example = "/pvp-on" },
            new CommandItem { Command = "/rtp", Description = "Телепортирует в случайное место на карте", Category = "Серверные", Example = "/rtp" },
            new CommandItem { Command = "/rules", Description = "Открывает правила сервера", Category = "Серверные", Example = "/rules" },
            new CommandItem { Command = "/tell", Description = "Отправляет личное сообщение игроку", Category = "Серверные", Example = "/tell PlayerName Привет!" },
            new CommandItem { Command = "/warp", Description = "Телепортирует в точку варпа", Category = "Серверные", Example = "/warp spawn" },
            new CommandItem { Command = "/sethome", Description = "Устанавливает точку дома", Category = "Серверные", Example = "/sethome" },
            new CommandItem { Command = "/home", Description = "Телепортирует домой", Category = "Серверные", Example = "/home" },
            new CommandItem { Command = "/deletehome", Description = "Удаляет точку дома", Category = "Серверные", Example = "/deletehome" },
            new CommandItem { Command = "/region claim", Description = "Создать приват территории", Category = "Серверные", Example = "/region claim MyBase" },
            new CommandItem { Command = "/region addmember", Description = "Добавить игрока в приват", Category = "Серверные", Example = "/region addmember MyBase PlayerName" },
            new CommandItem { Command = "/region remove", Description = "Удалить игрока из привата", Category = "Серверные", Example = "/region remove MyBase PlayerName" },
            new CommandItem { Command = "//hpos1", Description = "Установить первую точку привата", Category = "Серверные", Example = "//hpos1" },
            new CommandItem { Command = "//hpos2", Description = "Установить вторую точку привата", Category = "Серверные", Example = "//hpos2" },
            new CommandItem { Command = "//wand", Description = "Выдать деревянный топор для привата", Category = "Серверные", Example = "//wand" }
        };

        CommandsListBox.ItemsSource = _commands;
    }

    private void SwitchTab_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string tab)
        {
            _currentTab = tab;

            ModsTabButton.Background = tab == "Mods" ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C4DFF")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444466"));
            CommandsTabButton.Background = tab == "Commands" ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C4DFF")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444466"));

            ModsPanel.Visibility = tab == "Mods" ? Visibility.Visible : Visibility.Collapsed;
            CommandsPanel.Visibility = tab == "Commands" ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    // ===== МЕТОДЫ МОДОВ =====

    private void ApplyFilter()
    {
        if (_items == null) return;

        var filtered = _items.AsEnumerable();

        if (CategoryFilterComboBox.SelectedItem is ComboBoxItem categoryItem &&
            categoryItem.Content.ToString() != "Все категории")
        {
            filtered = filtered.Where(i => i.Category == categoryItem.Content.ToString());
        }

        if (ShowLibrariesCheckBox.IsChecked == false)
        {
            filtered = filtered.Where(i => !i.IsLibrary);
        }

        var searchText = SearchTextBox.Text.Trim();
        if (!string.IsNullOrEmpty(searchText) && searchText != "Поиск...")
        {
            filtered = filtered.Where(i => i.Name.ToLower().Contains(searchText.ToLower()) ||
                                           i.Description.ToLower().Contains(searchText.ToLower()));
        }

        ModsListBox.ItemsSource = filtered.OrderBy(i => i.Name).ToList();
    }

    // ===== ИСПРАВЛЕНО: разные имена =====
    private void FilterChanged_CheckBox(object sender, RoutedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyFilter();
    }

    private void FilterChanged_ComboBox(object sender, SelectionChangedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyFilter();
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isLoaded) return;
        ApplyFilter();
    }

    private void AddMod_Click(object sender, RoutedEventArgs e)
    {
        var name = NameTextBox.Text.Trim();
        if (string.IsNullOrEmpty(name) || name == "Название мода...")
        {
            MessageBox.Show("Введите название мода!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var mod = new ModItem
        {
            Name = name,
            Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Мобы",
            Description = "Описание мода...",
            IsLibrary = IsLibraryCheckBox.IsChecked == true
        };

        _items.Add(mod);
        _service.Save(_items);
        LoadItems();
        ClearForm();
    }

    private void ClearForm()
    {
        NameTextBox.Text = "Название мода...";
        NameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        IsLibraryCheckBox.IsChecked = false;
    }

    private void EditMod_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Редактирование модов пока в разработке!", "Скоро будет", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void DeleteMod_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
        {
            var result = MessageBox.Show("Удалить этот мод?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

    // ===== МЕТОДЫ КОМАНД =====

    private void CommandSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = CommandSearchTextBox.Text.Trim();
        if (string.IsNullOrEmpty(searchText) || searchText == "Поиск...")
        {
            CommandsListBox.ItemsSource = _commands;
            return;
        }

        var filtered = _commands.Where(c =>
            c.Command.ToLower().Contains(searchText.ToLower()) ||
            c.Description.ToLower().Contains(searchText.ToLower()) ||
            c.Category.ToLower().Contains(searchText.ToLower())
        ).ToList();

        CommandsListBox.ItemsSource = filtered;
    }

    private void CopyCommand_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string command)
        {
            try
            {
                Clipboard.SetText(command);
                MessageBox.Show($"Команда скопирована: {command}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось скопировать команду.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // ===== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =====

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
        if (NameTextBox.Text == "Название мода...")
        {
            NameTextBox.Text = "";
            NameTextBox.Foreground = Brushes.White;
        }
    }

    private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            NameTextBox.Text = "Название мода...";
            NameTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }

    private void CommandSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (CommandSearchTextBox.Text == "Поиск...")
        {
            CommandSearchTextBox.Text = "";
            CommandSearchTextBox.Foreground = Brushes.White;
        }
    }

    private void CommandSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CommandSearchTextBox.Text))
        {
            CommandSearchTextBox.Text = "Поиск...";
            CommandSearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        }
    }
}