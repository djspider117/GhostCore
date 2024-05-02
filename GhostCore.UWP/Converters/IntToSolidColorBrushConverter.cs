using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace GhostCore.UWP.Converters
{
    public class IntToSolidColorBrushConverter : IValueConverter
    {
        public static readonly IntToSolidColorBrushConverter Instance = new IntToSolidColorBrushConverter();

        public SolidColorBrush NotFoundColor { get; set; } = new SolidColorBrush(Colors.Magenta);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int val)
            {
                byte[] bytes = BitConverter.GetBytes(val);
                return new SolidColorBrush(Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]));
            }

            return NotFoundColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToSolidColorConverter : IValueConverter
    {
        public Color NotFoundColor { get; set; } = Colors.Magenta;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int val)
            {
                byte[] bytes = BitConverter.GetBytes(val);
                return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }

            return NotFoundColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
