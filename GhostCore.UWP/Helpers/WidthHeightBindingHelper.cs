using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GhostCore.UWP.Helpers
{
    public class WidthHeightBindingHelper
    {
        public static string GetWidthPath(DependencyObject obj)
        {
            return (string)obj.GetValue(WidthPathProperty);
        }

        public static void SetWidthPath(DependencyObject obj, string value)
        {
            obj.SetValue(WidthPathProperty, value);
        }

        public static string GetHeightPath(DependencyObject obj)
        {
            return (string)obj.GetValue(HeightPathProperty);
        }

        public static void SetHeightPath(DependencyObject obj, string value)
        {
            obj.SetValue(HeightPathProperty, value);
        }

        public static readonly DependencyProperty HeightPathProperty =
            DependencyProperty.RegisterAttached("HeightPath", typeof(string), typeof(WidthHeightBindingHelper), new PropertyMetadata(null, OnHeightPathPropertyChanged));

        public static readonly DependencyProperty WidthPathProperty =
            DependencyProperty.RegisterAttached("WidthPath", typeof(string), typeof(WidthHeightBindingHelper), new PropertyMetadata(null, OnWidthPathPropertyChanged));

        private static void OnWidthPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string propertyPath)
            {
                var widthProp = FrameworkElement.WidthProperty;

                BindingOperations.SetBinding(d, widthProp, new Binding
                {
                    Path = new PropertyPath(propertyPath),
                    Source = (d as FrameworkElement).DataContext
                });
            }
        }
        private static void OnHeightPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is string propertyPath)
            {
                var hprop = FrameworkElement.HeightProperty;

                BindingOperations.SetBinding(d, hprop, new Binding
                {
                    Path = new PropertyPath(propertyPath),
                    Source = (d as FrameworkElement).DataContext
                });
            }
        }
    }
}
