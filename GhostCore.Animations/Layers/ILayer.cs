using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using Windows.Foundation;

namespace GhostCore.Animations.Layers
{
    public interface ILayer
    {
        string Name { get; set; }
        float StartTime { get; set; }
        float Duration { get; set; }
        float EndTime { get; }
        float Opacity { get; set; }
        bool IsVisible { get; set; }
        TransformData Transform { get; set; }
        IList<AnimationCurve> Animations { get; set; }
        CanvasBlend BlendMode { get; set; }
        string MaskSource { get; set; }
        bool IsMasked { get; set; }
        bool UseRectangleMask { get; set; }
        Rect RectangleMask { get; set; }
    }
}
