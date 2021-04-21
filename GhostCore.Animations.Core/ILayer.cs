using GhostCore.Graphics.Colors;
using System.Collections.Generic;
using System.Numerics;

namespace GhostCore.Animations.Core
{
    public interface ILayer : INamed
    {
        RGBA PreviewColor { get; set; }

        float StartTime { get; set; }
        float Duration { get; set; }
        float EndTime { get; }
        float Opacity { get; set; }
        bool IsVisible { get; set; }
        bool IsLocked { get; set; }
        
        TransformData Transform { get; set; }
        Vector2 Anchor { get; set; }

        IList<AnimationCurve> Animations { get; set; }
        LayerBlendMode BlendMode { get; set; }
        IList<IMask> Masks { get; set; }
    }

    public interface ICompositeLayer : ILayer, IResourceDependentLayerExtractor
    {
        IList<ILayer> Children { get; set; }
    }

    public interface IResourceDependentLayer
    {
        // ?
    }
}
