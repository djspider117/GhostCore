using GhostCore.MVVM;
using System.ComponentModel;
using System.Numerics;

namespace GhostCore.Animations.Core
{
    public class TransformData
    {
        public readonly static TransformData Default = new TransformData(Vector2.Zero, Vector2.Zero, Vector2.One, 0);

        public Vector2 Center { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        public TransformData()
        {

        }

        public TransformData(Vector2 center, Vector2 position, Vector2 scale, float rotation)
        {
            Center = center;
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }
    }

    public class ObservableTransformData : ViewModelBase<TransformData>
    {
        public Vector2 Center
        {
            get { return Model.Center; }
            set { Model.Center = value; OnPropertyChanged(nameof(Center)); }
        }

        public Vector2 Position
        {
            get { return Model.Position; }
            set { Model.Position = value; OnPropertyChanged(nameof(Position)); }
        }

        public Vector2 Scale
        {
            get { return Model.Scale; }
            set { Model.Scale = value; OnPropertyChanged(nameof(Scale)); }
        }

        public float Rotation
        {
            get { return Model.Rotation; }
            set { Model.Rotation = value; OnPropertyChanged(nameof(Rotation)); }
        }

        public ObservableTransformData(TransformData model)
            : base(model)
        {

        }
    }
}
