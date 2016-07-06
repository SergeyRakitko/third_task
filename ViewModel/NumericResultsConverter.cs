using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Model;

namespace ViewModel
{
    [ValueConversion(sourceType: typeof(NumericResults), targetType: typeof(string))]
    public class NumericResultsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NumericResults num_res = value as NumericResults;
            if (value != null)
                return "min = " + num_res.minimum_value.ToString("0.##") + ", max = " + num_res.maximum_value.ToString("0.##") + ", average = " + num_res.average_value.ToString("0.##");
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
