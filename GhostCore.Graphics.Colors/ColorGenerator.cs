using System;

namespace GhostCore.Graphics.Colors
{
    /// <summary>
    /// Utility class used to generate colors.
    /// </summary>
    public static class ColorGenerator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates a random pastel color with lightness greater than middle gray.
        /// </summary>
        /// <returns>An RGBA pastel color with alpha set to 1.</returns>
        public static RGBA RandomRGBPastelColor()
        {

            return new RGBA(
                (_random.Next(128) + 127) / byte.MaxValue,
                (_random.Next(128) + 127) / byte.MaxValue,
                (_random.Next(128) + 127) / byte.MaxValue,
                1);
        }

        /// <summary>
        /// Generates a random RGBA color.
        /// </summary>
        /// <param name="randomAlpha">True if the alpha should be random, false if the alpha should be 1. Default is false.</param>
        /// <returns>A random RGBA color.</returns>
        public static RGBA RandomRGBColor(bool randomAlpha = false)
        {
            return new RGBA((float)_random.NextDouble(),
                (float)_random.NextDouble(),
                (float)_random.NextDouble(),
                (float)(randomAlpha ? _random.NextDouble() : 1));
        }

        /// <summary>
        /// Generates a random HSLA color from the given color space with alpha set to 1.
        /// </summary>
        /// <param name="colorSpace">The color space preset.</param>
        public static HSLA RandomHSLColor(HSLColorSpace colorSpace)
        {
            return new HSLA(
                (float)(_random.NextDouble() * (colorSpace.HueRange.Y - colorSpace.HueRange.X) + colorSpace.HueRange.X),
                (float)(_random.NextDouble() * (colorSpace.SaturationRange.Y - colorSpace.SaturationRange.X) + colorSpace.SaturationRange.X),
                (float)(_random.NextDouble() * (colorSpace.LightnessValue.Y - colorSpace.LightnessValue.X) + colorSpace.LightnessValue.X),
                1);
        }

        /// <summary>
        /// Generates a pleasing random color grid based on the provided color space.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        /// <param name="preset">The color space preset.</param>
        /// <returns>A multi-dimensional array of HSLA values.</returns>
        public static HSLA[,] GenerateRandomColorGrid(int rows, int columns, HSLColorSpace preset)
        {
            var rv = new HSLA[rows, columns];
            for (int col = 0; col < columns; col++)
            {
                var h = (float)_random.NextDouble() * (preset.HueRange.Y - preset.HueRange.X) + preset.HueRange.X;
                for (int row = 0; row < rows; row++)
                {
                    var perc = ((float)row) / rows;
                    var l = perc * (preset.LightnessValue.Y - preset.LightnessValue.X) + preset.LightnessValue.X;
                    var s = perc * (preset.SaturationRange.Y - preset.SaturationRange.X) + preset.SaturationRange.X; ;
                    rv[row, col] = new HSLA(h, s, l, 1);
                }
            }

            return rv;
        }

        /// <summary>
        /// Generates a pleasing fixed color grid based on the provided color space.
        /// </summary>
        /// <remarks>
        /// Generates a matrix of HSLA colors, ordered by lightness from top to bottom and
        /// ordered by hue from left to right, starting at 0 hue (red) on the left.
        /// </remarks>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        /// <param name="preset">The color space preset.</param>
        /// <returns>A multi-dimensional array of HSLA values.</returns>
        public static HSLA[,] GenerateUniformColorGrid(int rows, int columns, HSLColorSpace preset)
        {
            var rv = new HSLA[rows, columns];
            for (int col = 0; col < columns; col++)
            {
                var h = ((float)col / columns) * (preset.HueRange.Y - preset.HueRange.X) + preset.HueRange.X;
                for (int row = 0; row < rows; row++)
                {
                    var perc = ((float)row) / rows;
                    var l = perc * (preset.LightnessValue.Y - preset.LightnessValue.X) + preset.LightnessValue.X;
                    var s = perc * (preset.SaturationRange.Y - preset.SaturationRange.X) + preset.SaturationRange.X; ;
                    rv[row, col] = new HSLA(h, s, l, 1);
                }
            }

            return rv;
        }
    }
}
