using System;
using System.Windows.Data;

namespace PluginNs.Converters
{
    public class TimeOrDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string formattedDate = string.Empty;
            if (value == null) return formattedDate;

            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, DateTimeKind.Local);
            DateTime date = (DateTime)value;
            date = date.ToLocalTime();

            formattedDate = date < today ? date.ToString("MMM dd", culture) : date.ToString("HH:mm:ss", culture);
            return formattedDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
