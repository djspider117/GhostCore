using GhostCore.Animations.Core;
using GhostCore.MVVM;
using System.Numerics;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class RenderInfoViewModel : ViewModelBase<RenderInfo>
    {
        public Vector2 RenderSize
        {
            get { return Model.RenderSize; }
            set { Model.RenderSize = value; OnPropertyChanged(nameof(RenderSize)); }
        }

        public float RenderScale
        {
            get { return Model.RenderScale; }
            set { Model.RenderScale = value; OnPropertyChanged(nameof(RenderScale)); }
        }

        public float FrameRate
        {
            get { return Model.FrameRate; }
            set { Model.FrameRate = value; OnPropertyChanged(nameof(FrameRate)); }
        }

        public RenderInfoViewModel(RenderInfo model) 
            : base(model)
        {

        }
    }
}
