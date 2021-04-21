using Microsoft.Graphics.Canvas.Svg;

namespace GhostCore.Animations.Rendering
{
    public class SvgLoadEventArgs : FileLoadEventArgs
    {
        public CanvasSvgDocument SvgDocument { get; set; }
    }
}
