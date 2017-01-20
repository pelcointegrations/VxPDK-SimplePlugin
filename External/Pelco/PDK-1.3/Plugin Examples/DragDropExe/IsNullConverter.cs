using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pelco.Phoenix.UI.Core.Converters
{
    public class IsNullConverter : IValueConverter
    {
        private bool _inverted = false;
        
        public bool Inverted { 
            get { return _inverted; }
            set { _inverted = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverted ? value != null : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
    }
}
