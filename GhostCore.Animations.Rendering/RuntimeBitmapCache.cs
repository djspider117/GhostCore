using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Animations.Rendering
{
    public static class RuntimeBitmapCache
    {
        public static event AsyncTypedEventHandler<BitmapLoadEventArgs> BitmapLoadRequested;

        public static Dictionary<ICanvasResourceCreator, BitmapResourceCache> DeviceCache { get; internal set; } = new Dictionary<ICanvasResourceCreator, BitmapResourceCache>();

        public static async Task<CanvasBitmap> RequestBitmapLoad(string source, ICanvasResourceCreator resourceCreator)
        {
            var args = new BitmapLoadEventArgs
            {
                Source = source,
                ResourceCreator = resourceCreator
            };

            await BitmapLoadRequested?.Invoke(args);
            return args.Bitmap;
        }
    }

    public static class RuntimeSvgCache
    {
        public static event AsyncTypedEventHandler<SvgLoadEventArgs> SvgLoadRequested;

        public static Dictionary<ICanvasResourceCreator, SvgResourceCache> DeviceCache { get; internal set; } = new Dictionary<ICanvasResourceCreator, SvgResourceCache>();
        public static async Task<CanvasSvgDocument> RequestSvgDocumentLoad(string source, ICanvasResourceCreator resourceCreator)
        {
            var args = new SvgLoadEventArgs
            {
                Source = source,
                ResourceCreator = resourceCreator
            };

            await SvgLoadRequested?.Invoke(args);
            return args.SvgDocument;
        }
    }

    public class BitmapResourceCache : Dictionary<string, CanvasBitmap>, IDisposable
    {
        public void Dispose()
        {
            foreach (var x in Values)
            {
                x.Dispose();
            }
        }
    }

    public class SvgResourceCache : Dictionary<string, CanvasSvgDocument>, IDisposable
    {
        public void Dispose()
        {
            foreach (var x in Values)
            {
                x.Dispose();
            }
        }
    }
}
