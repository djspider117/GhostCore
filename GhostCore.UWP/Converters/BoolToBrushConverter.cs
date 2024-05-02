using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace GhostCore.UWP.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush TrueColor { get; set; }
        public Brush FalseColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b && b)
                return TrueColor;

            return FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
