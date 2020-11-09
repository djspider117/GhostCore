using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public int CollapseUnder { get; set; }
        public Visibility DefaultVisibility { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int i)
            {
                return i <= CollapseUnder ? Visibility.Collapsed : Visibility.Visible;
            }

            return DefaultVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
