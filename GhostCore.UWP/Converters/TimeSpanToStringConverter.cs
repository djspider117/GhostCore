using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public static readonly TimeSpanToStringConverter Instance = new TimeSpanToStringConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
            {
                if (TimeSpan.TryParse(s, out var result))
                {
                    return result;
                }
            }

            return new TimeSpan(0);
        }
    }
}
