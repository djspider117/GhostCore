using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.ObjectModel.Converters
{
    public class DoubleToSingleConverter : IValueConverter
    {
        public static readonly DoubleToSingleConverter Instance = new DoubleToSingleConverter();

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == null)
                return 0F;

            return (Single)((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if (value == null)
                return 0D;

            return (double)value;
        }
    }
}
