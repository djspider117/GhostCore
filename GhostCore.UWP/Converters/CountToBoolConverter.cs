using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class CountToBoolConverter : IValueConverter
    {
        public int TrueWhenOver { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int i)
            {
                return i > TrueWhenOver;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}