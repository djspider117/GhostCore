﻿using CompositionProToolkit.Win2d;
using GhostCore.Animations.Core;
using GhostCore.Animations.Data;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace GhostCore.Animations.Rendering
{
    internal static class GeometryMaskFactory
    {
        public static IGeometryMask TryConvert(IMask mask)
        {
            if (mask is RectMask rm)
                return new GeometricRectMask(rm);


            return null;
        }
    }

    internal class GeometricRectMask : IGeometryMask
    {
        private RectMask _mask;
        public IMask OriginalMask => _mask;

        internal GeometricRectMask(RectMask mask)
        {
            _mask = mask;
        }

        public CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            var trux = _mask.RelativeOffset.X + _mask.Anchor.X;
            var truy = _mask.RelativeOffset.Y + _mask.Anchor.Y;

            return CanvasGeometry.CreateRectangle(resourceCreator, trux, truy, _mask.Width, _mask.Height);
        }

    }
}
