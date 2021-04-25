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
    public sealed partial class TimelineEditor : EditorUserControl
    {

        private Dictionary<LayerViewModel, TimelineItemView> _uiMapping;

        public TimelineEditor()
        {
            _uiMapping = new Dictionary<LayerViewModel, TimelineItemView>();
            InitializeComponent();
        }

        protected override void OnLoadedInternal(object sender, RoutedEventArgs e)
        {
            CurrentProject.PropertyChanged += CurrentProject_PropertyChanged;
            SetSelectedLayers();
        }

        protected override void OnUnloadedInternal(object sender, RoutedEventArgs e)
        {
            base.OnUnloadedInternal(sender, e);
            CurrentProject.PropertyChanged -= CurrentProject_PropertyChanged;
            pnlTimelineArea.Loaded -= pnlTimelineArea_Loaded;
            pnlTimelineArea.SizeChanged -= pnlTimelineArea_SizeChanged;
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

                var rect = new TimelineItemView
                {
                    Layer = layer,
                    Height = layerHeight,
                    Width = relDuration * canvas.ActualWidth
                };

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, i * layerHeight);

                _uiMapping.Add(layer, rect);
                canvas.Children.Add(rect);

                i++;
            }
        }

        private void pnlTimelineArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayerCanvas();
        }

        private void UpdateLayerCanvas()
        {
            int i = 0;
            var sceneDuration = CurrentProject.SelectedScene.Duration;
            var relativeIn = CurrentProject.SelectedScene.Timeline.StartTime / sceneDuration;
            var relativeOut = CurrentProject.SelectedScene.Timeline.EndTime / sceneDuration;

            var relDuration = (relativeOut - relativeIn) * sceneDuration;

            foreach (var layer in CurrentProject.SelectedScene.Layers)
            {
                var view = _uiMapping[layer];

                var relativeLayerStart = layer.StartTime / relDuration;
                var relativeLayerDuration = layer.Duration / relDuration;

                var x = (relativeLayerStart - relativeIn) * pnlTimelineArea.ActualWidth;

                view.Width = relativeLayerDuration * pnlTimelineArea.ActualWidth;
                Canvas.SetLeft(view, x);

                i++;
            }
        }
    }
}
