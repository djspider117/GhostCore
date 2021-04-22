using GhostCore.Animations.Core;
using GhostCore.Animations.Data.Layers;
using Microsoft.Graphics.Canvas;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Animations.Rendering.Loaders
{
    public class ImageLayerResourceLoader : ILayerSpecificResourceLoader
    {
        public async Task LoadResourceAsync(ICanvasResourceCreator resourceCreator, IResourceDependentLayer layer)
        {
            if (!(layer is ImageLayer rdl))
                throw new ArgumentException($"{nameof(layer)} parameter must be of type {nameof(ImageLayer)}");

            if (rdl.Type == ImageType.Bitmap)
            {
                if (!RuntimeBitmapCache.DeviceCache.TryGetValue(resourceCreator, out var cache))
                {
                    cache = new BitmapResourceCache();
                    RuntimeBitmapCache.DeviceCache.Add(resourceCreator, cache);
                }

                if (!cache.ContainsKey(rdl.Source))
                {
                    var bitmap = await RuntimeBitmapCache.RequestBitmapLoad(rdl.Source, resourceCreator);

                    if (bitmap != null)
                        cache.Add(rdl.Source, bitmap);
                }
            }

            if (rdl.Type == ImageType.Svg)
            {
                if (!RuntimeSvgCache.DeviceCache.TryGetValue(resourceCreator, out var scache))
                {
                    scache = new SvgResourceCache();
                    RuntimeSvgCache.DeviceCache.Add(resourceCreator, scache);
                }

                if (!scache.ContainsKey(rdl.Source))
                {
                    var svgDoc = await RuntimeSvgCache.RequestSvgDocumentLoad(rdl.Source, resourceCreator);

                    if (svgDoc != null)
                        scache.Add(rdl.Source, svgDoc);
                }
            }
        }
    }
}
