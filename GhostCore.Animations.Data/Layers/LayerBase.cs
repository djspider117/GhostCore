using GhostCore.Animations.Core;
using GhostCore.Graphics.Colors;
using System.Collections.Generic;
using System.Numerics;

namespace GhostCore.Animations.Data.Layers
{
    public abstract class LayerBase : ILayer
    {
        public string Name { get; set; }
        public RGBA PreviewColor { get; set; } = ColorGenerator.RandomRGBColor(false);

        public float StartTime { get; set; }
        public float Duration { get; set; }
        public float EndTime => StartTime + Duration;

        public float Opacity { get; set; } = 1;
        public bool IsVisible { get; set; } = true;
        public bool IsLocked { get; set; }

        public TransformData Transform { get; set; } = TransformData.Default;
        public Vector2 Anchor { get; set; }


        public IList<AnimationCurve> Animations { get; set; }

        public LayerBlendMode BlendMode { get; set; }
        public IList<IMask> Masks { get; set; }

        public LayerBase()
        {
            Animations = new List<AnimationCurve>();
            Masks = new List<IMask>();

        }

    }
}
