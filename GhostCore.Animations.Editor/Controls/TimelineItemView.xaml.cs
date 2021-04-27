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

        public static readonly DependencyProperty IsLeftOverflowingProperty =
            DependencyProperty.Register(nameof(IsLeftOverflowing), typeof(bool), typeof(TimelineItemView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsRightOverflowingProperty =
            DependencyProperty.Register(nameof(IsRightOverflowing), typeof(bool), typeof(TimelineItemView), new PropertyMetadata(false));

        public static readonly DependencyProperty LeftOverflowIndicatorOffsetProperty =
            DependencyProperty.Register(nameof(LeftOverflowIndicatorOffset), typeof(double), typeof(TimelineItemView), new PropertyMetadata(0, (d, e) => (d as TimelineItemView).OnLeftOverflowIndicatorChanged(e)));

        public static readonly DependencyProperty RightOverflowIndicatorOffsetProperty =
            DependencyProperty.Register(nameof(RightOverflowIndicatorOffset), typeof(double), typeof(TimelineItemView), new PropertyMetadata(0, (d, e) => (d as TimelineItemView).OnRightOverflowIndicatorChanged(e)));

        public double LeftOverflowIndicatorOffset
        {
            get { return (double)GetValue(LeftOverflowIndicatorOffsetProperty); }
            set { SetValue(LeftOverflowIndicatorOffsetProperty, value); }
        }

        public double RightOverflowIndicatorOffset
        {
            get { return (double)GetValue(RightOverflowIndicatorOffsetProperty); }
            set { SetValue(RightOverflowIndicatorOffsetProperty, value); }
        }

        public LayerViewModel Layer
        {
            get { return (LayerViewModel)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }

        public bool IsLeftOverflowing
        {
            get { return (bool)GetValue(IsLeftOverflowingProperty); }
            set { SetValue(IsLeftOverflowingProperty, value); }
        }

        public bool IsRightOverflowing
        {
            get { return (bool)GetValue(IsRightOverflowingProperty); }
            set { SetValue(IsRightOverflowingProperty, value); }
        }

        public double InitialX { get; internal set; }
        public double InitialY { get; internal set; }

        public TimelineItemView()
        {
            InitializeComponent();
        }
        private void OnLeftOverflowIndicatorChanged(DependencyPropertyChangedEventArgs e)
        {
            lblLeftIndicator.Margin = new Thickness((double)e.NewValue, 0, 0, 0);
        }

        private void OnRightOverflowIndicatorChanged(DependencyPropertyChangedEventArgs e)
        {
            lblRightIndicator.Margin = new Thickness(0, 0, (double)e.NewValue, 0);
        }
    }
}
