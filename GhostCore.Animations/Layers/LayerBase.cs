using System.Collections.Generic;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace GhostCore.Animations.Layers
{
    public abstract class LayerBase : ILayer
    {
        public string Name { get; set; }
        public float Opacity { get; set; }
        public TransformData Transform { get; set; }
        public IList<AnimationCurve> Animations { get; set; }
        public ILayer Parent { get; set; }
        public CanvasBlend BlendMode { get; set; }
        public bool IsVisible { get; set; }
        public float StartTime { get; set; }
        public float Duration { get; set; }
        public float EndTime => StartTime + Duration;

        public string MaskSource { get; set; }
        public bool IsMasked { get; set; }
        public bool UseRectangleMask { get; set; }
        public Rect RectangleMask { get; set; }

        public LayerBase()
        {
            Animations = new List<AnimationCurve>();
            Transform = new TransformData(Vector2.Zero, Vector2.Zero, Vector2.One, 0);
            Opacity = 1;
            IsVisible = true;
        }
    }
}
