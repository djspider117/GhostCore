using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.ObjectModel.Converters
{
    public class StringToDoubleConverter : IValueConverter
    {
        public static StringToDoubleConverter Instance = new StringToDoubleConverter();

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            double val;
            if (value is double)
                return value;
            return double.TryParse((string)value,out val) ? val : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value.ToString();
        }
    }
}
