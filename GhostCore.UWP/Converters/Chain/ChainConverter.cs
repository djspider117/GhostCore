using GhostCore.UWP.Converters.Chain.Resolvers;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters.Chain
{
    public class ChainConverter : IValueConverter
    {
        public ObservableCollection<IValueConverter> Chain { get; private set; }
        public ObservableCollection<IConverterResolver> Resolvers { get; set; }

        public ChainConverter()
        {
            Chain = new ObservableCollection<IValueConverter>();
            Resolvers = new ObservableCollection<IConverterResolver>();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            object chainValue = value;

            foreach (var converter in Chain)
            {
                chainValue = converter.Convert(chainValue, targetType, parameter, language);
            }

            foreach (var resolver in Resolvers)
            {
                var conv = resolver.Resolve();
                chainValue = conv.Convert(chainValue, targetType, parameter, language);
            }
            return chainValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            object chainValue = value;

            foreach (var converter in Chain)
            {
                chainValue = converter.ConvertBack(chainValue, targetType, parameter, language);
            }
            foreach (var resolver in Resolvers)
            {
                var conv = resolver.Resolve();
                chainValue = conv.ConvertBack(chainValue, targetType, parameter, language);
            }
            return chainValue;
        }
    }
}