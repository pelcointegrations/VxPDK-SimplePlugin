using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PluginNs.Converters
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = new SolidColorBrush(Colors.Yellow);
            if (value == null) return brush;

            bool val = (bool)value;
            brush = val ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Red);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
