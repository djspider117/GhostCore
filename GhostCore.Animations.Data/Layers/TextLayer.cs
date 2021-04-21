using GhostCore.Animations.Core;
using GhostCore.Graphics.Colors;
using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.Animations.Data.Layers
{
    public class TextLayer : LayerBase
    {
        public string Text { get; set; }
        public GhostWordWrapping WordWrapping { get; set; } = GhostWordWrapping.NoWrap;
        public GhostVerticalAlignment VerticalAlignment { get; set; } = GhostVerticalAlignment.Bottom;
        public GhostHorizontalAlignment HorizontalAlignment { get; set; } = GhostHorizontalAlignment.Left;
        public GhostDrawTextOptions Options { get; set; } = GhostDrawTextOptions.Default;
        public GhostLineSpacingMode LineSpacingMode { get; set; } = GhostLineSpacingMode.Default;
        public float? LineSpacingBaseline { get; set; }
        public float? LineSpacing { get; set; }
        public bool? LastLineWrapping { get; set; }
        public float? IncrementalTabStop { get; set; }
        public ushort? FontWeight { get; set; }
        public GhostFontStyle FontStyle { get; set; } = GhostFontStyle.Normal;
        public GhostFontStretch FontStretch { get; set; } = GhostFontStretch.Normal;
        public float? FontSize { get; set; }
        public string FontFamily { get; set; }
        public GhostTextDirection Direction { get; set; } = GhostTextDirection.LeftToRightThenBottomToTop;
        public RGBA Color { get; set; }
        public List<TextKeyframe> TextKeyframes { get; set; }
    }

}
