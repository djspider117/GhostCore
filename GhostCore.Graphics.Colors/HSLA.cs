using System;
using System.Globalization;
using System.Numerics;

namespace GhostCore.Graphics.Colors
{
    /// <summary>
    /// Represents a color in the Hue-Saturation-Lightness color space with an alpha value
    /// </summary>
    public struct HSLA : IEquatable<HSLA>
    {
        #region Public Fields

        /// <summary>
        /// The hue value. 0 <= H < 360
        /// </summary>
        public float H;

        /// <summary>
        /// The saturation value. 0 <= S <= 1
        /// </summary>
        public float S;

        /// <summary>
        /// The lightness value. 0 <= L <= 1
        /// </summary>
        public float L;

        /// <summary>
        /// The alpha value. 0 <= A <= 1
        /// </summary>
        public float A;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new hue-saturation-lightness-alpha color value.
        /// </summary>
        /// <param name="h">The angle of the hue. 0 <= H < 360.</param>
        /// <param name="s">The saturation value. 0 <= S <= 1</param>
        /// <param name="l">The lightness value. 0 <= L <= 1</param>
        /// <param name="a">The alpha value. 0 <= A <= 1</param>
        public HSLA(float h, float s, float l, float a)
        {
            if (!Interval.LeftIncRightExt(0, 360, h)) throw new ArgumentException("Hue value must be in the interval [0, 360)", nameof(h));
            if (!Interval.Inclusive(0, 1, s)) throw new ArgumentException("Saturation value must be in the interval [0, 1]", nameof(s));
            if (!Interval.Inclusive(0, 1, l)) throw new ArgumentException("Lightness value must be in the interval [0, 1]", nameof(s));
            if (!Interval.Inclusive(0, 1, a)) throw new ArgumentException("Alpha value must be in the interval [0, 1]", nameof(s));

            H = h;
            S = s;
            L = l;
            A = a;
        }

        #endregion

        #region Equals Override

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is HSLA hsla && Equals(hsla);

        /// <inheritdoc />
        public bool Equals(HSLA other) => H == other.H && S == other.S && L == other.L && A == other.A;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(H, S, L, A);

        /// <inheritdoc />
        public static bool operator ==(HSLA left, HSLA right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(HSLA left, HSLA right) => !left.Equals(right);

        #endregion

        #region Conversions

        /// <inheritdoc />
        public static implicit operator RGBA(HSLA hsla) => hsla.ToRGBA();

        /// <inheritdoc />
        public static implicit operator HSLA(RGBA rgba) => rgba.ToHSLA();

        /// <summary>
        /// Coverts the current color from HSL color space to RGB. The alpha value is maintained.
        /// </summary>
        /// <returns>A RGBA object representing the same color and alpha but in the RGB color space.</returns>
        public RGBA ToRGBA()
        {
            if (S == 0)
                return new RGBA(L, L, L, A);

            float v1, v2;
            float hue = H / 360;

            v2 = (L < 0.5) ? (L * (1 + S)) : ((L + S) - (L * S));
            v1 = 2 * L - v2;

            return new RGBA(
                HueToRGB(v1, v2, hue + (1.0f / 3)), //R
                HueToRGB(v1, v2, hue), //G
                HueToRGB(v1, v2, hue - (1.0f / 3)), //B
                A);
        }

        #endregion

        #region ToString

        /// <inheritdoc />
        public override string ToString()
        {
            FormattableString str = $"[HSLA (H={H} deg, S={S * 100}%, L={L * 100}%, A={A}))";
            return str.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the current color to a css valid hsla color. Example: hsla(213, 43%, 12%, 0.3)
        /// </summary>
        public string ToCSSString()
        {
            FormattableString str = $"hsla({H},{S * 100}%,{L * 100}%,{A})";
            return str.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region Helpers

        private static float HueToRGB(float v1, float v2, float vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;

            if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
            if ((2 * vH) < 1) return v2;
            if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

            return v1;
        }

        #endregion
    }
}
