using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PluginNs.Converters
{
    public class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is IValueConverter))
            {
                // No second converter is given as parameter:
                // Just invert and return, if boolean input value was provided
                if (value is bool)
                    return !(bool)value;
                else
                    return DependencyProperty.UnsetValue; // Fallback for non-boolean input values
            }
            else
            {
                // Second converter is provided:
                // Retrieve this converter...
                IValueConverter converter = (IValueConverter)parameter;

                if (value is bool)
                {
                    // ...and invert and then convert boolean input value!
                    bool input = !(bool)value;
                    return converter.Convert(input, targetType, null, culture);
                }
                else if (value == null)
                {
                    object input = new object();
                    return converter.Convert(input, targetType, null, culture);
                }
                else
                {
                    object input = null;
                    return converter.Convert(input, targetType, null, culture);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
