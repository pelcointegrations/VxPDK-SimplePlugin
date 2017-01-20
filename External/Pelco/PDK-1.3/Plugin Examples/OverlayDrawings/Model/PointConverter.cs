using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OverlayDrawings.Model
{
    class PointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() == 2)
            {
                if (string.IsNullOrEmpty(values[0].ToString()) || string.IsNullOrEmpty(values[1].ToString()))
                {
                    return null;
                }

                try
                {
                    return new Point
                    {
                        X = double.Parse(values[0].ToString()),
                        Y = double.Parse(values[1].ToString())
                    };
                } catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
