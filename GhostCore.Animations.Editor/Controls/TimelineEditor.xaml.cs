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
using Windows.UI.Xaml.Shapes;

namespace GhostCore.Animations.Editor.Controls
{
    // TODO: update timeline item views when current scene changes

    public sealed partial class TimelineEditor : EditorUserControl
    {
        private Dictionary<LayerViewModel, TimelineItemView> _uiMapping;
        private ScrollViewer _svLayersScrollViewer;

        public TimelineEditor()
        {
            _uiMapping = new Dictionary<LayerViewModel, TimelineItemView>();
            InitializeComponent();

        }

        protected override void OnLoadedInternal(object sender, RoutedEventArgs e)
        {
            base.OnLoadedInternal(this, e);
            CurrentProject.PropertyChanged += CurrentProject_PropertyChanged;
            SetSelectedLayers();

            _svLayersScrollViewer = lvLayers.GetFirstDescendantOfType<ScrollViewer>();
            _svLayersScrollViewer.ViewChanged += svLayersScrollViewer_ViewChanged;
        }
        protected override void OnUnloadedInternal(object sender, RoutedEventArgs e)
        {
            base.OnUnloadedInternal(sender, e);
            CurrentProject.PropertyChanged -= CurrentProject_PropertyChanged;
            pnlTimelineArea.Loaded -= pnlTimelineArea_Loaded;
            pnlTimelineArea.SizeChanged -= pnlTimelineArea_SizeChanged;
            _svLayersScrollViewer.ViewChanged -= svLayersScrollViewer_ViewChanged;
        }

        #region Manual binding for selected layers GG MICROSOFT no propdp for 

        private void CurrentProject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProjectViewModel.SelectedScene))
            {
                SetSelectedLayers();
            }
        }

        private void SetSelectedLayers()
        {
            if (CurrentProject.SelectedScene != null)
            {
                lvLayers.SelectedItems.Clear();
                foreach (var item in CurrentProject.SelectedScene.SelectedLayers)
                {
                    lvLayers.SelectedItems.Add(item);
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (LayerViewModel item in e.RemovedItems)
            {
                CurrentProject.SelectedScene.SelectedLayers.Remove(item);
                item.IsSelected = false;
            }

            foreach (LayerViewModel item in e.AddedItems)
            {
                CurrentProject.SelectedScene.SelectedLayers.Add(item);
                item.IsSelected = true;
            }
        }

        #endregion

        #region Timeline Layout

        private void pnlTimelineArea_Loaded(object sender, RoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            var layerHeight = 32;

            int i = 0;
            var sceneDuration = CurrentProject.SelectedScene.Duration;

            canvas.Children.Clear();
            foreach (var layer in CurrentProject.SelectedScene.Layers)
            {
                var relStart = layer.StartTime / sceneDuration;
                var relDuration = layer.Duration / sceneDuration;

                var x = relStart * canvas.ActualWidth;

                var view = new TimelineItemView
                {
                    Layer = layer,
                    Height = layerHeight,
                    Width = relDuration * canvas.ActualWidth
                };

                view.InitialX = x;
                view.InitialY = i * layerHeight;

                Canvas.SetLeft(view, view.InitialX);
                Canvas.SetTop(view, view.InitialY);

                _uiMapping.Add(layer, view);
                canvas.Children.Add(view);

                i++;
            }
        }
        private void UpdateLayerCanvas()
        {
            pnlTimelineArea.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, pnlTimelineArea.ActualWidth, pnlTimelineArea.ActualHeight) };

            if (!_isLoaded)
                return;

            var vOffset = _svLayersScrollViewer.VerticalOffset;

            int i = 0;
            var sceneDuration = CurrentProject.SelectedScene.Duration;
            var relativeIn = CurrentProject.SelectedScene.Timeline.StartTime / sceneDuration + slTimeRange.RangeStart;
            var relativeOut = CurrentProject.SelectedScene.Timeline.EndTime * slTimeRange.RangeEnd / sceneDuration;

            relativeOut = System.Math.Max(relativeOut, 0.003);

            var relDuration = (relativeOut - relativeIn) * sceneDuration;

            foreach (var layer in CurrentProject.SelectedScene.Layers)
            {
                var view = _uiMapping[layer];

                var relativeLayerStart = layer.StartTime / relDuration;
                var relativeLayerDuration = layer.Duration / relDuration;

                var x = (relativeLayerStart - relativeIn) * pnlTimelineArea.ActualWidth;
                var y = view.InitialY - vOffset;
                view.Width = relativeLayerDuration * pnlTimelineArea.ActualWidth;
                Canvas.SetLeft(view, x);
                Canvas.SetTop(view, y);

                view.IsLeftOverflowing = x < 0;
                view.IsRightOverflowing = (x + view.Width) > pnlTimelineArea.ActualWidth;
                view.LeftOverflowIndicatorOffset = -x;
                view.RightOverflowIndicatorOffset = (x + view.Width) - pnlTimelineArea.ActualWidth;

                i++;
            }
        }

        #endregion

        #region Canvas Update Triggers

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!_isLoaded)
                return;

            var initialEndTime = CurrentProject.SelectedScene.Timeline.InitialEndTime;
            var maxZoomFactor = CurrentProject.SelectedScene.Timeline.MaxZoomFactor;
            var zoomFactor = (float)(e.NewValue + 1 + e.NewValue * maxZoomFactor);

            CurrentProject.SelectedScene.Timeline.StartTime = 0;
            CurrentProject.SelectedScene.Timeline.EndTime = initialEndTime / zoomFactor;
            UpdateLayerCanvas();
        }
        private void pnlTimelineArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            UpdateLayerCanvas();
        }
        private void svLayersScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            UpdateLayerCanvas();
        }
        private void slTimeRange_ValueChanged(object sender, Microsoft.Toolkit.Uwp.UI.Controls.RangeChangedEventArgs e)
        {
            UpdateLayerCanvas();
        }

        #endregion

    }
}
