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
            if (Children != null)
            {
                foreach (var x in Children)
                {
                    if (x is IResourceDependentLayer rdl)
                        yield return rdl;

                    if (x is ICompositeLayer cl)
                    {
                        foreach (var qx in cl.ExtractResourceDependentLayers())
                            yield return qx;
                    }
                }
            }
        }
    }
}
