namespace GhostCore.Animations.Core
{
    public interface ITimeline : IPlayable
    {
        float Duration { get; set; }
    }
}
