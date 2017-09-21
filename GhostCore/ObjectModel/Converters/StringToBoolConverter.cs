using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.ObjectModel.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            bool rv = false;

            if (value == null)
                return false;

            bool.TryParse(value.ToString(), out rv);

            return rv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if (value is bool)
            {
                return value.ToString();
            }

            return value;
        }
    }
}
