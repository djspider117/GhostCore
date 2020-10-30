using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace GhostCore.UWP.Helpers
{
    public class ClipToBounds
    {
        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.RegisterAttached("ClipToBounds", typeof(bool), typeof(ClipToBounds), new PropertyMetadata(false, ClipToBoundsChanged));

        public static bool GetClipToBounds(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClipToBoundsProperty);
        }

        public static void SetClipToBounds(DependencyObject obj, bool value)
        {
            obj.SetValue(ClipToBoundsProperty, value);
        }

        private static void ClipToBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //use FrameworkElement because it is the highest abstraction that contains safe size
            //UIElement does not contain save size data
            var element = d as FrameworkElement;
            if (element != null)
            {
                element.Loaded += Element_Loaded;
                element.SizeChanged += Element_SizeChanged;
                element.Unloaded += Element_Unloaded;
            }
        }
        private static void Element_Loaded(object sender, RoutedEventArgs e)
        {
            ClipElement(sender as FrameworkElement);
        }

        private static void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipElement(sender as FrameworkElement);
        }

        private static void Element_Unloaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;

            element.SizeChanged -= Element_SizeChanged;
            element.Unloaded -= Element_Unloaded;
        }

        private static void ClipElement(FrameworkElement element)
        {
            if (GetClipToBounds(element))
            {
                var clip = new RectangleGeometry { Rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight) };
                element.Clip = clip;
            }
        }
    }

}
