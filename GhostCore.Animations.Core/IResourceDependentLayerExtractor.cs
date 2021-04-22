using System.Collections.Generic;

namespace GhostCore.Animations.Core
{
    public interface IResourceDependentLayerExtractor
    {
        IEnumerable<IResourceDependentLayer> ExtractResourceDependentLayers();
    }

    public static class ResourceDependentLayerExtractionHelper
    {
        public static IEnumerable<IResourceDependentLayer> Extract(IEnumerable<ILayer> layerCollection)
        {
            if (layerCollection != null)
            {
                foreach (var x in layerCollection)
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
