using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public static NullToVisibilityConverter SharedInstance { get; private set; } = new NullToVisibilityConverter();

        public Visibility NullVisibility { get; set; } = Visibility.Collapsed;
        public Visibility NormalVisibility { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return NullVisibility;

            return NormalVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
