using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;
using Windows.UI.Text;

namespace GhostCore.Animations.Layers
{
    public class TextLayer : LayerBase
    {
        public string Text { get; set; }
        public CanvasWordWrapping WordWrapping { get; set; } = CanvasWordWrapping.NoWrap;
        public CanvasVerticalAlignment VerticalAlignment { get; set; } = CanvasVerticalAlignment.Bottom;
        public CanvasHorizontalAlignment HorizontalAlignment { get; set; } = CanvasHorizontalAlignment.Left;
        public CanvasDrawTextOptions Options { get; set; } = CanvasDrawTextOptions.Default;
        public CanvasLineSpacingMode LineSpacingMode { get; set; } = CanvasLineSpacingMode.Default;
        public float? LineSpacingBaseline { get; set; }
        public float? LineSpacing { get; set; }
        public bool? LastLineWrapping { get; set; }
        public float? IncrementalTabStop { get; set; }
        public ushort? FontWeight { get; set; }
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        public FontStretch FontStretch { get; set; } = FontStretch.Normal;
        public float? FontSize { get; set; }
        public string FontFamily { get; set; }
        public CanvasTextDirection Direction { get; set; } = CanvasTextDirection.LeftToRightThenBottomToTop;
        public Color Color { get; set; }
        public List<TextKeyframe> TextKeyframes { get; set; }
    }
}
