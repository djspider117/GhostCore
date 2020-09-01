using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Helpers
{
    public class CanvasCoordinatesBindingHelper
    {
        public static string GetLeftBindingPath(DependencyObject obj)
        {
            return (string)obj.GetValue(LeftBindingPathProperty);
        }

        public static void SetLeftBindingPath(DependencyObject obj, string value)
        {
            obj.SetValue(LeftBindingPathProperty, value);
        }

        public static string GetTopBindingPath(DependencyObject obj)
        {
            return (string)obj.GetValue(TopBindingPathProperty);
        }

        public static void SetTopBindingPath(DependencyObject obj, string value)
        {
            obj.SetValue(TopBindingPathProperty, value);
        }

        public static readonly DependencyProperty TopBindingPathProperty =
            DependencyProperty.RegisterAttached("TopBindingPath", typeof(string), typeof(CanvasCoordinatesBindingHelper), new PropertyMetadata(null, TopBindingPropertyChanged));

        public static readonly DependencyProperty LeftBindingPathProperty =
            DependencyProperty.RegisterAttached("LeftBindingPath", typeof(string), typeof(CanvasCoordinatesBindingHelper), new PropertyMetadata(null, LeftBindingPathPropertyChanged));

        private static void LeftBindingPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string propertyPath)
            {
                var prop = Canvas.LeftProperty;

                BindingOperations.SetBinding(d, prop, new Binding
                {
                    Path = new PropertyPath(propertyPath),
                    Source = (d as FrameworkElement).DataContext
                });
            }
        }
        private static void TopBindingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string propertyPath)
            {
                var hprop = Canvas.TopProperty;

                BindingOperations.SetBinding(d, hprop, new Binding
                {
                    Path = new PropertyPath(propertyPath),
                    Source = (d as FrameworkElement).DataContext
                });
            }
        }
    }


}
