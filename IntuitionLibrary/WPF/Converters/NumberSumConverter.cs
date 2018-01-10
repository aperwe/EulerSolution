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
    /// Returns sum of value + parameter.
    /// If value is not double, -1 is returned.
    /// By default (if parameter is not a double), simply the value is returned.
    /// </summary>
    public class NumberSumConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #region Preconditions
            if (!(value is double)) return -1;
            #endregion
            var wartoœæ = (double)value;
            if (parameter != null)
            {
                if (parameter is double)
                {
                    wartoœæ += (double)parameter;
                }
                if (parameter is string)
                {
                    double paramValue;
                    if (double.TryParse((string)parameter, out paramValue))
                    {
                        wartoœæ += paramValue;
                    }
                }
            }
            
            return wartoœæ;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
