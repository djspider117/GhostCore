using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class DebugPassThroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
