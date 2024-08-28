using System.Numerics;

namespace GhostCore.Math
{
    public interface ISizedTransformable
    {
        Vector2 RenderOrigin { get; set; }
        Vector2 Offset { get; set; }
        Vector2 Scale { get; set; }
        float Rotation { get; set; }

        double Width { get; set; }
        double Height { get; set; }
    }
}
