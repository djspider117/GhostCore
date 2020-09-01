using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class IntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int k)
            {
                var bytes = BitConverter.GetBytes(k);
                return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }

            return Colors.Magenta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Color k)
            {
                return BitConverter.ToInt32(new byte[] { k.B, k.G, k.R, k.A }, 0);
            }

            return 0;
        }
    }
}
