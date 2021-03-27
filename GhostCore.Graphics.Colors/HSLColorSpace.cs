using System.Numerics;

namespace GhostCore.Graphics.Colors
{
    /// <summary>
    /// Represents minimum and maximum values for an HSL color space.
    /// </summary>
    public struct HSLColorSpace
    {
        /// <summary>
        /// The name of the preset.
        /// </summary>
        public string Name;

        /// <summary>
        /// The hue range represented as a Vector2. X = min value, Y = max value.
        /// </summary>
        public Vector2 HueRange;

        /// <summary>
        /// The saturation range represented as a Vector2. X = min value, Y = max value.
        /// </summary>
        public Vector2 SaturationRange;

        /// <summary>
        /// The lightness range represented as a Vector2. X = min value, Y = max value.
        /// </summary>
        public Vector2 LightnessValue;

        /// <summary>
        /// Creates a new HSL color space.
        /// </summary>
        /// <param name="name">The name of the preset.</param>
        /// <param name="hueRange">The hue range.</param>
        /// <param name="saturationRange">The saturation range.</param>
        /// <param name="lightnessValue">The lightness range.</param>
        public HSLColorSpace(string name, Vector2 hueRange, Vector2 saturationRange, Vector2 lightnessValue)
        {
            Name = name;
            HueRange = hueRange;
            SaturationRange = saturationRange;
            LightnessValue = lightnessValue;
        }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
