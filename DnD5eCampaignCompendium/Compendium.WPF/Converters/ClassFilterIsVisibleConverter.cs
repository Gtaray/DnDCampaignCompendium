using Assisticant;
using Compendium.Model.Models;
using Compendium.WPF.ViewModels.Common;
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
    public class ClassFilterIsVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            FilterFlagViewModel<ClassModel> filter = ForView.Unwrap<FilterFlagViewModel<ClassModel>>(values[0]);
            bool showAllClasses = (bool)values[1];

            return showAllClasses || filter.Filter.HasSpells ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
