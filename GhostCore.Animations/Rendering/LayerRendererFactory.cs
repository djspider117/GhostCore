using GhostCore.Animations.Layers;
using System;
using System.Collections.Generic;

namespace GhostCore.Animations.Rendering
{
    public class LayerRendererFactory
    {
        private static Dictionary<Type, Func<ILayer, LayerRendererBase>> _typeMapping;

        static LayerRendererFactory()
        {
            _typeMapping = new Dictionary<Type, Func<ILayer, LayerRendererBase>>
            {
                { typeof(ImageLayer), layer => new ImageLayerRenderer(layer) },
                { typeof(TextLayer), layer => new TextLayerRenderer(layer) },
                { typeof(PathLayer), layer => new PathLayerRenderer(layer) },
                { typeof(CompositeLayer), layer => new CompositeLayerRenderer(layer) },
            };
        }

        public static LayerRendererBase CreateRenderer(ILayer layer)
        {
            if (_typeMapping.TryGetValue(layer.GetType(), out var factory))
                return factory(layer);

            throw new InvalidOperationException("Cannot create a renderer for an unregistered layer type");
        }
    }
}
