using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class NullToColorConverter : IValueConverter
    {
        public string NormalColor { get; set; } = "#FFCF40";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? NormalColor : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
