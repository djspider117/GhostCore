using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace GhostCore.UWP.Converters
{
    public class StringToFontFamilyConverter : IValueConverter
    {
        public static readonly StringToFontFamilyConverter Instance = new StringToFontFamilyConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
                return (FontFamily)XamlBindingHelper.ConvertValue(typeof(FontFamily), str);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is FontFamily ff)
                return ff.Source;

            return value;
        }
    }
}
