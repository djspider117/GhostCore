using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class UriToStringConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
            {
                return new Uri(s);
            }

            return null;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Uri u)
            {
                return u.ToString();
            }

            return null;
        }
    }

    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
            {
                return new Uri(s, UriKind.Relative);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Uri u)
            {
                return u.ToString();
            }

            return null;
        }
    }
}
