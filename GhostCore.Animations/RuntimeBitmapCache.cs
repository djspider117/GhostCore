using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Collections.Generic;

namespace GhostCore.Animations
{
    public static class RuntimeBitmapCache
    {
        public static Dictionary<ICanvasResourceCreator, BitmapResourceCache> DeviceCache { get; internal set; } = new Dictionary<ICanvasResourceCreator, BitmapResourceCache>();
    }

    public static class RuntimeSvgCache
    {
        public static Dictionary<ICanvasResourceCreator, SvgResourceCache> DeviceCache { get; internal set; } = new Dictionary<ICanvasResourceCreator, SvgResourceCache>();
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
