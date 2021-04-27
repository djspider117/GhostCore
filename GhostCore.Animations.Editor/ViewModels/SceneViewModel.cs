using GhostCore.Animations.Core;
using GhostCore.Animations.Data;
using GhostCore.Animations.Rendering;
using GhostCore.MVVM;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class SceneViewModel : ViewModelBase<Scene>
    {
        private RenderInfoViewModel _renderInfo;

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public RenderInfoViewModel RenderInfo
        {
            get { return _renderInfo; }
            set { _renderInfo = value; OnPropertyChanged(nameof(RenderInfo)); }
        }
        public SolidColorBrush BackdropColor
        {
            get { return Model.BackdropColor.ToSolidColorBrush(); }
            set { Model.BackdropColor = value.ToRGBA(); OnPropertyChanged(nameof(BackdropColor)); }
        }
        public float Duration
        {
            get { return Model.Duration; }
            set { Model.Duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        public TimelineViewModel Timeline { get; set; }

        public ViewModelCollection<LayerViewModel, ILayer> Layers { get; set; }

        public ObservableCollection<LayerViewModel> SelectedLayers { get; set; }

        public SceneViewModel(Scene model)
            : base(model)
        {
            if (model.RenderInfo == null)
                throw new ArgumentNullException(nameof(model.RenderInfo));

            SelectedLayers = new ObservableCollection<LayerViewModel>();
            _renderInfo = new RenderInfoViewModel(model.RenderInfo);

            Layers = new ViewModelCollection<LayerViewModel, ILayer>(model.Layers);
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].DisplayIndex = i + 1;
            }
            Timeline = new TimelineViewModel(0, model.Duration);
        }
    }

}
