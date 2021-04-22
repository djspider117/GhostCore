using GhostCore.Animations.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace GhostCore.Animations.Rendering
{
    internal interface IGeometryMask
    {
        IMask OriginalMask { get; }
        CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator);
    }

}
