using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Converters.Chain.Resolvers
{
    public class ConverterResolver : DependencyObject, IConverterResolver
    {
        public string ConverterKey { get; set; }
        public ResourceDictionary Source { get; set; }

        public IValueConverter Resolve()
        {
            IValueConverter rv = null;
            try
            {
                rv = (IValueConverter)Source[ConverterKey];
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Could not resolve converter with specified key {ConverterKey}", ex);
            }

            return rv;
        }
    }
}