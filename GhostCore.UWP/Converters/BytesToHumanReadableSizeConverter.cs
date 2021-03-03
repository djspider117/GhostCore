using System;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters
{
    public class BytesToHumanReadableSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                string[] suffixNames = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
                var counter = 0;

                decimal.TryParse(value.ToString(), out var dValue);

                while (Math.Round(dValue / 1024) >= 1)
                {
                    dValue /= 1024;
                    counter++;
                }

                return $"{dValue:n1} {suffixNames[counter]}";
            }
            catch
            {
                //catch and handle the exception
                return string.Empty;
            }
		}

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}