using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Data;

namespace QBits.Intuition.WPF.Converters
{
    /// <summary>
    /// Converts a count of items to WPF Visibility.
    /// If value == 0 - returns Visibility.Collapsed.
    /// Otherwise returns Visibility.Visible.
    /// By default (if value cannot be parsed as int), Visibility.Visible is returned.
    /// Useful to hide a WPF control if some count (value) is 0 and show it when the count (value) is non-0.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var wartoœæ = (int)value;
                if (wartoœæ == 0) return Visibility.Collapsed;
                    return Visibility.Visible;
            }
            return Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
