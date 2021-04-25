using GhostCore.Graphics.Colors;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace GhostCore.Animations.Rendering
{
    public static class RGBAExtensions
    {
        public static Color ToWinUiColor(this RGBA rgba)
        {
            return new Color
            {
                R = (byte)Math.MathUtils.Clamp(rgba.R * byte.MaxValue, 0, byte.MaxValue),
                G = (byte)Math.MathUtils.Clamp(rgba.G * byte.MaxValue, 0, byte.MaxValue),
                B = (byte)Math.MathUtils.Clamp(rgba.B * byte.MaxValue, 0, byte.MaxValue),
                A = (byte)Math.MathUtils.Clamp(rgba.A * byte.MaxValue, 0, byte.MaxValue),
            };
        }

        public static SolidColorBrush ToSolidColorBrush(this RGBA rgba)
        {
            return new SolidColorBrush(rgba.ToWinUiColor());
        }

        public static RGBA ToRGBA(this SolidColorBrush scb)
        {
            return ToRGBA(scb.Color);
        }

        private static RGBA ToRGBA(this Color col)
        {
            return new RGBA(((float)col.R) / byte.MaxValue,
                            ((float)col.G) / byte.MaxValue,
                            ((float)col.B) / byte.MaxValue,
                            ((float)col.A) / byte.MaxValue);
        }
    }

}
