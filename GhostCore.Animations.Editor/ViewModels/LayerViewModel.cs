using GhostCore.Animations.Core;
using GhostCore.Animations.Rendering;
using GhostCore.MVVM;
using System.Numerics;
using Windows.UI.Xaml.Media;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class LayerViewModel : ViewModelBase<ILayer>
    {
        private bool _isSolo;
        private int _displayIndex;
        private bool _isSelected;

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

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(IsNotSelected));
            }
        }
        public bool IsNotSelected
        {
            get { return !_isSelected; }
            set
            {
                _isSelected = !value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(IsNotSelected));
            }
        }


        private double _virtualWidth;

        public double VirtualWidth
        {
            get { return _virtualWidth; }
            set { _virtualWidth = value; OnPropertyChanged(nameof(VirtualWidth)); }
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
