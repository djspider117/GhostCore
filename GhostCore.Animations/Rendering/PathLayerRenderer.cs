using CompositionProToolkit.Win2d;
using GhostCore.Animations.Layers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace GhostCore.Animations.Rendering
{
    public class PathLayerRenderer : LayerRendererBase
    {
        private CanvasGeometry _geometry;
        private CanvasCachedGeometry _cachedGeom;
        private float _geomLen;
        private CanvasStroke _stroke;

        public float _runtimeTrimBegin;
        public float _runtimeTrimEnd;
        public float _runtimeThickness;
        public float _runtimeDashOffset;
        public float _runtimeDashLen;

        public PathLayerRenderer(ILayer layer)
            : base(layer)
        {

        }

        public override async Task Initialze(ICanvasResourceCreator resourceCreator)
        {
            await base.Initialze(resourceCreator);

            var layer = _layer as PathLayer;

            _runtimeDashLen = layer.DashLen;
            _runtimeDashOffset = layer.DashOffset;
            _runtimeThickness = layer.Thickness;
            _runtimeTrimBegin = layer.TrimBegin;
            _runtimeTrimEnd = layer.TrimEnd;

            _geometry = CanvasObject.CreateGeometry(resourceCreator, layer.Data);
            _cachedGeom = CanvasCachedGeometry.CreateStroke(_geometry, layer.Thickness);

            _geomLen = _geometry.ComputePathLength();

            _stroke = new CanvasStroke(resourceCreator, layer.Color, layer.Thickness, new CanvasStrokeStyle()
            {
                StartCap = layer.StartCap,
                LineJoin = layer.LineJoin,
                MiterLimit = layer.MiterLimit,
                EndCap = layer.EndCap
            });
        }

        public override Vector2 Measure(CanvasDrawingSession ds)
        {
            var size = new Vector2();
            var b = _geometry.ComputeBounds();
            size.X = (float)b.Right - (float)b.Left;
            size.Y = (float)b.Bottom - (float)b.Top;

            return size;
        }

        public override void RenderLayerComponents(CanvasDrawingSession ds)
        {
            var layer = _layer as PathLayer;

            if (layer.DashStyle == CanvasDashStyle.Dash)
            {
                var numDashes = _geomLen + _runtimeDashOffset / (_runtimeDashLen + _runtimeDashOffset);

                float cursor = _runtimeTrimBegin * _geomLen;
                for (int i = 0; i < numDashes; i++)
                {
                    var d0 = cursor;
                    var d1 = d0 + _runtimeDashLen;

                    var pt0 = _geometry.ComputePointOnPath(d0);
                    var pt1 = _geometry.ComputePointOnPath(d1);

                    ds.DrawLine(pt0, pt1, _stroke);

                    cursor = d1 + _runtimeDashOffset;

                    if (cursor >= _runtimeTrimEnd * _geomLen)
                        break;
                }

                return;
            }

            if (layer.DashStyle == CanvasDashStyle.Dot)
            {
                var numDashes = _geomLen + _runtimeDashOffset / (_runtimeDashLen + _runtimeDashOffset);

                float cursor = _runtimeTrimBegin * _geomLen;
                for (int i = 0; i < numDashes; i++)
                {
                    var pt0 = _geometry.ComputePointOnPath(cursor);

                    ds.FillCircle(pt0, _runtimeThickness, layer.Color);

                    cursor += _runtimeDashOffset;

                    if (cursor >= _runtimeTrimEnd * _geomLen)
                        break;
                }

                return;
            }

            //ds.DrawCachedGeometry(_cachedGeom, layer.Color);
        }

        public override void UpdatePerLayerAnimation(float val, string[] splitPath)
        {
            base.UpdatePerLayerAnimation(val, splitPath);

            if (splitPath[0] == "TrimEnd")
            {
                _runtimeTrimEnd = val;
                return;
            }

            if (splitPath[0] == "TrimBegin")
            {
                _runtimeTrimBegin = val;
                return;
            }

            if (splitPath[0] == "DashOffset")
            {
                _runtimeDashOffset = val;
                return;
            }

            if (splitPath[0] == "DashLen")
            {
                _runtimeDashLen = val;
                return;
            }

            if (splitPath[0] == "Thickness")
            {
                _runtimeThickness = val;
                return;
            }
        }
    }
}

