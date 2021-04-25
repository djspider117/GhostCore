using GhostCore.Animations.Core;
using GhostCore.Animations.Data;
using GhostCore.Animations.Rendering;
using GhostCore.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Media;

namespace GhostCore.Animations.Editor.ViewModels
{

    public class ProjectViewModel : ViewModelBase<Project>
    {
        private SceneViewModel _selectedScene;

        public ViewModelCollection<SceneViewModel, Scene> Scenes { get; set; }
        public ViewModelCollection<ProjectAssetViewModel, ProjectAsset> Assets { get; set; }

        public SceneViewModel SelectedScene
        {
            get { return _selectedScene; }
            set { _selectedScene = value; OnPropertyChanged(nameof(SelectedScene)); }
        }

        public ProjectViewModel(Project model)
            : base(model)
        {
            Assets = new ViewModelCollection<ProjectAssetViewModel, ProjectAsset>(model.Assets);
            Scenes = new ViewModelCollection<SceneViewModel, Scene>(model.Scenes);
            _selectedScene = Scenes[0];
        }
    }

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

        public ViewModelCollection<LayerViewModel, ILayer> Layers { get; set; }

        public SceneViewModel(Scene model)
            : base(model)
        {
            if (model.RenderInfo == null)
                throw new ArgumentNullException(nameof(model.RenderInfo));

            _renderInfo = new RenderInfoViewModel(model.RenderInfo);

            Layers = new ViewModelCollection<LayerViewModel, ILayer>(model.Layers);
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].DisplayIndex = i + 1;
            }
        }
    }

    public class LayerViewModel : ViewModelBase<ILayer>
    {
        private bool _isSolo;
        private int _displayIndex;

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public int DisplayIndex
        {
            get { return _displayIndex; }
            set { _displayIndex = value; OnPropertyChanged(nameof(DisplayIndex)); }
        }

        public SolidColorBrush PreviewColor
        {
            get { return Model.PreviewColor.ToSolidColorBrush(); }
            set { Model.PreviewColor = value.ToRGBA(); OnPropertyChanged(nameof(PreviewColor)); }
        }

        public float StartTime
        {
            get { return Model.StartTime; }
            set { Model.StartTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        public float Duration
        {
            get { return Model.Duration; }
            set { Model.Duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        public float EndTime => Model.EndTime;

        public float Opacity
        {
            get { return Model.Opacity; }
            set { Model.Opacity = value; OnPropertyChanged(nameof(Opacity)); }
        }

        public bool IsVisible
        {
            get { return Model.IsVisible; }
            set { Model.IsVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }

        public bool IsLocked
        {
            get { return Model.IsLocked; }
            set { Model.IsLocked = value; OnPropertyChanged(nameof(IsLocked)); }
        }

        public Vector2 Anchor
        {
            get { return Model.Anchor; }
            set { Model.Anchor = value; OnPropertyChanged(nameof(Anchor)); }
        }

        public LayerBlendMode BlendMode
        {
            get { return Model.BlendMode; }
            set { Model.BlendMode = value; OnPropertyChanged(nameof(BlendMode)); }
        }

        public ObservableTransformData Transform { get; set; }

        public bool IsSolo
        {
            get { return _isSolo; }
            set { _isSolo = value; OnPropertyChanged(nameof(IsSolo)); }
        }

        // TODO: add animations
        // TODO: add masks

        public LayerViewModel(ILayer model) 
            : base(model)
        {
            Transform = new ObservableTransformData(model.Transform);
        }
    }

}
