using Microsoft.Graphics.Canvas.Svg;

namespace GhostCore.Animations.Controls
{

    public class SvgLoadEventArgs : FileLoadEventArgs
    {
        public CanvasSvgDocument SvgDocument { get; set; }
    }
}
