using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.Animations.Core
{

    public enum GhostWordWrapping
    {
        //
        // Summary:
        //     Words are broken across lines to avoid text overflowing the layout box.
        Wrap = 0,
        //
        // Summary:
        //     Words are kept within the same line even when it overflows the layout box. This
        //     option is often used with scrolling to reveal overflow text.
        NoWrap = 1,
        //
        // Summary:
        //     Words are broken across lines to avoid text overflowing the layout box. Emergency
        //     wrapping occurs if the word is larger than the maximum width.
        EmergencyBreak = 2,
        //
        // Summary:
        //     When emergency wrapping, only wrap whole words, never breaking words when the
        //     layout width is too small for even a single word.
        WholeWord = 3,
        //
        // Summary:
        //     Wrap between any valid character clusters.
        Character = 4
    }
    public enum GhostVerticalAlignment
    {
        //
        // Summary:
        //     The top of the text is aligned to the top edge of the layout box.
        Top = 0,
        //
        // Summary:
        //     The bottom of the text is aligned to the bottom edge of the layout box.
        Bottom = 1,
        //
        // Summary:
        //     The center of the text is aligned to the center of the layout box.
        Center = 2
    }
    public enum GhostHorizontalAlignment
    {
        //
        // Summary:
        //     The left edge of the text is aligned to the left edge of the layout box.
        Left = 0,
        //
        // Summary:
        //     The right edge of the text is aligned to the right edge of the layout box.
        Right = 1,
        //
        // Summary:
        //     The center of the text is aligned to the center of the layout box.
        Center = 2,
        //
        // Summary:
        //     The left edge of the text is aligned to the left edge of the layout box and broken
        //     lines are aligned to the right edge.
        Justified = 3
    }
    public enum GhostDrawTextOptions : uint
    {
        //
        // Summary:
        //     Text is vertically snapped to pixel boundaries and is not clipped to the layout
        //     rectangle.
        Default = 0,
        //
        // Summary:
        //     Text is not vertically snapped to pixel boundaries. This setting is recommended
        //     for text that is being animated.
        NoPixelSnap = 1,
        //
        // Summary:
        //     Text is clipped to the layout rectangle.
        Clip = 2,
        //
        // Summary:
        //     Text is rendered using color versions of glyphs, if defined by the font.
        EnableColorFont = 4
    }
    public enum GhostLineSpacingMode
    {
        //
        // Summary:
        //     Automatically positioned, if Microsoft.Graphics.Canvas.Text.CanvasTextFormat.LineSpacing
        //     is negative; otherwise, LineSpacing and LineSpacingBaseline are treated as a
        //     quantities in DIPs.
        Default = 0,
        //
        // Summary:
        //     Spacing is affected by the absolute value of LineSpacing, and vertical offset
        //     of the text is determined by LineSpacingBaseline. LineSpacing and LineSpacingBaseline
        //     are treated as a quantities in DIPs.
        Uniform = 1,
        //
        // Summary:
        //     Spacing is affected by the absolute value of LineSpacing, and vertical offset
        //     of the text is determined by LineSpacingBaseline. LineSpacing and LineSpacingBaseline
        //     are treated as a ratio of the default.
        Proportional = 2
    }

    public enum GhostFontStyle
    {
        //
        // Summary:
        //     Represents a normal font style.
        Normal = 0,
        //
        // Summary:
        //     Represents an oblique font style.
        Oblique = 1,
        //
        // Summary:
        //     Represents an italic font style.
        Italic = 2
    }
    public enum GhostFontStretch
    {
        //
        // Summary:
        //     No defined font stretch.
        Undefined = 0,
        //
        // Summary:
        //     An ultra-condensed font stretch (50% of normal).
        UltraCondensed = 1,
        //
        // Summary:
        //     An extra-condensed font stretch (62.5% of normal).
        ExtraCondensed = 2,
        //
        // Summary:
        //     A condensed font stretch (75% of normal).
        Condensed = 3,
        //
        // Summary:
        //     A semi-condensed font stretch (87.5% of normal).
        SemiCondensed = 4,
        //
        // Summary:
        //     The normal font stretch that all other font stretch values relate to (100%).
        Normal = 5,
        //
        // Summary:
        //     A semi-expanded font stretch (112.5% of normal).
        SemiExpanded = 6,
        //
        // Summary:
        //     An expanded font stretch (125% of normal).
        Expanded = 7,
        //
        // Summary:
        //     An extra-expanded font stretch (150% of normal).
        ExtraExpanded = 8,
        //
        // Summary:
        //     An ultra-expanded font stretch (200% of normal).
        UltraExpanded = 9
    }

    public enum GhostTextDirection
    {
        //
        // Summary:
        //     Text is read from left to right and lines flow from top to bottom.
        LeftToRightThenTopToBottom = 0,
        //
        // Summary:
        //     Text is read from right to left and lines flow from top to bottom.
        RightToLeftThenTopToBottom = 1,
        //
        // Summary:
        //     Text is read from left to right and lines flow from bottom to top.
        LeftToRightThenBottomToTop = 2,
        //
        // Summary:
        //     Text is read from right to left and lines flow from bottom to top.
        RightToLeftThenBottomToTop = 3,
        //
        // Summary:
        //     Text is read from top to bottom and lines flow from left to right.
        TopToBottomThenLeftToRight = 4,
        //
        // Summary:
        //     Text is read from bottom to top and lines flow from left to right.
        BottomToTopThenLeftToRight = 5,
        //
        // Summary:
        //     Text is read from top to bottom and lines flow from right to left.
        TopToBottomThenRightToLeft = 6,
        //
        // Summary:
        //     Text is read from bottom to top and lines flow from right to left.
        BottomToTopThenRightToLeft = 7
    }
}
