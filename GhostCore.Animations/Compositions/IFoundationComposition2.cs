using Windows.Foundation;

namespace GhostCore.Animations
{
    public interface IFoundationComposition2
    {
        Size RenderSize { get; set; }
        float RenderScale { get; set; }
        float FrameRate { get; set; }
    }
}
