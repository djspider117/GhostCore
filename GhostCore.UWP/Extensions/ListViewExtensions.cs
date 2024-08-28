using GhostCore.UWP.Utils;
using Windows.UI.Xaml.Controls;

namespace GhostCore.UWP.Extensions
{
    public static class ListViewExtensions
    {
        public static Panel GetItemsPanel(this ListView itemsControl)
        {
            return UIHelper.GetVisualChild<ItemsStackPanel>(itemsControl);
        }
    }
}