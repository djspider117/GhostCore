using System.Numerics;

namespace GhostCore.Animations.Core
{
    public class TransformData
    {
        public readonly static TransformData Default = new TransformData(Vector2.Zero, Vector2.Zero, Vector2.One, 0);

        public Vector2 Center { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        public TransformData()
        {

        }

        public TransformData(Vector2 center, Vector2 position, Vector2 scale, float rotation)
        {
            Center = center;
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }
    }
}
