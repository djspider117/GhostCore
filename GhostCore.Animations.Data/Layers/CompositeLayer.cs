using GhostCore.Animations.Core;
using System.Collections.Generic;

namespace GhostCore.Animations.Data.Layers
{
    public class CompositeLayer : LayerBase, ICompositeLayer
    {
        public IList<ILayer> Children { get; set; }

        public CompositeLayer()
        {
            Children = new List<ILayer>();
        }

        public IEnumerable<IResourceDependentLayer> ExtractResourceDependentLayers()
        {
            return ResourceDependentLayerExtractionHelper.Extract(Children);
        }
    }
}
