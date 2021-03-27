using GhostCore.Animations.Layers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace GhostCore.Animations.Rendering
{
    public class ImageLayerRenderer : LayerRendererBase
    {
        private CanvasBitmap _bitmapRef;
        private CanvasSvgDocument _svgRef;

        public ImageLayerRenderer(ILayer layer) 
            : base(layer)
        {
        }

        public override async Task Initialze(ICanvasResourceCreator resourceCreator)
        {
            await base.Initialze(resourceCreator);

            var imglayer = _layer as ImageLayer;

            switch (imglayer.Type)
            {
                case ImageType.Bitmap:
                    RuntimeBitmapCache.DeviceCache[resourceCreator].TryGetValue(imglayer.Source, out _bitmapRef);
                    break;
                case ImageType.Svg:
                    RuntimeSvgCache.DeviceCache[resourceCreator].TryGetValue(imglayer.Source, out _svgRef);
                    break;
                default:
                    break;
            }
        }

        public override Vector2 Measure(CanvasDrawingSession ds)
        {
            if (_bitmapRef != null)
            {
                var sz = _bitmapRef.SizeInPixels;
                return new Vector2(sz.Width, sz.Height);
            }

            return Vector2.Zero;
        }

        public override void RenderLayerComponents(CanvasDrawingSession ds)
        {
            var imglayer = _layer as ImageLayer;

            if (imglayer.Type == ImageType.Bitmap && _bitmapRef == null)
            {
                ds.DrawText($"Bitmap resource not loaded for {imglayer.Source}", new Vector2(), Colors.Magenta);
                return;
            }

            if (imglayer.Type == ImageType.Svg && _svgRef == null)
            {
                ds.DrawText($"SVG resource not loaded for {imglayer.Source}", new Vector2(), Colors.Magenta);
                return;
            }

            switch (imglayer.Type)
            {
                case ImageType.Bitmap:
                    ds.DrawImage(_bitmapRef);
                    break;
                case ImageType.Svg:
                    ds.DrawSvg(_svgRef, new Size(imglayer.SvgViewportWidth, imglayer.SvgViewportHeight));
                    break;
                default:
                    break;
            }
        }
    }
}
