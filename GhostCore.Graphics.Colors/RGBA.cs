using System;
using System.Globalization;

namespace GhostCore.Graphics.Colors
{
    /// <summary>
    /// Represents a color in the Red-Greed-Blue color space with an alpha value
    /// </summary>
    public struct RGBA : IEquatable<RGBA>
    {
        #region Public Fields

        /// <summary>
        /// The red value. 0 <= R <= 1
        /// </summary>
        public float R;

        /// <summary>
        /// The green value. 0 <= G <= 1
        /// </summary>
        public float G;

        /// <summary>
        /// The blue value. 0 <= B <= 1
        /// </summary>
        public float B;

        /// <summary>
        /// The alpha value. 0 <= A <= 1
        /// </summary>
        public float A;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new RGBA color value.
        /// </summary>
        /// <param name="r">The red value. 0 <= R <= 1</param>
        /// <param name="g">The green value. 0 <= G <= 1</param>
        /// <param name="b">The blue value. 0 <= B <= 1</param>
        /// <param name="a">The alpha value. 0 <= A <= 1</param>
        public RGBA(float r, float g, float b, float a)
        {
            if (!Interval.Inclusive(0, 1, r)) throw new ArgumentException("Red value must be in the interval [0, 360)", nameof(r));
            if (!Interval.Inclusive(0, 1, g)) throw new ArgumentException("Green value must be in the interval [0, 1]", nameof(g));
            if (!Interval.Inclusive(0, 1, b)) throw new ArgumentException("Blue value must be in the interval [0, 1]", nameof(b));
            if (!Interval.Inclusive(0, 1, a)) throw new ArgumentException("Alpha value must be in the interval [0, 1]", nameof(a));

            R = r;
            G = g;
            B = b;
            A = a;
        }

        #endregion

        #region Equals Override

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is RGBA rgba && Equals(rgba);
        }

        /// <inheritdoc />
        public bool Equals(RGBA other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A;
        }

        /// <inheritdoc />
        public static bool operator ==(RGBA left, RGBA right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(RGBA left, RGBA right)
        {
            return !(left.Equals(right));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hashCode = 1960784236;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            return hashCode;
        }


        #endregion

        #region Conversion

        /// <summary>
        /// Coverts the current color from RGB color space to HSL. The alpha value is maintained.
        /// </summary>
        /// <returns>A HSLA object representing the same color and alpha but in the HSL color space.</returns>
        public HSLA ToHSLA()
        {
            float min = Math.Min(Math.Min(R, G), B);
            float max = Math.Max(Math.Max(R, G), B);
            float delta = max - min;

            float h = 0;
            float s = 0;
            float l = (float)((max + min) / 2.0f);

            if (delta != 0)
            {
                if (l < 0.5f)
                {
                    s = (float)(delta / (max + min));
                }
                else
                {
                    s = (float)(delta / (2.0f - max - min));
                }


                if (R == max)
                {
                    h = (G - B) / delta;
                }
                else if (G == max)
                {
                    h = 2f + (B - R) / delta;
                }
                else if (B == max)
                {
                    h = 4f + (R - G) / delta;
                }
            }

            return new HSLA(h, s, l, A);
        }

        #endregion

        #region ToString

        /// <inheritdoc />
        public override string ToString() => $"[RGBA (R={R}, G={G}, B={B})";

        /// <summary>
        /// Converts the current color to a css valid rgba color. Example: rgba(141,254,215,0.5)
        /// </summary>
        public string ToCSSString()
        {
            FormattableString str = $"rgba({R * 255},{G * 255},{B * 255},{A})";
            return str.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the color into a valid hexstring color. Example #2ABF1E
        /// </summary>
        public string ToHexString()
        {
            FormattableString str = $"#{(byte)(R * 255):X2}{(byte)(G * 255):X2}{(byte)(B * 255):X2}";
            return str.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the color into a valid RGBA hexstring color. Example #FF2ABF1E
        /// </summary>
        public string ToARGBHexString()
        {
            FormattableString str = $"#{(byte)(A * 255):X2}{(byte)(R * 255):X2}{(byte)(G * 255):X2}{(byte)(B * 255):X2}";
            return str.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
