﻿using GhostCore.Animations.Layers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System.Linq;
using System.Numerics;
using Windows.UI.Text;

namespace GhostCore.Animations.Rendering
{
    public class TextLayerRenderer : LayerRendererBase
    {
        private CanvasTextFormat _textFormat;
        private CanvasTextLayout _textLayout;

        public TextLayerRenderer(ILayer layer)
            : base(layer)
        {
        }

        public override void UpdateAnimationState(float time)
        {
            base.UpdateAnimationState(time);

            
            var layer = _layer as TextLayer;

            float curTimeInSec = time / 1000;
            if (layer.TextKeyframes != null)
            {
                var kf = layer.TextKeyframes.LastOrDefault(x => x.Time <= curTimeInSec);
                layer.Text = kf.Value;
            }

        }

        public override Vector2 Measure(CanvasDrawingSession ds)
        {
            var textlayer = Layer as TextLayer;

            _textFormat = new CanvasTextFormat()
            {
                WordWrapping = textlayer.WordWrapping,
                VerticalAlignment = textlayer.VerticalAlignment,
                HorizontalAlignment = textlayer.HorizontalAlignment,
                Options = textlayer.Options,
                LineSpacingMode = textlayer.LineSpacingMode,
                FontStyle = textlayer.FontStyle,
                FontStretch = textlayer.FontStretch,
                Direction = textlayer.Direction
            };

            if (textlayer.FontFamily != null)
                _textFormat.FontFamily = textlayer.FontFamily;

            if (textlayer.LineSpacingBaseline != null)
                _textFormat.LineSpacingBaseline = textlayer.LineSpacingBaseline.Value;

            if (textlayer.LineSpacing != null)
                _textFormat.LineSpacing = textlayer.LineSpacing.Value;

            if (textlayer.LastLineWrapping != null)
                _textFormat.LastLineWrapping = textlayer.LastLineWrapping.Value;

            if (textlayer.IncrementalTabStop != null)
                _textFormat.IncrementalTabStop = textlayer.IncrementalTabStop.Value;

            if (textlayer.FontSize != null)
                _textFormat.FontSize = textlayer.FontSize.Value;

            if (textlayer.FontWeight != null)
                _textFormat.FontWeight = new FontWeight() { Weight = textlayer.FontWeight.Value };

            _textLayout = new CanvasTextLayout(ds, textlayer.Text, _textFormat, 0.0f, 0.0f);
            var rv = new Vector2((float)_textLayout.DrawBounds.Width, (float)_textLayout.DrawBounds.Height);
            return rv;
        }

        public override void RenderLayerComponents(CanvasDrawingSession ds)
        {
            var textlayer = Layer as TextLayer;

            ds.DrawTextLayout(_textLayout, Vector2.Zero, textlayer.Color);

            _textLayout.Dispose();
            _textFormat.Dispose();
        }
    }
}
