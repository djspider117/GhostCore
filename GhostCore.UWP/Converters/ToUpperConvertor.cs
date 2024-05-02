using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class ToUpperConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string text)
                return text.ToUpperInvariant();

            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
