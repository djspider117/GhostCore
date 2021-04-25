using GhostCore.Animations.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GhostCore.Animations.Editor.Controls
{
    public sealed partial class TimelineItemView : UserControl
    {
        public static readonly DependencyProperty LayerProperty =
            DependencyProperty.Register(nameof(Layer), typeof(LayerViewModel), typeof(TimelineItemView), new PropertyMetadata(null));
        
        public LayerViewModel Layer
        {
            get { return (LayerViewModel)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }

        public TimelineItemView()
        {
            InitializeComponent();
        }
    }
}
