using GhostCore.Graphics.Colors;
using System.Collections.Generic;

namespace GhostCore.Animations.Core
{
    public interface IScene : INamed, IResourceDependentLayerExtractor
    {
        IList<ILayer> Layers { get; set; }
        RGBA BackdropColor { get; set; }
        RenderInfo RenderInfo { get; set; }
    }
}
