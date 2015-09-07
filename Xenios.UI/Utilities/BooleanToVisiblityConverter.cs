using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Xenios.UI.Utilities
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue.GetValueOrDefault(false) ?
                Visibility.Visible :
                Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var visiblity = value as Visibility?;

            return visiblity.GetValueOrDefault(Visibility.Collapsed) == Visibility.Visible;
        }
    }
}
