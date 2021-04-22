using GhostCore.Animations.Core;
using GhostCore.Animations.Data.Layers;
using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;

namespace GhostCore.Animations.Rendering
{
    public class CompositeLayerRenderer : LayerRendererBase
    {
        private IList<ILayer> _layers;
        private List<LayerRendererBase> _renderers;


        private float _curTime;
        public override float CurrentTime
        {
            get
            {
                return _curTime;
            }
            internal set
            {
                _curTime = value;
                foreach (var rnd in _renderers)
                {
                    rnd.CurrentTime = value;
                }
            }
        }

        public CompositeLayerRenderer(ILayer compositeLayer) : base(compositeLayer)
        {
            var layer = compositeLayer as CompositeLayer;
            _layers = layer.Children;
            _renderers = new List<LayerRendererBase>();
        }

        public override async Task Initialze(ICanvasResourceCreator resourceCreator)
        {
            await base.Initialze(resourceCreator);
            foreach (var layer in _layers)
            {
                var rnd = LayerRendererFactory.CreateRenderer(layer);
                _renderers.Add(rnd);
                await rnd.Initialze(resourceCreator);
            }
        }

        public override void UpdateAnimationState(float time)
        {
            foreach (var _rnd in _renderers)
            {
                _rnd.UpdateAnimationState(time);
            }

            base.UpdateAnimationState(time);
        }

        public override Vector2 Measure(CanvasDrawingSession ds)
        {
            float xmax = 0;
            float ymax = 0;

            foreach (var rnd in _renderers)
            {
                var localSize = rnd.Measure(ds);

                if (xmax < localSize.X)
                    xmax = localSize.X;

                if (ymax < localSize.Y)
                    ymax = localSize.Y;
            }

            return new Vector2(xmax, ymax);
        }

        public override void Render(CanvasDrawingSession ds)
        {
            var psz = Measure(ds);
            var pcpos = RenderTransform.Center * psz;
            var pTrans = Matrix3x2.CreateScale(RenderTransform.Scale, pcpos) *
                         Matrix3x2.CreateTranslation(RenderTransform.Position) *
                         Matrix3x2.CreateRotation(RenderTransform.Rotation, pcpos + RenderTransform.Position);


            foreach (var rnd in _renderers)
            {
                var layer = rnd.Layer;

                if (!layer.IsVisible)
                    return;

                if (rnd.CurrentTime >= layer.StartTime)
                {
                    if (layer.Duration != 0 && rnd.CurrentTime >= layer.EndTime * 1000)
                        return;

                    var sz = rnd.Measure(ds);
                    var cpos = rnd.RenderTransform.Center * sz;

                    var p = rnd.RenderTransform.Position;
                    var s = rnd.RenderTransform.Scale;

                    var transform = Matrix3x2.CreateScale(s, cpos) *
                                    Matrix3x2.CreateTranslation(p) *
                                    Matrix3x2.CreateRotation(rnd.RenderTransform.Rotation, cpos + p);

                    transform *= pTrans;

                    ds.Transform = transform;
                    ds.Blend = (CanvasBlend)layer.BlendMode;

                    CanvasActiveLayer drawingLayer = null;

                    var shouldHandleMasks = layer.Masks.Count != 0;

                    if (shouldHandleMasks && rnd.Mask != null)
                    {
                        var maskTransform = Matrix3x2.CreateScale(new Vector2(1 / s.X, 1 / s.Y), cpos) *
                                            Matrix3x2.CreateTranslation(-p) *
                                            Matrix3x2.CreateRotation(rnd.RenderTransform.Rotation, cpos + p);

                        if (MaskRenderMode != MaskRenderMode.Hidden)
                        {
                            ds.Transform = transform * maskTransform;

                            if (MaskRenderMode == MaskRenderMode.Border)
                                ds.DrawGeometry(rnd.Mask, Colors.Red, 2);

                            if (MaskRenderMode == MaskRenderMode.Fill)
                                ds.FillGeometry(rnd.Mask, Colors.Red);

                            ds.Transform = transform;
                        }

                        drawingLayer = ds.CreateLayer(layer.Opacity, rnd.Mask, maskTransform);
                    }
                    else
                    {
                        drawingLayer = ds.CreateLayer(layer.Opacity);
                    }

                    using (drawingLayer)
                    {
                        rnd.RenderLayerComponents(ds);
                    }
                }
            }
        }

        public override void UpdatePerLayerAnimation(float val, string[] splitPath)
        {
            foreach (var rnd in _renderers)
            {
                rnd.UpdatePerLayerAnimation(val, splitPath);
            }

            base.UpdatePerLayerAnimation(val, splitPath);

        }

        public override void RenderLayerComponents(CanvasDrawingSession ds)
        {
        }
    }

}
