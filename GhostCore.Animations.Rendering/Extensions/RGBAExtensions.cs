using GhostCore.Graphics.Colors;
using Windows.UI;

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
    }

}
