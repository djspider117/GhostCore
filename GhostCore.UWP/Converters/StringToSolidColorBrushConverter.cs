using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace GhostCore.UWP.Converters
{
    public class StringToSolidColorBrushConverter : IValueConverter
    {
        public static readonly StringToSolidColorBrushConverter Instance = new StringToSolidColorBrushConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
            {
                var solidColorBackground = (Color)XamlBindingHelper.ConvertValue(typeof(Color), s);
                return new SolidColorBrush(solidColorBackground);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is SolidColorBrush scb)
            {
                var color = scb.Color;
                FormattableString str = $"#{(byte)(color.A * 255):X2}{(byte)(color.R * 255):X2}{(byte)(color.G * 255):X2}{(byte)(color.B * 255):X2}";
                return str.ToString(CultureInfo.InvariantCulture);
            }

            return value;
        }
    }
}
