using System.Collections.Generic;

namespace GhostCore.Animations.Core
{
    public interface IResourceDependentLayerExtractor
    {
        IEnumerable<IResourceDependentLayer> ExtractResourceDependentLayers();
    }

}
