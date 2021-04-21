using GhostCore.Animations.Core;

namespace GhostCore.Animations.Data
{
    public class Timeline : ITimeline
    {
        public PlayableWrapMode WrapMode { get; set; }
        public float Duration { get; set; }
    }
}
