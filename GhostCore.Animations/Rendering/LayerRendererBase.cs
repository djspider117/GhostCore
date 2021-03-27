using CompositionProToolkit.Win2d;
using GhostCore.Animations.Controls;
using GhostCore.Animations.Layers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace GhostCore.Animations.Rendering
{
    public abstract class LayerRendererBase
    {
        protected ILayer _layer;
        protected CanvasGeometry _mask;

        public ILayer Layer { get => _layer; set => _layer = value; }
        public virtual float CurrentTime { get; internal set; }
        public TransformData RenderTransform;
        public FoundationComposition ParentComposition { get; set; }
        internal int ParentCompIndex { get; set; }

        public LayerRendererBase(ILayer layer)
        {
            Layer = layer;
            RenderTransform = layer?.Transform;
        }

        public virtual Task Initialze(ICanvasResourceCreator resourceCreator)
        {
            if (Layer.MaskSource != null)
                _mask = CanvasObject.CreateGeometry(resourceCreator, Layer.MaskSource);

            return Task.CompletedTask;
        }

        public virtual void UpdateAnimationState(float time)
        {
            var layer = Layer;
            foreach (var animCurve in layer.Animations)
            {
                float curTimeInSec = time / 1000;
                if (layer.Duration != 0 && curTimeInSec > layer.EndTime)
                    continue;

                var val = animCurve.Evaluate(curTimeInSec, layer.StartTime);
                var splitPath = animCurve.TargetProperty.Split('.');

                UpdatePerLayerAnimation(val, splitPath);
            }
        }

        public virtual void Render(CanvasDrawingSession ds)
        {
            InternalRender(_layer, ds);
        }

        public abstract Vector2 Measure(CanvasDrawingSession ds);
        public abstract void RenderLayerComponents(CanvasDrawingSession ds);

        protected void InternalRender(ILayer layer, CanvasDrawingSession ds)
        {
            if (!layer.IsVisible)
                return;

            if (CurrentTime >= layer.StartTime * 1000)
            {
                if (layer.Duration != 0 && CurrentTime >= layer.EndTime * 1000)
                    return;

                var sz = Measure(ds);
                var cpos = RenderTransform.Center * sz;

                var p = RenderTransform.Position - cpos;
                var s = RenderTransform.Scale;

                var transform = Matrix3x2.CreateScale(s, cpos) *
                                Matrix3x2.CreateTranslation(p) *
                                Matrix3x2.CreateRotation(RenderTransform.Rotation, cpos + RenderTransform.Position);

                ds.Transform = transform;
                ds.Blend = layer.BlendMode;

                CanvasActiveLayer drawingLayer = null;

                if (layer.IsMasked)
                {
                    if (layer.UseRectangleMask)
                    {
                        var r = layer.RectangleMask;
                        var clipRect = new Rect(r.X - p.X, r.Y - p.Y, r.Width * s.X, r.Height * s.Y);

                        // TODO: implement clip rect rotation

                        drawingLayer = ds.CreateLayer(layer.Opacity, clipRect);
                    }
                    else if (_mask != null)
                    {
                        drawingLayer = ds.CreateLayer(layer.Opacity, _mask);
                    }
                }
                else
                {
                    drawingLayer = ds.CreateLayer(layer.Opacity);
                }

                using (drawingLayer)
                {
                    RenderLayerComponents(ds);
                }

            }
        }

        public virtual void UpdatePerLayerAnimation(float val, string[] splitPath)
        {
            var layer = Layer;
            // NOTE: maybe it's better to have all the code here and return whenever a subpart is complete, in order to minimize push/pop on the stack (save 0.5ms ?)

            UpdateTransform(RenderTransform, layer.Transform, val, splitPath);
            UpdateOpacity(layer, val, splitPath);
        }

        private void UpdateOpacity(ILayer layer, float val, string[] splitPath)
        {
            if (splitPath[0] == "Opacity")
                layer.Opacity = val;
        }

        private void UpdateTransform(TransformData renderTransform, TransformData trans, float val, string[] splitPath)
        {
            float x = trans.Position.X;
            float y = trans.Position.Y;
            float scalex = trans.Scale.X;
            float scaley = trans.Scale.Y;
            float rot = trans.Rotation;
            float cx = trans.Center.X;
            float cy = trans.Center.Y;

            if (splitPath[0] == "Transform")
            {
                if (splitPath[1] == "Position")
                {
                    if (splitPath[2] == "X")
                    {
                        x = val;
                        goto SET_VALUES;
                    }

                    if (splitPath[2] == "Y")
                    {
                        y = val;
                        goto SET_VALUES;
                    }
                }

                if (splitPath[1] == "Scale")
                {
                    if (splitPath.Length == 2)
                    {
                        scalex = val;
                        scaley = val;
                        goto SET_VALUES;
                    }

                    if (splitPath[2] == "X")
                    {
                        scalex = val;
                        goto SET_VALUES;
                    }

                    if (splitPath[2] == "Y")
                    {
                        scaley = val;
                        goto SET_VALUES;
                    }
                }

                if (splitPath[1] == "Center")
                {
                    if (splitPath[2] == "X")
                    {
                        cx = val;
                        goto SET_VALUES;
                    }

                    if (splitPath[2] == "Y")
                    {
                        cy = val;
                        goto SET_VALUES;
                    }
                }

                if (splitPath[1] == "Rotation")
                    rot = val;
            }

        SET_VALUES:
            renderTransform.Position = new Vector2(x, y);
            renderTransform.Center = new Vector2(cx, cy);
            renderTransform.Scale = new Vector2(scalex, scaley);
            renderTransform.Rotation = rot;
        }
    }
}
