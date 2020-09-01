using GhostCore.UWP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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