using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.UWP.ObjectModel
{
    public static class BindingModeConverter
    {
        public static Windows.UI.Xaml.Data.BindingMode ToXamlBindingMode(GhostCore.ObjectModel.BindingMode ghostCoreBindingMode)
        {
            switch (ghostCoreBindingMode)
            {
                case GhostCore.ObjectModel.BindingMode.OneWay:
                    return Windows.UI.Xaml.Data.BindingMode.OneWay;
                case GhostCore.ObjectModel.BindingMode.OneTime:
                    return Windows.UI.Xaml.Data.BindingMode.OneTime;
                case GhostCore.ObjectModel.BindingMode.TwoWay:
                    return Windows.UI.Xaml.Data.BindingMode.TwoWay;
                case GhostCore.ObjectModel.BindingMode.OneWayToSource:
                    return Windows.UI.Xaml.Data.BindingMode.TwoWay;
                case GhostCore.ObjectModel.BindingMode.None:
                    return Windows.UI.Xaml.Data.BindingMode.TwoWay;
                default:
                    return Windows.UI.Xaml.Data.BindingMode.TwoWay;
            }
        }

        public static GhostCore.ObjectModel.BindingMode ToGhostCoreBindingMode(Windows.UI.Xaml.Data.BindingMode xamlBindingMode)
        {
            switch (xamlBindingMode)
            {
                case Windows.UI.Xaml.Data.BindingMode.OneWay:
                    return GhostCore.ObjectModel.BindingMode.OneWay;
                case Windows.UI.Xaml.Data.BindingMode.OneTime:
                    return GhostCore.ObjectModel.BindingMode.OneTime;
                case Windows.UI.Xaml.Data.BindingMode.TwoWay:
                    return GhostCore.ObjectModel.BindingMode.TwoWay;
                default:
                    return GhostCore.ObjectModel.BindingMode.OneWay;
            }
        }
    }
}
