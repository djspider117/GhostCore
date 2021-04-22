using GhostCore.Animations.Data.Layers;
using System;
using System.Collections.Generic;

namespace GhostCore.Animations.Rendering.Loaders
{
    public class LayerSpecificResourceLoaderFactory : DictionaryFactory<Type, ILayerSpecificResourceLoader>
    {
        protected override void Configure(Dictionary<Type, Func<ILayerSpecificResourceLoader>> factoryMap)
        {
            factoryMap.Add(typeof(ImageLayer), () => new ImageLayerResourceLoader());
        }
    }
}
