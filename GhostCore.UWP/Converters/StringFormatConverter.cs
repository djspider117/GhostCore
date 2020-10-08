using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public string StringFormat { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!string.IsNullOrEmpty(StringFormat) && value != null)
                return string.Format(StringFormat, value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
