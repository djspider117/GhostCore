using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class BoolToDoubleConverter : IValueConverter
    {
        public double TrueValue { get; set; } = 1;
        public double FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            {
                if (b)
                    return TrueValue;

                return FalseValue;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double d)
            {
                if (d == TrueValue)
                    return true;

                return false;
            }

            return value;
        }
    }
}
