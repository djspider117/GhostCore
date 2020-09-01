using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public static NullToBoolConverter SharedInstance { get; private set; } = new NullToBoolConverter();

        public bool NullValue { get; set; } = false;
        public bool NormalValue { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return NullValue;

            return NormalValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
