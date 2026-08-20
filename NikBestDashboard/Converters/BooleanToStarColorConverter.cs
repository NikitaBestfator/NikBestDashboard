using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NikBestDashboard.Converters;

public class BooleanToStarColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
        {
            return new SolidColorBrush(Colors.Gold);
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}