using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace GhostCore.Animations.Rendering
{
    internal interface IGeometryMask
    {
        CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator);
    }

}
