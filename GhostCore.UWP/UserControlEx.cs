using GhostCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GhostCore.UWP
{
    public class UserControlEx : UserControl
    {
        private bool _isLoaded;

        public UserControlEx()
        {
            Loaded += UserControlEx_Loaded;
        }

        private async void UserControlEx_Loaded(object sender, RoutedEventArgs e)
        {

            Loaded -= UserControlEx_Loaded;
            try
            {
                await OnLoaded(sender, e, _isLoaded);
                _isLoaded = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }

            Unloaded += UserControlEx_Unloaded;
        }
        private async void UserControlEx_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= UserControlEx_Unloaded;

            try
            {
                await OnUnloaded(sender, e);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }


        protected virtual Task OnLoaded(object sender, RoutedEventArgs e, bool wasLoadedPreviously)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnUnloaded(object sender, RoutedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
