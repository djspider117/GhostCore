using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI;

namespace GhostCore.Animations.Layers
{
    public class PathLayer : LayerBase
    {
        public string Data { get; set; }
        public float TrimBegin { get; set; }
        public float TrimEnd { get; set; } = 1;
        public float Thickness { get; set; } = 2;
        public Color Color { get; set; } = Colors.Red;

        public CanvasStrokeTransformBehavior TransformBehavior { get; set; } = CanvasStrokeTransformBehavior.Normal;
        public CanvasCapStyle StartCap { get; set; } = CanvasCapStyle.Round;
        public float MiterLimit { get; set; }
        public CanvasLineJoin LineJoin { get; set; } = CanvasLineJoin.Round;
        public CanvasCapStyle EndCap { get; set; } = CanvasCapStyle.Round;
        public CanvasDashStyle DashStyle { get; set; } = CanvasDashStyle.Dot;
        public float DashOffset { get; set; }
        public float DashLen { get; set; }
        public CanvasCapStyle DashCap { get; set; } = CanvasCapStyle.Round;
    }
}
