namespace GhostCore.Graphics.Colors
{
    internal static class Interval
    {
        public static bool LeftIncRightExt(float min, float max, float value) => min <= value && value < max;
        public static bool LeftExtRightInc(float min, float max, float value) => min < value && value <= max;

        public static bool Exclusive(float min, float max, float value) => min < value && value < max;
        public static bool Inclusive(float min, float max, float value) => min <= value && value <= max;
    }
}
