using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Compendium.WPF.Converters
{
    public class IntToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the passed in value and convert it to a boolean - the as keyword returns a null if we can't convert to a boolean so the ?? allows us to set a default value if we get a null instead
            int hierarchy = (value as int?) ?? 0;
            // If the item IS a sub item, we want a larger Left Margin
            // Since the Margin Property expects a Thickness, that's what we need to return
            return new Thickness(hierarchy > 0 ? 12 * hierarchy : 0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
