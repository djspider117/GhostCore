using GhostCore.Animations.Core;
using GhostCore.Graphics.Colors;
using System.Collections.Generic;

namespace GhostCore.Animations.Data
{
    public class Scene : IScene
    {
        public string Name { get; set; }

        public RenderInfo RenderInfo { get; set; }
        public RGBA BackdropColor { get; set; }
        public IList<ILayer> Layers { get; set; }

        public IEnumerable<IResourceDependentLayer> ExtractResourceDependentLayers()
        {
            return ResourceDependentLayerExtractionHelper.Extract(Layers);
        }
    }
}
