using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GhostCore.UWP
{

    public static class DependencyPropertyHelper
    {
        public static DependencyProperty Register<T>(string name, object defaultValue = null, string callbackName = null) where T : class
        {
            PropertyMetadata meta = null;
            var type = typeof(T);

            if (callbackName != null)
            {
                var met = type.GetMethod(callbackName);

                meta = new PropertyMetadata(defaultValue, (d, e) => met.Invoke(d, new object[] { e }));
            }
            else
            {
                meta = new PropertyMetadata(defaultValue);
            }

            var prop = type.GetProperty(name);

            return DependencyProperty.Register(name, prop.PropertyType, typeof(T), meta);
        }
    }
}
