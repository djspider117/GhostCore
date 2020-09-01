using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GhostCore.UWP.Utils
{
    public static class UIHelper
    {
        public static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default;
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static async Task InvokeOnUI(this object obj, Action a)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a());
        }

        public static async Task InvokeOnUI(Action a)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a());
        }

        public static async Task<string> InputTextDialogAsync(string title)
        {
            var inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;

            var dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return null;
        }

        public static IAsyncOperation<ContentDialogResult> ShowContentDialog(string title, string content, string yes, string no)
        {
            var deleteSiteDlg = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = yes,
                CloseButtonText = no
            };

            return deleteSiteDlg.ShowAsync();
        }
    }

}
