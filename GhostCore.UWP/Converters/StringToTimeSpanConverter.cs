using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class StringToTimeSpanConverter : IValueConverter
    {
        public static readonly StringToTimeSpanConverter Instance = new StringToTimeSpanConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString();
        }
    }
}
