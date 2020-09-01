using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Helpers
{
    public class ZIndexBindingHelper
    {
        public static readonly DependencyProperty OrderIndexBindingPathProperty =
            DependencyProperty.RegisterAttached("ZIndexBindingPathProperty", typeof(string), typeof(ZIndexBindingHelper), new PropertyMetadata(null, BindingPathPropertyChanged));

        public static string GetOrderIndexBindingPathProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(OrderIndexBindingPathProperty);
        }

        public static void SetOrderIndexBindingPathProperty(DependencyObject obj, string value)
        {
            obj.SetValue(OrderIndexBindingPathProperty, value);
        }

        private static void BindingPathPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string propertyPath)
            {
                var binding = new Binding
                {
                    Path = new PropertyPath($"{propertyPath}"),
                    Mode = BindingMode.TwoWay,
                };

                if (propertyPath == "ZIndex")
                    BindingOperations.SetBinding(obj, Canvas.ZIndexProperty, binding);

            }
        }
    }
}
