using System.Numerics;

namespace GhostCore.Graphics.Colors
{
    /// <summary>
    /// Holds pre-defined HSL color spaces.
    /// </summary>
    public static class HSLColorSpaces
    {
        /// <summary>
        /// Pleasing color pallet. Not too saturated, not to dark. Perfectly balanced, as all things should be.s
        /// </summary>
        public static readonly HSLColorSpace Khronos = new HSLColorSpace(nameof(Khronos),
                                                                                    new Vector2(0, 360),
                                                                                    new Vector2(0.719f, 0.026f),
                                                                                    new Vector2(0.851f, 0.296f));

        /// <summary>
        /// Represents the full HSL color space gamut.
        /// </summary>
        public static readonly HSLColorSpace AllColors = new HSLColorSpace(nameof(AllColors),
                                                                                    new Vector2(0, 360),
                                                                                    new Vector2(0, 1f),
                                                                                    new Vector2(0f, 1f));
        /// <summary>
        /// Pastel colors.
        /// </summary>
        public static readonly HSLColorSpace Pastel = new HSLColorSpace(nameof(Pastel), new Vector2(0, 360),
                                                                                    new Vector2(0, 0.3f),
                                                                                    new Vector2(0.7f, 1f));

        /// <summary>
        /// Intense colors suitable
        /// </summary>
        public static readonly HSLColorSpace Pimp = new HSLColorSpace(nameof(Pimp), new Vector2(0, 360),
                                                                                  new Vector2(0.3f, 1f),
                                                                                  new Vector2(0.25f, 0.7f));
        /// <summary>
        /// Dark color shades.
        /// </summary>
        public static readonly HSLColorSpace Shades = new HSLColorSpace(nameof(Shades), new Vector2(0, 240),
                                                                                    new Vector2(0, 0.15f),
                                                                                    new Vector2(0, 1f));
        /// <summary>
        /// Tarnished color pallet, suitable for realistic usage.
        /// </summary>
        public static readonly HSLColorSpace Tarnish = new HSLColorSpace(nameof(Tarnish), new Vector2(0, 360),
                                                                                     new Vector2(0, 0.15f),
                                                                                     new Vector2(0.3f, 0.7f));

        public static readonly HSLColorSpace Intense = new HSLColorSpace(nameof(Intense), new Vector2(0, 360),
                                                                                     new Vector2(0.2f, 1f),
                                                                                     new Vector2(0.15f, 0.8f));

        public static readonly HSLColorSpace Fluo = new HSLColorSpace(nameof(Fluo), new Vector2(0, 300),
                                                                                  new Vector2(0.35f, 1f),
                                                                                  new Vector2(0.75f, 1f));
    }
}
