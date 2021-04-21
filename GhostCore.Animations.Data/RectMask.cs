using GhostCore.Animations.Core;
using System.Numerics;

namespace GhostCore.Animations.Data
{
    public class RectMask : IMask
    {
        public Vector2 RelativeOffset { get; set; }
        public Vector2 Anchor { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
